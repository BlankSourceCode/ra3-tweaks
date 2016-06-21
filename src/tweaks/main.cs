using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace RA3Tweaks.Tweaks
{
    public class Main
    {
        /// <summary>
        /// Add the RA3-Tweaks menu into the game
        /// </summary>
        [Tweak("MenuManager", "GatherMenus", false)]
        public static void GatherMenus(MenuManager instance)
        {
            Debug.Log("Adding RA3-Tweaks Menu...");
            MenuMain mainMenu = instance.menus.Where(m => m.name == "MenuMain").Select(m => m.GetComponentInChildren<MenuMain>()).First();
            if (mainMenu != null)
            {
                // Create the new tweaks menu from the asset bundle
                var tweakMenu = AssetsHandler.CreateAsset<GameObject>("MenuTweaks");
                tweakMenu.name = "MenuTweaks";
                tweakMenu.transform.GetChild(0).gameObject.AddComponent<MenuTweaks>();
                instance.AddNewMenu(tweakMenu);

                Debug.Log(mainMenu.mHeaderText.font);

                // Add a button on the main menu to open the tweak options
                var button = AssetsHandler.CreateAsset<Button>("MenuButton");
                button.onClick.AddListener(() =>
                {
                    Debug.Log("Opening Tweaks Menu...");
                    var menuBase = tweakMenu.GetComponentInChildren<MenuBase>();
                    var gameLogic = UnityEngine.Object.FindObjectOfType(typeof(GameLogic)) as GameLogic;
                    gameLogic.MainMode = global::GameLogic.MAINMODE.SETTINGS;
                    instance.Pop();
                    bool result = instance.Push("MenuTweaks");
                    Debug.Log(result);
                });

                button.transform.SetParent(mainMenu.transform, false);
            }
        }

        /// <summary>
        /// Add new custom components to the start of the component list
        /// </summary>
        [Tweak("ComponentInfoList", "LoadFromResource", "ComponentInfoList::LoadFromJsonString")]
        [Tweak("ComponentInfoList", "LoadFromFile", "ComponentInfoList::LoadFromJsonString")]
        public static int LoadFromJsonString(string jsonString)
        {
            Debug.Log("Loading RA3-Tweaks Components...");

            // Load up the custom components.json file
            string newJson = "";
            int count = 0;
            string[] allAssets = AssetsHandler.Bundle.GetAllAssetNames();
            foreach(var a in allAssets)
            {
                if (a.EndsWith("/component.json", StringComparison.OrdinalIgnoreCase))
                {
                    // Append this component to the json
                    string compJson = AssetsHandler.Bundle.LoadAsset<TextAsset>(a).text;
                    string componentName = a.Substring(0, a.LastIndexOf('/') + 1);

                    // Prefix the prefab name with something we can identify later
                    Regex r = new Regex("\"componentprefabname\"\\s*:\\s*\"");
                    compJson = r.Replace(compJson, "\"componentprefabname\": \"RA3Tweaks/" + componentName);

                    Regex r2 = new Regex("\"thumbnail\"\\s*:\\s*\"");
                    compJson = r2.Replace(compJson, "\"thumbnail\": \"RA3Tweaks/" + componentName);

                    Debug.Log("Component.json loaded as:\r\n" + compJson);

                    newJson += (count > 0 ? ",\r\n" : "") + compJson;
                    count++;
                }
            }

            if (count > 0)
            {
                // Strip off the mismatched [ ] and combine the files
                int endIndex = jsonString.LastIndexOf(']');
                if (endIndex > -1)
                {
                    jsonString = jsonString.Substring(0, endIndex - 1) + ",\r\n" + newJson + "\r\n]";
                    Debug.Log(jsonString);
                }
            }


            // Call the original load with the new modified json
            ComponentInfoList instance = typeof(ComponentInfoList).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as ComponentInfoList;
            return instance.LoadFromJsonString(jsonString);
        }

        /// <summary>
        /// Load up the custom thumbnails
        /// </summary>
        /// <param name="instance"></param>
        [Tweak("ComponentInfoList", "LoadThumbnails")]
        public static TweakReturnVoid LoadThumbnails(ComponentInfoList instance)
        {
            Debug.Log("Loading thumbnails...");

            ComponentInfo[] array = instance.mItems;
            for (int i = 0; i < array.Length; i++)
            {
                ComponentInfo componentInfo = array[i];
                if (componentInfo.mThumbnailName != null && componentInfo.mThumbnailName.StartsWith("RA3Tweaks/"))
                {
                    // Load the new thumbnail
                    Debug.Log("Loading thumbnail for " + componentInfo.mThumbnailName);
                    componentInfo.mThumbnail = AssetsHandler.Bundle.LoadAsset<Sprite>(componentInfo.mThumbnailName.Replace("RA3Tweaks/", ""));
                }
                else
                {
                    // Do the default loading
                    string name = "UI/ComponentThumbs/" + componentInfo.mThumbnailName;
                    componentInfo.mThumbnail = Resources.Load<Sprite>(name);
                }
            }

            // Return 'prevent default' as true to skip over the original code
            return new TweakReturnVoid(true);
        }

        /// <summary>
        /// Process loading custom components
        /// </summary>
        [Tweak("Bot", "LoadBotComponents", "UnityEngine.Resources::Load")]
        [Tweak("MenuBotLab", "CreateChassisNew", "UnityEngine.Resources::Load")]
        [Tweak("MenuBotLab", "CreateChassis", "UnityEngine.Resources::Load")]
        [Tweak("MenuBotLab", "CreateComponent", "UnityEngine.Resources::Load")]
        public static UnityEngine.Object LoadResources(string name)
        {
            if (name.StartsWith("ComponentPrefabs/RA3Tweaks/"))
            {
                Debug.Log("Loading " + name + " resource from RA3-Tweaks...");

                // Load this up from out asset bundle
                GameObject component = AssetsHandler.Bundle.LoadAsset<GameObject>(name.Replace("ComponentPrefabs/RA3Tweaks/", ""));
                if (component.GetComponent<BotCompBase>() == null)
                {
                    Debug.Log("Component found, modifying...");

                    // Update the name
                    component.name = name.Replace("ComponentPrefabs/", "");

                    // Find the TweakInfo for this component
                    int apId = 0;
                    for (int i = 0; i < component.transform.childCount; i++)
                    {
                        var t = component.transform.GetChild(i);
                        if (t.name.Equals("TweakInfo", StringComparison.OrdinalIgnoreCase))
                        {
                            // Search the TweakInfo for convertable classes
                            for (int j = 0; j < t.childCount; j++)
                            {
                                // Replace the transforms with instances since asset bundles can't transfer custom properties
                                var info = t.GetChild(j);
                                Type infoType = typeof(BotCompBase).Assembly.GetType(info.name);
                                if (infoType != null)
                                {
                                    // Add this type of component
                                    var newComponent = component.AddComponent(infoType);

                                    if (newComponent is AttachPoint)
                                    {
                                        var ap = (AttachPoint)newComponent;
                                        ap.mId = apId;
                                        ap.mOffset = info.transform.localPosition;
                                        ap.mParent = component.transform;
                                        ap.mNormal = info.transform.localRotation * Vector3.up;

                                        // Read in the plug/socket info from children transforms
                                        ap.mIsPlug = (ap.transform.FindChildByName("Plug") != null);
                                        var socket = ap.transform.FindChildByName("Socket");
                                        ap.mIsSocket = (socket != null);
                                        ap.mSocketType = SocketType.ST_FIXED;
                                        if (socket != null && socket.childCount > 0)
                                        {
                                            switch (socket.GetChild(0).name.ToLowerInvariant())
                                            {
                                                case "fixed":
                                                    ap.mSocketType = SocketType.ST_FIXED;
                                                    break;
                                                case "motor":
                                                    ap.mSocketType = SocketType.ST_MOTOR;
                                                    break;
                                                case "swing":
                                                    ap.mSocketType = SocketType.ST_SWING;
                                                    break;
                                                case "wheel":
                                                    ap.mSocketType = SocketType.ST_WHEEL;
                                                    break;
                                            }
                                        }
                                        apId++;
                                    }
                                    else if (newComponent is BotCompBase)
                                    {
                                        ((BotCompBase)newComponent).mPrefabName = name.Replace("ComponentPrefabs/", "");
                                    }
                                }
                            }

                            GameObject.Destroy(t.gameObject);
                            break;
                        }
                    }
                }

                TweakHelpers.LogTransform((component as GameObject).transform, "STOCK " + name);
                return component;
            }

            // Fallback to regular loading
            var stockComponent = Resources.Load(name);
            TweakHelpers.LogTransform((stockComponent as GameObject).transform, "STOCK " + name);
            return stockComponent;
        }

        /// <summary>
        /// Try to prevent tweaked bots from being uploaded to steam, 
        /// since they could have custom components that will break other people
        /// </summary>
        [Tweak("MenuWorkshopUpload", "Start", false)]
        [Tweak("MenuWorkshopUpload", "OnEnable", false)]
        public static void OnEnable(MenuWorkshopUpload instance)
        {
            Debug.Log("No workshop upload allowed");
            instance.OnExit();
        }

        [Tweak("MenuBotLab", "ChassisExtrudeUpdate", false)]
        public static void ChassisExtrudeUpdate(MenuBotLab instance)
        {
            // Check that we are drawing the base plate
            if (instance.GetPrivateField<MenuBotLab.EXTRUDE_STATE>("mExtrudeState") == MenuBotLab.EXTRUDE_STATE.DEFINE_SECTION)
            {
                var count = instance.GetPrivateField<List<Vector3>>("mVertices").Count;
                if (count > 0)
                {
                    // Add a new end point that follows the mouse cursor (like RA2 did!)
                    instance.mBasePlateLines.SetVertexCount(count + 1);
                    instance.mBasePlateLines.SetPosition(count, instance.mCursorIndicator.transform.localPosition);
                }
            }
        }
    }
}
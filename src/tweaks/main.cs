using System;
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
            string path = Path.Combine(AssetsHandler.AssetDirectory, @".\components.json");
            if (System.IO.File.Exists(path))
            {
                newJson = System.IO.File.ReadAllText(path);
            }
            else
            {
                newJson = AssetsHandler.Bundle.LoadAsset<TextAsset>("Components").text;
            }

            if (newJson != null)
            {
                // Prefix the prefab name with something we can identify later
                Regex r = new Regex("\"componentprefabname\"\\s*:\\s*\"");
                newJson = r.Replace(newJson, "\"componentprefabname\": \"RA3Tweaks/");

                Regex r2 = new Regex("\"thumbnail\"\\s*:\\s*\"");
                newJson = r2.Replace(newJson, "\"thumbnail\": \"RA3Tweaks/");

                Debug.Log("Components.json loaded as:\r\n" + newJson);

                // Strip off the mismatched [ ] and combine the files
                int startIndex = newJson.IndexOf('[');
                int endIndex = jsonString.LastIndexOf(']');
                if (startIndex > -1 && endIndex > -1)
                {
                    jsonString = jsonString.Substring(0, endIndex - 1) + ",\r\n" + newJson.Substring(startIndex + 1);
                }
            }

            // Call the original load with the new modified json
            ComponentInfoList instance = typeof(ComponentInfoList).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as ComponentInfoList;
            int count = instance.LoadFromJsonString(jsonString);
            Debug.Log("Total components loaded = " + count);
            return count;
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
                Debug.Log("Loading "+ name + " resource from RA3-Tweaks...");
                
                // Load this up from out asset bundle
                GameObject component = AssetsHandler.Bundle.LoadAsset<GameObject>(name.Replace("ComponentPrefabs/RA3Tweaks/", ""));
                if (component.GetComponent<BotCompBase>() == null)
                {
                    Debug.Log("Component found, modifying...");

                    // Update the name
                    component.name = name.Replace("ComponentPrefabs/", "");

                    // Add missing components since the asset bundle won't contain them
                    component.AddComponent<CollisionHandler>();
                    component.AddComponent<InterActMaterial>();
                    var botcomp = component.AddComponent<BotCompBase>();
                    botcomp.mPrefabName = name.Replace("ComponentPrefabs/", "");

                    // Replace the transforms named 'AttachPoint' with AttachPoint instances
                    int apId = 0;
                    var all = component.GetComponentsInChildren<Transform>();
                    foreach (var t in all)
                    {
                        if (t.name == "AttachPoint")
                        {
                            // TODO: Allow this data to be specified in the component somehow
                            var ap = component.AddComponent<AttachPoint>();
                            ap.mId = apId;
                            ap.mSocketType = SocketType.ST_FIXED;
                            ap.mOffset = t.transform.localPosition;
                            ap.mParent = component.transform;
                            ap.mNormal = t.transform.localRotation * Vector3.up;
                            ap.mIsPlug = true;
                            ap.mIsSocket = true;
                            GameObject.Destroy(t.gameObject);
                            apId++;
                        }
                    }
                }

                return component;
            }
            
            // Fallback to regular loading
            return Resources.Load(name);
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
    }
}
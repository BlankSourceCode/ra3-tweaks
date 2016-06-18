using System;
using System.Linq;
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

        [Tweak("ComponentInfoList", "LoadFromJsonString")]
        public static TweakReturn<int> LoadFromJsonString(ComponentInfoList instance, string jsonString)
        {
            Plugin.Initialize();

            return new TweakReturn<int>(false, 0);
        }
    }
}
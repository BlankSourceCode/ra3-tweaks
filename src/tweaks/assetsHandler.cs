using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RA3Tweaks
{
    public static class AssetsHandler
    {
        private static AssetBundle bundleInstance;

        public static string AssetDirectory
        {
            get
            {
                return @".\RobotArena3_Data\Managed\ra3-tweaks";
            }
        }

        public static AssetBundle Bundle
        {
            get
            {
                if (AssetsHandler.bundleInstance == null)
                {
                    string path = Path.Combine(AssetsHandler.AssetDirectory, @".\ra3-tweaks");
                    Debug.Log("Loading RA3-Tweaks from path: " + path);
                    AssetsHandler.bundleInstance = AssetBundle.LoadFromFile(path);
                }

                return bundleInstance;
            }
        }

        public static T CreateAsset<T>(string name) where T : UnityEngine.Object
        {
            var prefab = AssetsHandler.Bundle.LoadAsset<GameObject>(name);
            if (prefab != null)
            {
                var item = GameObject.Instantiate(prefab);
                if (typeof(T) == typeof(GameObject))
                {
                    Debug.Log(name + " Loaded.");
                    return item as T;
                }

                var component = item.GetComponent<T>();
                Debug.Log(name + " Loaded as " + component.ToString());
                return component;
            }

            return null;
        }
    }
}
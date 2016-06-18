using System;
using System.IO;
using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build RA3-Tweaks AssetBundles...", priority = 10000)]
    static void BuildAllAssetBundles()
    {
        string outputPath = "../../out/bundles";
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
        
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None);
    }
}
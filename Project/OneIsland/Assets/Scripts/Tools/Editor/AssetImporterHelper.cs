using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AssetImporterHelper : AssetPostprocessor
{
    public static readonly string BaseDir = "Assets/UI";
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] moveAssets, string[] moveFromAssePaths)
    {
        foreach (var importedAsset in importedAssets)
        {
            MarkAssetBundle(importedAsset);
        }
        foreach (var moveAsset in moveAssets)
        {
            MarkAssetBundle(moveAsset);
        }
    }

    private static void MarkAssetBundle(string asset)
    {
        if (asset.EndsWith(".cs"))
            return;

        var importer = AssetImporter.GetAtPath(asset);
        if (!importer)
            return;

        var bundleName = GetAssetBundleName(asset);
        bundleName = bundleName.Replace(" ", "");
        bundleName = bundleName.Replace("——", "-");

        if (!string.Equals(importer.assetBundleName, bundleName))
        {
            importer.assetBundleName = bundleName;
            importer.SaveAndReimport();
        }
    }

    private static string GetAssetBundleName(string asset)
    {
        string bundleName = string.Empty;
        if (string.IsNullOrEmpty(bundleName))
            bundleName = TryGetPrefabName(asset);

        return bundleName;
    }

    private static string TryGetPrefabName(string asset)
    {
        if (asset.EndsWith(".prefab"))
        {
            Debug.Log(asset);
            return GetRelativeDirPath(asset, BaseDir) + "_prefab";
        }
        return string.Empty;
    }

    private static string GetRelativeDirPath(string path, string basePath)
    {
        var relativePath = path.Substring(basePath.Length + 1);
        return Path.GetDirectoryName(relativePath).ToLower().Replace("\\", "/");
    }
}

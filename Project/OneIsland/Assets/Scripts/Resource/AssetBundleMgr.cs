using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetBundleMgr
{
    public Object LoadAsset<T>(string bundle, string asset)
    {
        System.Type t = typeof(T);

        Object obj = null;
#if UNITY_EDITOR
        // obj = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/" + uri, t);
#else
        // resources.bytes的第一级目录为AssetBundle
        var abName = uri.Substring(0, uri.IndexOf("/")).ToLower() + ".bundle";
        var fname = Path.GetFileName(uri);
        AssetBundle ab = null;
        if (File.Exists(updatePath + "/" + fname))
        {
            // 热更的资源，是一个独立的AssetBundle文件，以fname为文件名
            ab = LoadAssetBundle(fname);
        }
        else
        {
            ab = LoadAssetBundle(abName);
        }
        if (null != ab)
        {
            obj = ab.LoadAsset<Object>("Assets/GameRes/" + uri);
        }
#endif

        if (null != obj)
        {
            // m_assets[uri] = obj;
        }
        return obj;
    }

    // public AssetBundle LoadAssetBundle(string abName)
    // {
    //     if (m_bundles.ContainsKey(abName))
    //         return m_bundles[abName];


    //     AssetBundle bundle = null;
    //     if (File.Exists(updatePath + "/" + abName))
    //     {
    //         // 优先从update目录（热更新目录）中查找资源
    //         bundle = AssetBundle.LoadFromFile(updatePath + abName);
    //     }
    //     else if (File.Exists(extPath + "/" + abName))
    //     {
    //         // 从拓展包目录加载资源
    //         bundle = AssetBundle.LoadFromFile(extPath + abName);
    //     }
    //     else
    //     {
    //         bundle = AssetBundle.LoadFromFile(internalPath + abName);
    //     }

    //     if (null != bundle)
    //     {
    //         m_bundles[abName] = bundle;
    //         GameLogger.Log("LoadAssetBundle Ok, abName: " + abName);
    //     }

    //     return bundle;
    // }


    protected System.Type GetAssetType(string uri)
    {
        if (uri.EndsWith(".prefab"))
        {
            return typeof(GameObject);
        }
        else if (uri.EndsWith(".ogg") || uri.EndsWith(".wav"))
        {
            return typeof(AudioClip);
        }
        else if (uri.EndsWith(".spriteatlas"))
        {
            return typeof(UnityEngine.U2D.SpriteAtlas);
        }
        else if (uri.EndsWith(".mat"))
        {
            return typeof(Material);
        }
        else if (uri.EndsWith(".anim"))
        {
            return typeof(AnimationClip);
        }
        return typeof(AssetBundle);
    }

    private static AssetBundleMgr s_instance;
    public static AssetBundleMgr instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new AssetBundleMgr();
            return s_instance;
        }
    }
}

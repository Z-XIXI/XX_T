using UnityEngine;

public class ResourceManager
{
    public T GetAsset<T>(string bundle, string asset) where T : Object
    {
        var obj = AssetBundleMgr.instance.LoadAsset<T>(bundle, asset);
        if (null == obj)
        {
            GameLogger.LogError("Instantiate Error, bundle:" + bundle + " asset:" + asset);
            return default(T);
        }

        if (typeof(T) == typeof(GameObject))
        {
            T go = Object.Instantiate(obj) as T;
            return go;
        }
        else
        {
            return obj as T;
        }
    }

    private static ResourceManager s_instance;
    public static ResourceManager instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new ResourceManager();
            return s_instance;
        }
    }
}
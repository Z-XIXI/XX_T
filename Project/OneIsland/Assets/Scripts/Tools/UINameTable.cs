using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UINameTable : MonoBehaviour
{
    [System.Serializable]
    public struct StringGameObjectPair
    {
        public string key;
        public GameObject value;
    }
    public List<StringGameObjectPair> nodeList;

    private Dictionary<string, GameObject> _itemDic;

    public Dictionary<string, GameObject> ItemDic
    {
        get
        {
            if (this._itemDic == null)
            {
                this._itemDic = new Dictionary<string, GameObject>(StringComparer.Ordinal);
                if (this.nodeList != null)
                {
                    foreach (StringGameObjectPair item in this.nodeList)
                    {
                        this._itemDic.Add(item.key, item.value);
                    }
                }
            }
            return this._itemDic;
        }
    }

    public GameObject Find(string key)
    {
        GameObject obj;
        if (this.ItemDic.TryGetValue(key, out obj))
        {
            return obj;
        }
        return null;
    }

    public bool Add(string key, GameObject obj)
    {
        if (this.ItemDic.ContainsKey(key))
        {
            return false;
        }
        else
        {
            if (null == this.nodeList)
                this.nodeList = new List<StringGameObjectPair>();
            this._itemDic.Add(key, obj);
            UINameTable.StringGameObjectPair pair = default(UINameTable.StringGameObjectPair);
            pair.key = key;
            pair.value = obj;
            this.nodeList.Add(pair);
            return true;
        }
    }

    public UINameTable.StringGameObjectPair[] Search(string name)
    {
        List<UINameTable.StringGameObjectPair> results = new List<StringGameObjectPair>();
        foreach (UINameTable.StringGameObjectPair item in this.nodeList)
        {
            if (item.key.Contains(name))
            {
                results.Add(item);
            }
        }
        return results.ToArray();
    }
}

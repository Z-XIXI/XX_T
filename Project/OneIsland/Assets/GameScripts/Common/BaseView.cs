using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class BaseView : IView
{
    protected virtual string viewName { get; }
    protected virtual string assetName { get; }
    protected virtual string bundleName { get; }
    private bool isOpen;
    private Dictionary<string, GameObject> nodeList;
    protected Dictionary<string, GameObject> NodeList
    {
        get { return nodeList;  }
        set { nodeList = value; }
    }
    protected BaseView()
    {
        InitView();
        //ViewManager.Instance.
    }
    protected virtual void InitView()
    {
        Debug.Log("viewName" + viewName);
        ViewManager.Instance.RegisterView(viewName, this);
    }

    public virtual void OpenView()
    {
        
    }

    public virtual void CloseView()
    {

    }
    public async void Open()
    {
        if (isOpen)
            return;

        bool exists = await AddressableManager.Instance.CheckIfAssetExists(assetName);
        if (!exists)
        {
            Debug.LogError($"Asset {assetName} does not exist!");
            return;
        }

        var obj = await AddressableManager.Instance.InstantiateAsync(assetName, ViewManager.Instance.UIViewCanvas);

        UINameTable uiNameTab = obj.GetComponent<UINameTable>();
        if (null != uiNameTab)
        {
            NodeList = uiNameTab.ItemDic;
        }
        LoadCallback();
    }
    public async void Close()
    {
        //AddressableManager.Instance.ReleaseInstance<assetName>();
    }
    public bool IsOpen()
    {
        return this.isOpen;
    }

    protected T GetComponentSelf<T>(GameObject gameObject)
    {
        return gameObject.GetComponent<T>();
    }

    protected virtual void LoadCallback()
    {

    }
    protected virtual void OpenCallback()
    {

    }

}
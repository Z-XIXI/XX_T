using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class BaseView : IView
{
    protected virtual string ViewName { get; }
    protected virtual string AssetName { get; }
    protected virtual string BundleName { get; }
    private bool _isOpen;
    private GameObject _viewGameObject;
    private Dictionary<string, GameObject> _nodeList;
    protected Dictionary<string, GameObject> NodeList{ get => _nodeList; set => _nodeList = value;}

    protected BaseView()
    {
        InitView();
        //ViewManager.Instance.
    }
    protected virtual void InitView()
    {
        ViewManager.Instance.RegisterView(ViewName, this);
    }

    public virtual void OpenView()
    {
        
    }

    public virtual void CloseView()
    {

    }
    public async void Open()
    {
        if (this._isOpen)
            return;

        bool exists = await AddressableManager.Instance.CheckIfAssetExists(AssetName);
        if (!exists)
        {
            Debug.LogError($"Asset {AssetName} does not exist!");
            return;
        }

        var obj = await AddressableManager.Instance.InstantiateAsync(AssetName, ViewManager.Instance.UIViewCanvas);
        _viewGameObject = obj;
        UINameTable uiNameTab = obj.GetComponent<UINameTable>();
        if (null != uiNameTab)
        {
            NodeList = uiNameTab.ItemDic;
        }
        this.LoadCallback();
    }
    public void Close()
    {
        
    }
    public void Relese()
    {
        AddressableManager.Instance.ReleaseInstance(_viewGameObject);
    }
    public bool IsOpen()
    {
        return this._isOpen;
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
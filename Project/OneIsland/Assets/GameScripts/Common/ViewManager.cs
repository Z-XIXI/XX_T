using System.Collections.Generic;
using UnityEngine;

public class ViewManager : SingletonClass<ViewManager>
{
    private Dictionary<string, IView> _views = new Dictionary<string, IView>();
    /// <summary>
    /// �򿪽���
    /// </summary>
    public void Open(string viewName)
    {
        IView view = this.GetView(viewName);
        if (null == view)
            Debug.LogErrorFormat("���Դ򿪲����ڵĽ���{0}", viewName);

        if (!view.IsOpen())
        {
            view.Open();
        }
    }
    /// <summary>
    /// ��ȡ����
    /// </summary>
    public IView GetView(string viewName)
    {
        return _views.TryGetValue(viewName, out IView view) ? view : null;
    }
    public void RegisterView(string viewName, IView view)
    {
        if (!_views.ContainsKey(viewName))
        {
            _views.Add(viewName, view);
        }
    }
    private Transform _uiViewCanvas;
    public Transform UIViewCanvas { get { return _uiViewCanvas; } }
    public void Init(GameObject tempUIViewCanvas)
    {
        _uiViewCanvas = tempUIViewCanvas.transform;
    }
}
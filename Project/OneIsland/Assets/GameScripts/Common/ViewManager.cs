using System.Collections.Generic;
using UnityEngine;

public class ViewManager
{
    private ViewManager(){
    }
    private Dictionary<string, IView> _views = new Dictionary<string, IView>();
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
    private static ViewManager instance;
    public static ViewManager Instance
    {
        get
        {
            if (null == instance)
                instance = new ViewManager();
            return instance;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject UIViewCanvas;
    void Awake()
    {
        DontDestroyOnLoad(this);
        GameUIController.Instance.Init();
        ViewManager.Instance.Init(UIViewCanvas);
    }
    private void OnDestroy()
    {
        GameUIController.Instance.Delete();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            IView view = ViewManager.Instance.GetView(ViewName.MainOperaListView);
            if (null != view)
            {
                view.Open();
            }
        }
    }
}

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
        GameDataManager.Instance.LoadGameData();
    }
    private void OnDestroy()
    {
        GameUIController.Instance.Delete();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            IView view = ViewManager.Instance.GetView(ViewNameEnum.MainOperaListView);
            if (null != view)
            {
                view.Open();
            }
        }
    }
    void OnApplicationQuit()
    {
        // 游戏退出时自动保存
        GameDataManager.Instance.SaveGameData();
    }
}

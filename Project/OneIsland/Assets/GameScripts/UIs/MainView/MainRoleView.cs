using UnityEngine;
using UnityEngine.EventSystems;

public class MainRoleView : BaseView
{
    protected override string viewName { get; } = ViewName.MainRoleView;
    protected override string assetName { get; } = ViewName.MainRoleView;
    protected override string bundleName { get; } = ViewName.MainRoleView;
    public MainRoleView() : base()
    {
        
    }

    protected override void LoadCallback()
    {
        UniversalEventHandler roleEventHandle = NodeList["btn_role"].GetComponent<UniversalEventHandler>();
        roleEventHandle.OnRightClickEvent = OnRightClickRoleEvent;
    }

    private void OnRightClickRoleEvent(PointerEventData eventData)
    {
        IView view = ViewManager.Instance.GetView(ViewName.MainOperaListView);
        if (!view.IsOpen())
        {
            view.Open();
        }
    }
    //public GameObject OperaBtnPanel;
    //public GameObject WishBottlePanel;
    //public TMP_InputField InputWish;

    //public void Awake()
    //{
    //    OperaBtnPanel.SetActive(false);
    //    WishBottlePanel.SetActive(false);
    //}

    ///// <summary>
    ///// 点击人物
    ///// </summary>
    //public void OnClickRole()
    //{
    //    OperaBtnPanel.SetActive(true);
    //}

    ///// <summary>
    ///// 点击许愿瓶
    ///// </summary>
    //public void OnClickWishBottle()
    //{
    //    OperaBtnPanel.SetActive(false);
    //    WishBottlePanel.SetActive(true);
    //}

    ///// <summary>
    ///// 点击许愿
    ///// </summary>
    //public void OnClickWish()
    //{
    //    string wishStr = InputWish.text;
    //    Debug.LogError("wishStr " + wishStr);
    //}
}

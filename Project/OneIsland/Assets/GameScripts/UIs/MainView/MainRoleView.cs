using UnityEngine;

public class MainRoleView : BaseView
{
    protected override string viewName { get; } = ViewName.MainRoleView;
    protected override string assetName { get; } = ViewName.MainRoleView;
    protected override string bundleName { get; } = ViewName.MainRoleView;
    protected override void InitView()
    {
        base.InitView();
    }

    protected override void OpenCallback()
    {
        Debug.Log("Open");
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

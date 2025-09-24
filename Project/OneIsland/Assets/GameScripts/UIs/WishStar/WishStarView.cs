using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WishStarView : BaseView
{
    protected override string ViewName { get; } = ViewNameEnum.WishStarView;
    protected override string AssetName { get; } = ViewNameEnum.WishStarView;
    protected override string BundleName { get; } = ViewNameEnum.WishStarView;
    public WishStarView() : base()
    {

    }

    private GameObject wishing_panel;
    private GameObject normal_panel;
    private TextMeshProUGUI txt_wish_time;
    private InputField input_wish;
    protected override void LoadCallback()
    {
        wishing_panel = NodeList["wishing_panel"];
        normal_panel = NodeList["normal_panel"];
        txt_wish_time = NodeList["txt_wish_time"].GetComponent<TextMeshProUGUI>();
        input_wish = NodeList["input_wish"].GetComponent<InputField>();

        Debug.Log(wishing_panel == null);
        Debug.Log(NodeList["wishing_panel"] == null);

        Button btn_wish = NodeList["btn_wish"].GetComponent<Button>();
        btn_wish.onClick.AddListener(OnClickWish);

        this.UpdateShowPanel();
    }

    private void UpdateShowPanel()
    {
        WishStarPanelType curShowType = WishStarModel.Instance.GetWishStarShowPanel();
        wishing_panel.SetActive(curShowType == WishStarPanelType.Wishing);
        normal_panel.SetActive(curShowType == WishStarPanelType.Normal);

        if (curShowType == WishStarPanelType.Normal)
            this.UpdateNoramlPanelShow();
        else if (curShowType == WishStarPanelType.Wishing)
            this.UpdateWishingPanelShow();
    }
    /// <summary>
    /// 普通界面
    /// </summary>
    private void UpdateNoramlPanelShow()
    {

    }
    /// <summary>
    /// 许愿ing界面
    /// </summary>
    private void UpdateWishingPanelShow()
    {

    }
    /// <summary>
    /// 点击许愿
    /// </summary>
    private void OnClickWish()
    {
        string wishItemName = input_wish.text;
        //WishStarCtrl.Instance
    }
}
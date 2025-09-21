using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static MainUIModel;

public class MainOperaListView : BaseView
{
    protected override string viewName { get; } = ViewName.MainOperaListView;
    protected override string assetName { get; } = ViewName.MainOperaListView;
    protected override string bundleName { get; } = ViewName.MainOperaListView;
    /// <summary>
    /// 操作列表数据
    /// </summary>
    private List<OperaItemSO> operaListData = new List<OperaItemSO>();
    /// <summary>
    /// 操作列表
    /// </summary>
    private Dictionary<int, OperaListItem> operaListItems;
    protected override void InitView()
    {
        base.InitView();
    }

    protected override void LoadCallback()
    {
        //操作列表
        operaListItems = new Dictionary<int, OperaListItem>();
        RecyclableScrollRect list = NodeList["opera_list"].GetComponent<RecyclableScrollRect>();
        list.ItemCallback = OperaListItemCreateCall;
        operaListData = MainUIModel.Instance.OperaListDatas;
        list.DataCount = operaListData.Count;

    }

    private void OperaListItemCreateCall(RectTransform item, int rowIndex)
    {
        OperaListItem _operaListItems = operaListItems.ContainsKey(rowIndex) ? operaListItems[rowIndex] : new OperaListItem(item);
        _operaListItems.UpdataData(operaListData[rowIndex]);
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

    /// <summary>
    /// 操作列表数据
    /// </summary>
    private struct OperaListItemData
    {
        public string operaTitle;

        public OperaListItemData(string _operaTitle)
        {
            operaTitle = _operaTitle;
        }
    }
    /// <summary>
    /// 操作列表Item
    /// </summary>
    private class OperaListItem : BaseListItem
    {
        TextMeshProUGUI txt_opera_desc;
        Button btn_opera;
        private OperaItemSO _data;
        public OperaListItem(RectTransform item) : base(item)
        {
            txt_opera_desc = NodeList["txt_opera_desc"].GetComponent<TextMeshProUGUI>();
            btn_opera = NodeList["btn_opera"].GetComponent<Button>();

            btn_opera.onClick.AddListener(OnClickOperaBtn);
        }
        public void UpdataData(OperaItemSO data)
        {
            _data = data;
            txt_opera_desc.text = data.operaTitle;
        }

        private void OnClickOperaBtn()
        {
            if (_data.operaType == OperaListItemType.WishStar)
            {
                ViewManager.Instance.Open(ViewName.WishStarView);
            }
            Debug.Log("Click" + txt_opera_desc.text);
        }
    }
}



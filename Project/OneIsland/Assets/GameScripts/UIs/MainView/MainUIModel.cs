using System.Collections.Generic;
using UnityEngine;

public class MainUIModel : SingletonClass<MainUIModel>, IModel
{
    private List<OperaItemSO> _operaListDatas;
    public List<OperaItemSO> OperaListDatas;
    public MainUIModel()
    {
        InitCfg();
    }

    public void InitCfg()
    {
        MainUIOperaListSO cfg = AddressableManager.Instance.LoadAssetSync<MainUIOperaListSO>("MainUIOperaListSO");
        _operaListDatas = cfg.operaListDatas;
    }
}
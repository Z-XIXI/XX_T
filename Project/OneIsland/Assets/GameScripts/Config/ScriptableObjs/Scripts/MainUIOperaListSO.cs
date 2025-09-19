using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainUIOperaListSO", menuName = "ScriptableObject/MainUIOperaListSO")]
public class MainUIOperaListSO : ScriptableObject
{
    public List<OperaItemSO> operaListDatas;
}

[System.Serializable]
public struct OperaItemSO
{
    public string operaTitle;
    public OperaListItemType operaType;
}

public enum OperaListItemType
{
    WishStar,
    Farm,
}
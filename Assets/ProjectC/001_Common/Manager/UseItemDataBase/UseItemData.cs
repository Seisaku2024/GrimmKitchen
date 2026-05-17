using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelectUseItemInfo;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "UseItemData", menuName = "ScriptableObjects/UseItem/作成 UseItemData")]
public class UseItemData : ScriptableObject
{
    // 制作者 田内
    // アイテム使用用途データ

    [Header("ID")]
    [SerializeField]
    private SelectUseItemID m_selectUseItemID = SelectUseItemID.None;

    public SelectUseItemID SelectUseItemID
    {
        get { return m_selectUseItemID; }
    }

    [Header("ボタン名")]
    [SerializeField]
    private LocalizedString m_useItemName = new();

    public LocalizedString UseItemName
    {
        get { return m_useItemName; }
    }

    [Header("タイトル名")]
    [SerializeField]
    private string m_titleName = "Title";

    public string TitleName
    {
        get { return m_titleName; }
    }

}

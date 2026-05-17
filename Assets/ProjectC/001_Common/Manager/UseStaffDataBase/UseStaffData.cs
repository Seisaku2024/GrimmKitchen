using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelectUseStaffInfo;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "UseStaffData", menuName = "ScriptableObjects/UseStaff/作成 UseStaffData")]
public class UseStaffData : ScriptableObject
{
    // 制作者 田内
    // アイテム使用用途データ

    [Header("ID")]
    [SerializeField]
    private SelectUseStaffID m_selectUseStaffID = SelectUseStaffID.None;

    public SelectUseStaffID SelectUseStaffID
    {
        get { return m_selectUseStaffID; }
    }

    [Header("ボタン名")]
    [SerializeField]
    private LocalizedString m_useStaffName = new();

    public LocalizedString UseStaffName
    {
        get { return m_useStaffName; }
    }

    [Header("タイトル名")]
    [SerializeField]
    private string m_titleName = "Title";

    public string TitleName
    {
        get { return m_titleName; }
    }
}

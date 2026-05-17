using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UseStaffDataBase", menuName = "ScriptableObjects/UseStaff/作成 UseStaffDataBase")]
public class UseStaffDataBase : ScriptableObject
{
    // 制作者 田内
    // アイテム使用用途データベース


    [Header("データリスト")]
    [SerializeField]
    private List<UseStaffData> m_useStaffDataList = new();


    public List<UseStaffData> UseStaffDataList { get { return m_useStaffDataList; } }
}

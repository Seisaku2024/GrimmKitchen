    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StaffMemberDataBase", menuName = "ScriptableObjects/StaffMember/作成 StaffMemberDataBase")]
public class StaffMemberDataBase : ScriptableObject
{
    // 制作者 田内
    // スタッフデータベース

    [Header("スタッフのデータリスト")]
    [SerializeField]
    private List<StaffMemberData> m_staffMemberDataBaseList = new();


    public List<StaffMemberData> StaffMemberDataBaseList { get { return m_staffMemberDataBaseList; } }
}

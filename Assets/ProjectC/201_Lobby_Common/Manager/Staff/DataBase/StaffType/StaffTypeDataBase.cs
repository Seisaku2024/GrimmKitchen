using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StaffTypeDataBase", menuName = "ScriptableObjects/StaffType/作成 StaffTypeDataBase")]
public class StaffTypeDataBase : ScriptableObject
{
    // 制作者 田内
    // スタッフタイプのデータベース

    [Header("スタッフタイプのデータリスト")]
    [SerializeField]
    private List<StaffTypeData> m_staffTypeDataBaseList = new List<StaffTypeData>();


    public List<StaffTypeData> StaffTypeDataBaseList { get { return m_staffTypeDataBaseList; } }

}

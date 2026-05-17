using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = " StaffNameDataBase", menuName = "ScriptableObjects/StaffName/作成  StaffNameDataBase")]
public class StaffNameDataBase : ScriptableObject
{
    // 制作者 田内

    [Header("データリスト")]
    [SerializeField]
    private List<StaffNameData> m_staffNameDataList = new();


    public List<StaffNameData> staffNameDataList
    {
        get { return m_staffNameDataList; }
    }


}

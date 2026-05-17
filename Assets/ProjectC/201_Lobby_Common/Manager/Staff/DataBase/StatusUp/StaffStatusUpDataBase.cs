using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StaffStatusUpDataBase", menuName = "ScriptableObjects/StaffStatusUp/作成 StaffStatusUpDataBase")]
public class StaffStatusUpDataBase : ScriptableObject
{
    // 制作者 田内

    [Header("データ")]
    [SerializeField]
    private List<StaffStatusUpData> m_staffStatusUpDataList = new();

    public List<StaffStatusUpData> StaffStatusUpDataList
    {
        get { return m_staffStatusUpDataList; }
    }
}

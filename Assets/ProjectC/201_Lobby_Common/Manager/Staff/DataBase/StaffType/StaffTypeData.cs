using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;

[CreateAssetMenu(fileName = "StaffTypeData", menuName = "ScriptableObjects/StaffType/作成 StaffTypeData")]
[System.Serializable]
public class StaffTypeData : ScriptableObject
{
    // 制作者 田内


    [Header("スタッフタイプ")]
    [SerializeField]
    private StaffType m_staffType = StaffType.Chef;

    public StaffType StaffType
    {
        get { return m_staffType; }
    }


    [Header("名前")]
    [SerializeField]
    private string m_staffTypeName = "Base Staff";

    public string StaffTypeName
    {
        get { return m_staffTypeName; }
    }



}

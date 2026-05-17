using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;

[CreateAssetMenu(fileName = "StaffNameData", menuName = "ScriptableObjects/StaffName/作成 StaffNameData")]
public class StaffNameData : ScriptableObject
{
    // 制作者 田内
    // スタッフの名前データベース

    [Header("ランダムスタッフ名ID")]
    [SerializeField]
    private StaffNameID m_staffNameID = StaffNameID.None;

    public StaffNameID StaffNameID
    {
        get { return m_staffNameID; }
    }

    [Header("性別")]
    [SerializeField]
    private StaffGenderType m_staffGenderType = StaffGenderType.Man;

    public StaffGenderType StaffGenderType
    {
        get { return m_staffGenderType; }
    }

    [Header("名前")]
    [SerializeField]
    private UnityEngine.Localization.LocalizedString m_staffName;

    public UnityEngine.Localization.LocalizedString StaffName
    {
        get { return m_staffName; }
    }


    [Header("スタッフ共通名")]
    [SerializeField]
    private UnityEngine.Localization.LocalizedString m_staffCommonName;

    public UnityEngine.Localization.LocalizedString StaffCommonName
    {
        get { return m_staffCommonName; }
    }

}

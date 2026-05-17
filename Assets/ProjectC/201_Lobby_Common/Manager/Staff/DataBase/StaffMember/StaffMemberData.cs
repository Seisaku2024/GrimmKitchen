using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using StaffInfo;

[CreateAssetMenu(fileName = "StaffMemberData", menuName = "ScriptableObjects/StaffMember/作成 StaffMemberData")]
[System.Serializable]
public class StaffMemberData : ScriptableObject
{
    // 制作者 田内
    // スタッフの情報

    //=====================================================

    [Header("スタッフID")]
    [SerializeField]
    private StaffID m_staffID = StaffID.Akazukin;

    public StaffID StaffID
    {
        get { return m_staffID; }
    }

    //=======================================================

    [Header("スタッフ名")]
    [SerializeField]
    private StaffNameID m_staffNameID;

    public StaffNameID StaffNameID
    {
        get { return m_staffNameID; }
    }

    //=====================================================

    [Header("スタッフ性別")]
    [SerializeField]
    private StaffGenderType m_staffGenderType = StaffGenderType.Man;

    public StaffGenderType StaffGenderType
    {
        get { return m_staffGenderType; }
    }

    //=====================================================

    [Header("スタッフの画像")]
    [SerializeField]
    private Sprite m_staffSprite = null;

    public Sprite StaffSprite
    {
        get { return m_staffSprite; }
    }

    //=====================================================

    [Header("スタッフプレハブ(StaffDataがアタッチされたもの)")]
    [SerializeField]
    private GameObject m_staffPrefab = null;

    public GameObject StaffPrefab
    {
        get { return m_staffPrefab; }
    }

    //=====================================================

    // スピードに関する
    [Header("配膳値")]
    [SerializeField]
    [Min(1)]
    private int m_provideValue = 1;

    public int ProvideValue
    {
        get { return m_provideValue; }
    }

    //=====================================================

    // 料理作成時間に関する
    [Header("調理値")]
    [SerializeField]
    [Min(1)]
    private int m_cookingValue = 1;

    public int CookingValue
    {
        get { return m_cookingValue; }
    }

    //=====================================================

    // TODO:未定
    [Header("接客値")]
    [SerializeField]
    [Min(1)]
    private int m_serviceValue = 1;

    public int ServiceValue
    {
        get { return m_serviceValue; }
    }

    //===========================================================

    [Header("ステータス分配値")]
    [SerializeField]
    [Min(0)]
    private int m_maxStatusDistributionValue = 100;

    public int MaxStatusDistributionValue
    {
        get { return m_maxStatusDistributionValue; }
    }
}

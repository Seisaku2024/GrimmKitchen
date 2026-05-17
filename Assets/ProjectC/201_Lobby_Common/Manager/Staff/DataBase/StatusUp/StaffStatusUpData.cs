using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using StaffInfo;

[CreateAssetMenu(fileName = "StaffStatusUpData", menuName = "ScriptableObjects/StaffStatusUp/作成 StaffStatusUpData")]
public class StaffStatusUpData : ScriptableObject
{
    // 制作者　田内

    [Header("ID")]
    [SerializeField]
    private StaffStatusUpID m_staffStatusUpID = StaffStatusUpID.None;

    public StaffStatusUpID StaffStatusUpID
    {
        get { return m_staffStatusUpID; }
    }

    [Header("名前")]
    [SerializeField]
    private LocalizedString m_statusUpName = new();

    public LocalizedString StatusUpName
    {
        get { return m_statusUpName; }
    }

    [Header("説明文")]
    [SerializeField]
    private LocalizedString m_statusUpDescription = new();

    public LocalizedString StatusUpDescription
    {
        get { return m_statusUpDescription; }
    }

    [Header("画像")]
    [SerializeField]
    private Sprite m_statusUpSprite = null;

    public Sprite StatusUpSprite
    {
        get { return m_statusUpSprite; }
    }

    [Header("金額")]
    [SerializeField]
    private float m_money = 10000;

    public float Money
    {
        get { return m_money; }
    }

    [System.Serializable]
    public class StaffStatusUp
    {
        [Header("最低値")]
        [SerializeField]
        [Min(0)]
        private int m_minValue = 0;

        public int MinValue
        {
            get { return m_minValue; }
        }

        [Header("最高値")]
        [SerializeField]
        [Min(0)]
        private int m_maxValue = 0;

        public int MaxValue
        {
            get { return m_maxValue; }
        }
    }

    [Header("ステータス ; 配膳")]
    [SerializeField]
    private StaffStatusUp m_provideStatusUp = new();

    public StaffStatusUp ProvideStaffStatusUp
    {
        get { return m_provideStatusUp; }
    }

    [Header("ステータス ; 料理")]
    [SerializeField]
    private StaffStatusUp m_cookingStatusUp = new();

    public StaffStatusUp CookingStaffStatusUp
    {
        get { return m_cookingStatusUp; }
    }

    [Header("ステータス ; 接客")]
    [SerializeField]
    private StaffStatusUp m_serviceStatusUp = new();

    public StaffStatusUp ServiceStaffStatusUp
    {
        get { return m_serviceStatusUp; }
    }


   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;
using UniRx;
using UnityEngine.Localization;

[System.Serializable]
public class StaffStatusData
{
    // 制作者 田内
    // スタッフステータス

    public class GlobalChangeStaffStatusDataEvent
    {
        public StaffStatusData StaffStatusData;

        static public void PublishGlobalChangeStaffStatusDataEvent(StaffStatusData _data)
        {
            // イベント送信
            GlobalChangeStaffStatusDataEvent eve = new();
            eve.StaffStatusData = _data;
            MessageBroker.Default.Publish<GlobalChangeStaffStatusDataEvent>(eve);
        }
    }



    // ID
    [Header("スタッフID")]
    [SerializeField]
    private StaffID m_staffID = StaffID.Akazukin;
    public StaffID StaffID
    {
        get { return m_staffID; }
    }


    [Header("スタッフタイプ")]
    [SerializeField]
    private StaffType m_staffType = StaffType.Hall;

    public StaffType StaffType
    {
        get { return m_staffType; }
        set { m_staffType = value; }
    }


    [Header("スタッフ名ID")]
    [SerializeField]
    private StaffNameID m_staffNameID = StaffNameID.None;

    public StaffNameID StaffNameID
    {
        get { return m_staffNameID; }
    }


    [System.Flags]
    public enum StatusType
    {
        None = 0,
        IsDismissal = 1 << 0,
        IsStatusUp = 1 << 1,
        IsEmployment = 1 << 2,
        IsSalary = 1 << 3,
    }

    [Header("ステータス種類")]
    [SerializeField]
    private StatusType m_isStatusType = StatusType.None;

    public StatusType IsStatusType
    {
        get { return m_isStatusType; }
    }


    [Header("セーブ")]
    [SerializeField]
    private bool m_isSave = true;

    public bool IsSave
    {
        get { return m_isSave; }
    }

    [Header("配膳値")]
    [SerializeField]
    [Min(1)]
    private int m_provideValue = 50;

    public int ProvideValue
    {
        get { return m_provideValue; }
        set
        {
            m_provideValue = value;
            CheckMaxStatusValue();
            GlobalChangeStaffStatusDataEvent.PublishGlobalChangeStaffStatusDataEvent(this);
        }
    }

    // 料理作成時間に関する
    [Header("調理値")]
    [SerializeField]
    [Min(1)]
    private int m_cookingValue = 50;

    public int CookingValue
    {
        get { return m_cookingValue; }
        set
        {
            m_cookingValue = value;
            CheckMaxStatusValue();
            GlobalChangeStaffStatusDataEvent.PublishGlobalChangeStaffStatusDataEvent(this);
        }
    }

    // TODO:未定
    [Header("接客値")]
    [SerializeField]
    [Min(1)]
    private int m_serviceValue = 50;

    public int ServiceValue
    {
        get { return m_serviceValue; }
        set
        {
            m_serviceValue = value;
            CheckMaxStatusValue();
            GlobalChangeStaffStatusDataEvent.PublishGlobalChangeStaffStatusDataEvent(this);
        }
    }

    //====================
    // 固有ID
    private GameGuid m_guid = GameGuid.NewGuid();

    public GameGuid Guid
    {
        get { return m_guid; }
    }

    //==============================================
    //                実行処理
    //==============================================

    public StaffStatusData()
    {
    }


    public StaffStatusData(StaffStatusData _data)
    {
        m_staffID = _data.StaffID;
        m_staffType = _data.StaffType;
        m_provideValue = _data.ProvideValue;
        m_cookingValue = _data.CookingValue;
        m_serviceValue = _data.ServiceValue;
        m_isStatusType = _data.IsStatusType;
        m_staffNameID = _data.StaffNameID;

        // GUIDのみ新規作成
        m_guid = GameGuid.NewGuid();

    }

    public void Load(StaffStatusSaveLoadData _saveLoadData)
    {
        m_staffID = _saveLoadData.StaffID;
        m_staffType = _saveLoadData.StaffType;
        m_provideValue = _saveLoadData.ProvideValue;
        m_cookingValue = _saveLoadData.CookingValue;
        m_serviceValue = _saveLoadData.ServiceValue;
        m_isStatusType = _saveLoadData.IsStatusType;
        m_staffNameID = _saveLoadData.StaffNameID;
        m_guid = new(_saveLoadData.Guid.GuidString);
    }

    public int EmploymentPrice()
    {
        if ((m_isStatusType & StatusType.IsEmployment) != 0) return 0;

        uint totalStatusValue = (uint)(m_provideValue + m_cookingValue + m_serviceValue);

        // 雇用金額をセット
        float employmentPriceValue = totalStatusValue * 30;
        return (int)employmentPriceValue;
    }


    public int SalaryPrice()
    {
        if ((m_isStatusType & StatusType.IsSalary) != 0) return 0;

        uint totalStatusValue = (uint)(m_provideValue + m_cookingValue + m_serviceValue);

        // 給料金額をセット
        float salaryPriceValue = totalStatusValue * 10;
        return (int)salaryPriceValue;
    }


    /// <summary>
    /// 引数IDを基にデータをセットする
    /// </summary>
    public void SetData(StaffID _id)
    {
        // IDをセット
        m_staffID = _id;

        // スタッフ情報
        var staffMemberData = StaffMemberDataBaseManager.instance.GetStaffMemberData(m_staffID);
        if (staffMemberData == null) return;

        // 名前をセット
        m_staffNameID = staffMemberData.StaffNameID;

        // ステータスをセット
        m_provideValue = staffMemberData.ProvideValue;
        m_cookingValue = staffMemberData.CookingValue;
        m_serviceValue = staffMemberData.ServiceValue;
        CheckMaxStatusValue();
    }

    public void RandomSetData(StaffID _id)
    {
        // IDをセット
        m_staffID = _id;

        // スタッフ情報
        var staffMemberData = StaffMemberDataBaseManager.instance.GetStaffMemberData(m_staffID);
        if (staffMemberData == null) return;

        // ランダムで名前IDをセット
        m_staffNameID = StaffNameDataBaseManager.instance.GetRandomStaffNameID(staffMemberData.StaffGenderType);

        // 配分ステータスをステータスに割り当てる
        int total = staffMemberData.MaxStatusDistributionValue;
        int randomValue1 = Random.Range(0, total);
        int randomValue2 = Random.Range(0, total - randomValue1);
        int randomValue3 = Random.Range(0, total - randomValue1 - randomValue2);

        // ステータスをセット
        m_provideValue = staffMemberData.ProvideValue + randomValue1;
        m_cookingValue = staffMemberData.CookingValue + randomValue2;
        m_serviceValue = staffMemberData.ServiceValue + randomValue3;
        CheckMaxStatusValue();

    }

    /// <summary>
    /// 引数スタッフステータスを強化
    /// </summary>
    public void StatusUp(StaffStatusUpData _data)
    {
        if (_data == null) return;
        m_provideValue += Random.Range(_data.ProvideStaffStatusUp.MaxValue, _data.ProvideStaffStatusUp.MinValue);
        m_cookingValue += Random.Range(_data.CookingStaffStatusUp.MaxValue, _data.CookingStaffStatusUp.MinValue);
        m_serviceValue += Random.Range(_data.ServiceStaffStatusUp.MaxValue, _data.ServiceStaffStatusUp.MinValue);

        CheckMaxStatusValue();
        GlobalChangeStaffStatusDataEvent.PublishGlobalChangeStaffStatusDataEvent(this);
    }



    /// <summary>
    /// ステータス値が最大値を超えていたら調整を行う
    /// </summary>
    public void CheckMaxStatusValue()
    {

        int maxValue = StaffManager.instance.MaxStatusValue;

        if (maxValue < m_provideValue)
        {
            m_provideValue = maxValue;
        }

        if (maxValue < m_cookingValue)
        {
            m_cookingValue = maxValue;
        }

        if (maxValue < m_serviceValue)
        {
            m_serviceValue = maxValue;
        }

    }
}


[System.Serializable]
public class StaffStatusSaveLoadData
{
    public StaffStatusSaveLoadData(StaffStatusData _data)
    {
        if (_data == null) return;
        StaffID = _data.StaffID;
        StaffType = _data.StaffType;
        ProvideValue = _data.ProvideValue;
        CookingValue = _data.CookingValue;
        ServiceValue = _data.ServiceValue;
        IsStatusType = _data.IsStatusType;
        StaffNameID = _data.StaffNameID;
        Guid = new(_data.Guid);
    }

    public StaffID StaffID = StaffID.Akazukin;

    public StaffType StaffType = StaffType.Hall;

    public int ProvideValue = 50;

    public int CookingValue = 50;

    public int ServiceValue = 50;

    public StaffStatusData.StatusType IsStatusType = StaffStatusData.StatusType.None;

    public StaffNameID StaffNameID = StaffNameID.None;

    public GameGuidSaveLoadData Guid = new();
}

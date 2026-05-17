using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;
using UpgradeManagementHouseInfo;

[System.Serializable]
public class StaffPointData
{
    // 制作者 田内

    /* 
     * スタッフの設置ポイントデータ
     * 設置スタッフのGuid等を管理
    */

    //==================================
    [Header("設置ポイント")]
    [SerializeField]
    private GameObject m_setPoint = null;

    public GameObject SetPoint
    {
        get
        {
            if (m_setPoint == null) m_setPoint = new();
            return m_setPoint;
        }
    }

    //==================================
    [Header("スタッフ設置タイプ")]
    [SerializeField]
    private StaffType m_setStaffType = StaffType.Hall;

    public StaffType SetStaffType
    {
        get { return m_setStaffType; }
    }

    //==================================
    // スタッフセットデータ

    private SetStaffPointData m_setStaffPointData = null;

    public SetStaffPointData SetStaffPointData
    {
        get
        {
            if (m_setStaffPointData == null) m_setStaffPointData = new(this);
            return m_setStaffPointData;
        }
    }

    //=================
    // スタッフデータ
    private StaffStatusData m_staffStatusData = null;

    public StaffStatusData StaffStatusData
    {
        get { return m_staffStatusData; }
    }

    //=================
    // 解禁ステージタイプ

    [Header("解禁するステージタイプ")]
    [SerializeField]
    private UpgradeManagementHouseID m_upgradeManagementHouseID = UpgradeManagementHouseID.Upgrade1;

    public UpgradeManagementHouseID UpgradeManagementHouseID
    {
        get { return m_upgradeManagementHouseID; }
    }

    //============================================
    //               実行処理
    //============================================

    public void Load(StaffPointSaveLoadData _saveLoadData)
    {
        if (_saveLoadData == null) return;
        m_setStaffPointData = new(this);
        m_setStaffPointData.PointNumber = StaffManager.instance.StaffPointDataList.IndexOf(this);
        m_setStaffPointData.StaffGuid = new(_saveLoadData.StaffGuid.GuidString);
    }

    /// <summary>
    /// ステータスにデータをセット
    /// </summary>
    public void SetStaffStatusData(StaffStatusData _data)
    {
        if (_data != null)
        {
            m_staffStatusData = _data;

            if (m_setStaffPointData == null) m_setStaffPointData = new(this);
            m_setStaffPointData.StaffGuid = _data.Guid;
        }
        else
        {
            m_staffStatusData = null;

            if (m_setStaffPointData == null) m_setStaffPointData = new(this);
            m_setStaffPointData.StaffGuid = GameGuid.Empty;
        }
    }

    /// <summary>
    /// ターゲットにしているスタッフがストレージに存在しなければ
    /// </summary>
    public void CheckAddedStaffStorage()
    {
        if (m_staffStatusData == null) return;

        if (StaffManager.instance.IsAddedStaffStorage(m_staffStatusData) == false)
        {
            m_staffStatusData = null;

            if (m_setStaffPointData == null) m_setStaffPointData = new(this);
            m_setStaffPointData.StaffGuid = GameGuid.Empty;
        }

    }

    public void FindToSetStaffStatusData()
    {
        if (m_setStaffPointData == null || m_setStaffPointData.StaffGuid == null) return;

        foreach (var data in StaffManager.instance.StaffStorageRC)
        {
            if (data == null) continue;

            // Guidが一致するデータをセット
            if (GameGuid.IsMatchString(m_setStaffPointData.StaffGuid, data.Guid))
            {
                SetStaffStatusData(data);
                return;
            }
        }

        // 当てはまらなければnull
        SetStaffStatusData(null);
    }


    /// <summary>
    /// 解禁されているか
    /// </summary>
    public bool IsUnlock()
    {
        // 初期状態であれば
        if (m_upgradeManagementHouseID == UpgradeManagementHouseID.Upgrade1) return true;

        if (ManagementDataManager.instance == null) return false;
        if (m_upgradeManagementHouseID <= ManagementDataManager.instance.UpgradeManagementHouseID) return true;
        return false;
    }



}



[System.Serializable]
public class StaffPointSaveLoadData
{
    public StaffPointSaveLoadData(SetStaffPointData _data)
    {
        if (_data == null) return;

        PointNumber = _data.PointNumber;
        StaffGuid = new(_data.StaffGuid);
    }

    public int PointNumber = 0;
    public GameGuidSaveLoadData StaffGuid = null;
}

[System.Serializable]
public class SetStaffPointData
{
    public SetStaffPointData(StaffPointData _data)
    {
        if (_data == null) return;
        PointNumber = StaffManager.instance.StaffPointDataList.IndexOf(_data);
    }

    public int PointNumber = -1;
    public GameGuid StaffGuid = GameGuid.Empty;
}
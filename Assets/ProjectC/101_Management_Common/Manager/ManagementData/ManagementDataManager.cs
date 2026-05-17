using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UpgradeManagementHouseInfo;

namespace UpgradeManagementHouseInfo
{
    public enum UpgradeManagementHouseID
    {
        Upgrade1 = 0,
        Upgrade2 = 1,
        Upgrade3 = 2,
    }
}


public class ManagementDataManager : BaseManager<ManagementDataManager>
{
    // 制作者 田内
    // 経営ゲームに関する情報を管理するマネージャークラス


    // 総額
    private int m_totalEarnedMoney = 0;

    public int TotalEarnedMoney
    {
        get { return m_totalEarnedMoney; }
        set { m_totalEarnedMoney = value; }
    }

    // 総評価
    private int m_totalEvaluation = 0;

    public int TotalEvaluation
    {
        get { return m_totalEvaluation; }
        set { m_totalEvaluation = value; }
    }

    // お店の状態
    private UpgradeManagementHouseID m_upgradeManagementHouseID = UpgradeManagementHouseID.Upgrade1;

    public UpgradeManagementHouseID UpgradeManagementHouseID
    {
        get { return m_upgradeManagementHouseID; }
    }


    [Header("デバッグ")]
    [SerializeField]
    private bool m_isDebug = true;

    [Header("デバッグ用総額最低値")]
    [SerializeField]
    private int m_initializeEarnedMoney = 0;

    [Header("デバッグ用総評価最低値")]
    [SerializeField]
    private int m_initializeTotalEvaluation = 0;

    [Header("デバッグ用お店状態")]
    [SerializeField]
    private UpgradeManagementHouseID m_initializeUpgradeManagementHouseID = UpgradeManagementHouseID.Upgrade1;

    //==================================================
    //                  処理
    //==================================================

    override protected void Load()
    {
        // ロード情報をセット
        var saveLoad = ManagementDataSaveLoader.Load();
        m_totalEarnedMoney = saveLoad.TotalEarnedMoney;
        m_totalEvaluation = saveLoad.TotalEvaluation;
        m_upgradeManagementHouseID = saveLoad.UpgradeManagementHouseID;

#if UNITY_EDITOR
        // デバッグ用
        if (m_isDebug)
        {
            m_totalEarnedMoney = m_initializeEarnedMoney;
            m_totalEvaluation = m_initializeTotalEvaluation;
            m_upgradeManagementHouseID = m_initializeUpgradeManagementHouseID;
        }
#endif
    }


    /// <summary>
    /// ゲームデータを初期化する
    /// </summary>
    public void OnInitialize()
    {
        m_totalEarnedMoney = 0;
        m_totalEvaluation = 0;
    }
}

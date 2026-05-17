/*!
 * @file ManagementEventManager.cs
 * @brief 経営のイベントを管理するマネージャー
 * @author 田内
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @brief 経営のイベントを管理するマネージャー
/// BaseManagementEvent.cs を継承したイベントを抽選し実行する
/// </summary>
public class ManagementEventManager : BaseManager<ManagementEventManager>
{

    [Header("データベース")]
    [SerializeField]
    private ManagementEventDataBase m_managementEventDataBase = null;


    [SerializeField]
    private int m_eventMax = 10;

    public ManagementEventDataBase ManagementEventDataBase
    {
        get { return m_managementEventDataBase; }
    }


    // 発生中のイベントリスト
    private List<BaseManagementEvent> m_eventList = new();

    // 常駐する潜在イベントリスト(イベントマックスに影響しない)
    private List<PotentialBaseManagementEvent> m_potentialEventList = new();

    // 矢印表示を管理してくれるやつのキャッシュ
    private TargetIndicatorController m_targetIndicatorController = null;

    public List<BaseManagementEvent> EventList
    {
        get { return m_eventList; }
    }

    public bool IsEventMax()
    {
        return m_eventList.Count >= m_eventMax;
    }


    //==============================================
    //                 実行処理
    //==============================================

    void Update()
    {
        OnEventListUpdate();
        OnPotentialEventCheck();// 潜在イベントの条件チェック
    }


    /// <summary>
    /// @brief 外部からイベントを追加し抽選・実行する
    /// @return 追加できたかどうか
    /// </summary>
    public bool AddEventList(BaseManagementEvent _baseManagementEvent)
    {
        if (_baseManagementEvent == null) return false;

        if (m_eventList.Count >= m_eventMax) return false;

        // リストに追加
        m_eventList.Add(_baseManagementEvent);

        // 初期処理
        _baseManagementEvent.OnStart();


        // イベントの方向に矢印表示　伊波
        if (!m_targetIndicatorController)
        {
            var UIObjects = GameObject.FindGameObjectsWithTag("UI");
            foreach (var UI in UIObjects)
            {
                if (UI.TryGetComponent(out m_targetIndicatorController))
                {
                    break;
                }
            }
        }

        // イベント時誘導したいときのみ矢印表示
        if (_baseManagementEvent.EnableTargetIndicator)
        {

            _baseManagementEvent.TargetIndicator = m_targetIndicatorController.AddIndicator(
                    _baseManagementEvent.transform,
                    TargetIndicatorController.IndicatorType.managementEvent);
        }


        return true;
    }

    public void AddPotentialEvent(PotentialBaseManagementEvent _baseManagementEvent)
    {
        if (_baseManagementEvent == null) return;

        // 潜在イベントリストに追加（m_eventMaxには影響しない）
        m_potentialEventList.Add(_baseManagementEvent);
    }


    //! 追加されたイベント群の抽選・実行を行う
    private void OnEventListUpdate()
    {
        foreach (var eve in m_eventList)
        {
            if (eve == null) continue;

            // 処理を行う
            eve.OnUpdate();

            // 終了すれば
            if (eve.IsEventEnd)
            {
                // 終了処理を行う
                eve.OnExit();
            }
        }

        m_eventList.RemoveAll(_ =>
        {
            if (_ == null) return true;
            return false;
        });


    }

    //! 潜在イベントの条件チェックを行い、条件を満たしたら正規イベントに追加
    private void OnPotentialEventCheck()
    {
        var removeList = new List<PotentialBaseManagementEvent>();

        foreach (var potentialEvent in m_potentialEventList)
        {
            potentialEvent.OnUpdate();

            // ここで特定の条件を満たしているかチェック
            if (potentialEvent.IsCanAddEvent)
            {
                potentialEvent.OnRegisterProcess();
                potentialEvent.Registed = true;

                // 追加後削除するか
                if (potentialEvent.IsAddBreak)
                {
                    removeList.Add(potentialEvent);
                }

            }
        }
        foreach (var remove in removeList)
        {
            m_potentialEventList.Remove(remove);
        }
    }
}

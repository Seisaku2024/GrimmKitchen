/*!
 * @file BaseManagementEvent.cs
 * @brief 経営イベントの基底クラス
 * @author 田内
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagementEventInfo;
using ManagementGameInfo;

/// <summary>
/// @brief 経営イベントの基底クラス
/// マネージャーから管理される想定 (ManagementEventManager.cs)
/// </summary>
public class BaseManagementEvent : MonoBehaviour
{

    [Header("イベントID")]
    [SerializeField]
    private ManagementEventID m_eventID = ManagementEventID.Gangster;

    public ManagementEventID EventID
    {
        get { return m_eventID; }
        set { m_eventID = value; }
    }


    //! @brief 動作が終了したかどうか
    private bool m_isEventEnd = false;

    //! @brief 動作終了かどうかの参照用
    public bool IsEventEnd
    {
        get { return m_isEventEnd; }
    }

    // イベント時の矢印を消すように補完しておく
    private TargetIndicator m_indicator;
    public TargetIndicator TargetIndicator { get { return m_indicator; } set { m_indicator = value; } }

    [SerializeField,Header("矢印ガイドを表示するか")]
    private bool m_enableTargetIndicator = true;

    public bool EnableTargetIndicator { get { return m_enableTargetIndicator; } }


    //====================================================
    //                  実行処理
    //====================================================


    //! @brief イベントの開始 継承先でオーバーライドする
    /// <summary>
    /// イベント開始処理
    /// </summary>
    virtual public void OnStart()
    {
        // 開始処理はここで行う
    }


    /// <summary>
    /// イベント実行処理
    /// </summary>
    virtual public void OnUpdate()
    {
        m_isEventEnd = true;
        Debug.LogError(this.GetType().Name + " クラスで OnUpdate がオーバーライドされていません");
    }


    /// <summary>
    /// イベント終了処理
    /// </summary>
    virtual public void OnExit()
    {
        // 終了処理があればここで行う


        if(m_indicator)
        {
            m_indicator.Target = null;
            m_indicator = null;
        }

        // 満足度を追加
        AddSatisfactionLevel();

        // 削除
        Destroy(gameObject);
    }


    /// <summary>
    ///  満足度を追加
    /// </summary>
    protected void AddSatisfactionLevel()
    {
        var data = ManagementEventManager.instance.ManagementEventDataBase.GetManagementEventData(m_eventID);
        if (data == null) return;

        // 満足度を追加
        ManagementGameDataManager.instance.AddSatisfactionValue(data.AddSatisfactionValue);
    }


    /// <summary>
    /// @brief イベント終了処理
    /// イベントの解決での終了か失敗での終了か指定できる
    /// </summary>
    /// <param name="_type"></param>
    public void SetEventEnd(EventSolutionType _type)
    {
        if (m_isEventEnd) return;

        // イベント数を追加
        ManagementGameDataManager.instance.AddEventSolutionNum(_type);

        // イベント終了
        m_isEventEnd = true;
    }

}

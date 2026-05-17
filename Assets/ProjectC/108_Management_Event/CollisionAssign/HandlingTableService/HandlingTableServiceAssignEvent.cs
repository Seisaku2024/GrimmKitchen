/*!
 * @file HandlingTableServiceAssignEvent.cs
 * @brief プレイヤーが料理をもってテーブルまでもっていくイベントの管理、当たり判定等を行う
 * @author 上甲
 */

using OrderFoodInfo;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HandlingTableServiceAssignEvent : BaseAssignEventObject
{
    public OrderFoodData m_orderFoodData = null;

    // プレイヤーが料理を設置するためのイベント
    [SerializeField]
    PlayerPutTableServiceAssignEvent m_playerPutEvent = null;


    GameObject m_playerInstans = null;

    private GameObjectRegistry m_gameObjectRegistry = null;

    //======================================
    //           実行処理
    //======================================

    protected override void Start()
    {
        if (m_playerPutEvent == null)
        {
            m_playerPutEvent = FindObjectOfType<PlayerPutTableServiceAssignEvent>();
        }

        base.Start();
    }

    public void SetOrderFoodData(OrderFoodData orderFoodData)
    {
        m_orderFoodData = orderFoodData;

        // オーダーフードデータがある場合は当たり判定を有効にする
        this.isCollisionEnable = (orderFoodData != null);

    }

    /// <summary>
    /// @brief 何らかのアクセスがあった際のイベント
    /// アクセスの定義は継承先で行う
    /// </summary>
    public override void OnCollisionAccessEvent()
    {
        // 料理のステートが条件を満たしていない場合は処理を行わない
        if (!IsCarryAbleState()) return;


        var parentObj = m_playerInstans;

        if (m_gameObjectRegistry != null)
        {
            parentObj = m_gameObjectRegistry.GetObjectByKey("HavePoint");
        }

        m_orderFoodData.transform.SetParent(parentObj.transform);
        m_orderFoodData.transform.localPosition = Vector3.zero;
        m_orderFoodData.transform.rotation = quaternion.identity;


        m_orderFoodData.CurrentOrderFoodState = OrderFoodState.PlayerCarry;

        // プレイヤーが料理を設置するためのイベントの当たり判定を有効にする
        if (m_playerPutEvent)
        {
            m_playerPutEvent.SetOrderFoodData(m_orderFoodData);
        }

        m_cCore.PlayerParameters.m_isFoodHold = true;

        this.isCollisionEnable = false;
        m_orderFoodData = null;
    }

    protected override bool IsShowAbleActionUI()
    {
        return IsCarryAbleState();
    }

    private bool IsCarryAbleState()
    {
        // 料理が紐づけられていない状態
        if (m_orderFoodData == null) { return false; }

        // すでに他の料理を持っていたら
        if (!m_playerPutEvent.IsCarryAbleState()) { return false; }

        if (isCollisionEnable == false) return false;

        var state = m_orderFoodData.CurrentOrderFoodState;

        return state == OrderFoodState.WaitCarry || state == OrderFoodState.WaitHallStaff;
    }


    protected override void OnCollisionTriggerEvent()
    {
    }

    /// <summary>
    /// @brief 接触終了時のイベント
    /// </summary>
    protected override void OnCollisionTriggerExitEvent()
    {
    }

    /// <summary>
    /// @brief アクセスされたかどうかを返す
    /// 主にキーアクセスを想定
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public override bool IsAccessed(ref IInputProvider input, GameObject player)
    {
        if (m_gameObjectRegistry == null)
        {
            m_gameObjectRegistry = player.transform.root.GetComponent<GameObjectRegistry>();
        }
        if (m_playerInstans == null)
        {
            m_playerInstans = player;
        }

        if (!IsCarryAbleState()) return false;

        return input.Cleanning;
    }


}

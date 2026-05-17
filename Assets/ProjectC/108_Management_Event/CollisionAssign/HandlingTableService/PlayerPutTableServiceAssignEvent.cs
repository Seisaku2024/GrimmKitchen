using OrderFoodInfo;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerPutTableServiceAssignEvent : BaseAssignEventObject
{
    public OrderFoodData m_orderFoodData = null;

    public bool SetOrderFoodData(OrderFoodData orderFoodData)
    {
        if (m_orderFoodData != null) return false;
        m_orderFoodData = orderFoodData;
        m_showIcon.SetActive(true);

        transform.position = m_orderFoodData.TargetTableSetData.TablePoint.position;
        transform.rotation = quaternion.identity;

        isCollisionEnable = true;
        return true;
    }

    [SerializeField]
    private GameObject m_showIcon = null;

    /// <summary>
    /// @brief 何らかのアクセスがあった際のイベント
    /// アクセスの定義は継承先で行う
    /// </summary>
    public override void OnCollisionAccessEvent()
    {
        if (IsCarryAbleState()) return;

        m_orderFoodData.transform.parent = null;
        m_orderFoodData.transform.localPosition = m_orderFoodData.TargetTableSetData.TablePoint.transform.position;
        m_orderFoodData.transform.rotation = quaternion.identity;

        m_orderFoodData.CurrentOrderFoodState = OrderFoodState.Set;

        isCollisionEnable = false;
        m_showIcon.SetActive(false);
        m_orderFoodData = null;
        m_cCore.PlayerParameters.m_isFoodHold = false;

        Debug.Log("料理を設置しました");
    }

    private void Update()
    {
        if (m_orderFoodData == null && m_showIcon != null && m_showIcon.activeInHierarchy)
        {
            m_showIcon.SetActive(false);
            m_cCore.PlayerParameters.m_isFoodHold = false;
        }
    }



    public bool IsCarryAbleState()
    {
        if (m_orderFoodData != null) return false;
        return true;
    }

    /// <summary>
    /// @brief アクセスされたかどうかを返す
    /// 主にキーアクセスを想定
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public override bool IsAccessed(ref IInputProvider input, GameObject player)
    {
        if (m_orderFoodData == null) return false;
        return input.Cleanning;
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

}

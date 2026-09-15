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
        if (m_orderFoodData != null || orderFoodData == null ||
            orderFoodData.TargetTableSetData == null || orderFoodData.TargetTableSetData.TablePoint == null) return false;
        m_orderFoodData = orderFoodData;
        ShowDestinationEffect();

        transform.position = m_orderFoodData.TargetTableSetData.TablePoint.position;
        transform.rotation = quaternion.identity;

        isCollisionEnable = true;
        return true;
    }

    [SerializeField]
    private GameObject m_showIcon = null;

    [SerializeField, Header("配膳先の机の上に表示するエフェクト")]
    private GameObject m_destinationEffectPrefab = null;

    private GameObject m_destinationEffect = null;

    private void ShowDestinationEffect()
    {
        HideDestinationEffect();
        if (m_showIcon != null) m_showIcon.SetActive(true);
        if (m_destinationEffectPrefab == null) return;

        var table = m_orderFoodData.TargetTableSetData;
        // 料理を置く机上のポイントに表示する。
        var position = table.TablePoint.position;

        m_destinationEffect = Instantiate(m_destinationEffectPrefab, position, Quaternion.identity);

        // 開始時の閃光などの単発演出を止め、持続するオーラだけを表示する。
        var particles = m_destinationEffect.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var particle in particles)
        {
            particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        foreach (var particle in particles)
        {
            var main = particle.main;
            // 円錐のMeshLight2は非ループでも寿命が無限のため、持続演出として残す。
            bool isPersistent = main.loop ||
                (main.startLifetime.mode == ParticleSystemCurveMode.Constant &&
                 float.IsPositiveInfinity(main.startLifetime.constant));
            if (!isPersistent) continue;

            // 生成直後から粒子が出揃った状態にする。子の単発演出は再生しない。
            main.startDelay = 0f;
            particle.Simulate(main.duration, false, true, true);
            particle.Play(false);
        }
    }

    private void HideDestinationEffect()
    {
        if (m_showIcon != null) m_showIcon.SetActive(false);
        if (m_destinationEffect == null) return;
        m_destinationEffect.SetActive(false);
        Destroy(m_destinationEffect);
        m_destinationEffect = null;
    }

    private void OnDisable()
    {
        HideDestinationEffect();
    }

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
        HideDestinationEffect();
        m_orderFoodData = null;
        m_cCore.PlayerParameters.m_isFoodHold = false;

        Debug.Log("料理を設置しました");
    }

    private void Update()
    {
        if (m_orderFoodData == null && (m_destinationEffect != null ||
            (m_showIcon != null && m_showIcon.activeSelf)))
        {
            HideDestinationEffect();
            isCollisionEnable = false;
            if (m_cCore != null) m_cCore.PlayerParameters.m_isFoodHold = false;
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

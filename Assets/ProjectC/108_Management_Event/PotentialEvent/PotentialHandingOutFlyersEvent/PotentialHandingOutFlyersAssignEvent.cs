/*!
 * @file PotentialHandingOutFlyersAssignEvent.cs
 * @brief チラシ配りイベントの当たり判定系担当
 * @author 上甲
 */

using ExternalPropertyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialHandingOutFlyersAssignEvent : BaseAssignEventObject
{
    //! @brief 接触時入店抽選判定
    [SerializeField] private PassebryColliderAttach m_passebryColliderAttacher = null;

    [SerializeField, Range(0, 100), Label("入店確率")] private int m_probability = 0;

    //! @brief アニメーション中のチラシ配り判定回数
    [SerializeField, Min(2)] private int m_handingOutFlyersNum = 1;

    Animator m_coreAnimator = null;

    private float m_handingOutFlyersInterval = 0.0f;

    // ! @brief チラシ配り判定時間
    private float m_handingOutFlyersTime = 1.0f;

    // ! @brief チラシ配り中のタイマー
    private float m_handingOutFlyersTimer = 0.0f;

    //! @brief 判定発生用の間隔カウンター
    private float m_handingOutFlyersIntervalCounter = 0.0f;

    //! @brief チラシ配り中かを管理するプロパティ
    private bool m_isHandingOutFlyers = false;

    private void Update()
    {
        if (m_isHandingOutFlyers)
        {
            m_handingOutFlyersIntervalCounter += Time.deltaTime;

            if (m_handingOutFlyersIntervalCounter >= m_handingOutFlyersInterval)
            {

                //! @todo チラシ配り抽選判定実行
                if (m_passebryColliderAttacher != null)
                {
                    m_passebryColliderAttacher.InsideThinkEntering(m_probability,true);
                    Debug.Log("チラシ配り抽選判定実行");
                }

                m_handingOutFlyersIntervalCounter = 0.0f;
                m_handingOutFlyersTimer += m_handingOutFlyersInterval;
            }

            if (m_handingOutFlyersTimer >= m_handingOutFlyersTime)
            {
                m_isHandingOutFlyers = false;
                m_handingOutFlyersTimer = 0.0f;
            }
        }
    }

    protected override void OnCollisionTriggerEvent()
    {
    }

    protected override void OnCollisionTriggerExitEvent()
    {
    }

    public override void OnCollisionAccessEvent()
    {
        if (m_passebryColliderAttacher == null)
        {
            m_passebryColliderAttacher = GetComponent<PassebryColliderAttach>();
        }
    }

    public override bool SetAnimationTrigger()
    {
        if (base.SetAnimationTrigger())
        {
            m_coreAnimator = m_cCore.m_animator;
            m_coreAnimator.Update(0.0f);
            var state = m_coreAnimator.GetCurrentAnimatorStateInfo(0);
            m_handingOutFlyersTime = state.length;
            m_handingOutFlyersInterval = m_handingOutFlyersTime / m_handingOutFlyersNum;
            m_isHandingOutFlyers = true;
            return true;
        }
        return false;
    }

    public override bool IsAccessed(ref IInputProvider input, GameObject player)
    {
        if (m_isHandingOutFlyers) return false;
        return input.Cleanning;
    }

    private void Start()
    {
        base.Start();
        if (m_passebryColliderAttacher == null)
        {
            TryGetComponent<PassebryColliderAttach>(out m_passebryColliderAttacher);
        }
    }

}

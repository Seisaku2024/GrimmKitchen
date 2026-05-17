using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine.SocialPlatforms.Impl;
using Arbor.ValueFlow;
using UnityEngine.Serialization;
using KinematicCharacterController;

[AddComponentMenu("")]
public class RotateEnemyTarget : StateBehaviour
{

    //ターゲットリグをエネミーの方向へと移行し、その方向へプレイヤーを傾ける処理（山本）

    [SerializeField]
    [SlotType(typeof(Animator))]
    private FlexibleComponent m_animator = new FlexibleComponent(FlexibleHierarchyType.RootGraph);
    [SerializeField]
    private FlexibleTransform m_enemyTransform = new FlexibleTransform(FlexibleHierarchyType.RootGraph);
    [SerializeField]
    private FlexibleFloat m_moveLookTargetSpeed = new FlexibleFloat();
    [SerializeField]
    private FlexibleComponent m_targetCore = new FlexibleComponent();
    [SerializeField]
    private FlexibleFloat m_shotInterval = new FlexibleFloat();
    [SerializeField]
    private FlexibleInt m_shotCount = new FlexibleInt();
    [SerializeField]
    [FormerlySerializedAs("nextState")]
    private StateLink _NextState = new StateLink();
    [SerializeField]
    [FormerlySerializedAs("returnState")]
    private StateLink _ReturnState = new StateLink();
    [SerializeField]
    [FormerlySerializedAs("DisappearState")]
    private StateLink _Disappearstate = new StateLink();



    private Vector3 m_targetPos = Vector3.zero;
    private Vector3 m_targetVec = Vector3.zero;
    private float m_shotTime = 0.0f;
    private Animator m_myAnimator = null;
    private CharacterCore m_enemyCore = null;
    private bool m_attackFlg = false;

    // Use this for initialization
    void Start()
    {

    }

    // Use this for awake state
    public override void OnStateAwake()
    {
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        m_targetPos = Vector3.zero;
        m_targetVec = Vector3.zero;

        m_myAnimator = m_animator.value as Animator;
        m_enemyCore = m_targetCore.value as CharacterCore;

        m_shotTime = 0.0f;
        m_attackFlg = false;

    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();



        //リグ0なら1に変更する
        if (m_myAnimator.TryGetComponent(out SetRigInfo rig))
        {
            if (rig == null)
            {
                return;
            }

            if (rig.UseRig.weight == 0.0f)
                //リグが働くようにWeightを1に
                rig.UseRig.weight = 1.0f;
        }


    }

    public override void OnStateFixedUpdate()
    {
        base.OnStateFixedUpdate();

        //ショットカウントが０以下なら消滅へ
        if (m_shotCount.value <= 0)
        {
            Transition(_Disappearstate);
            return;
        }

        //敵のHPが０なら追従モードへと戻る
        if (m_enemyCore.Status.m_hp.Value <= 0.0f)
        {
            Transition(_ReturnState);
            return;
        }

        //　敵が吸収状態なら攻撃せずに元に戻す(山本)
        if(m_enemyCore.gameObject.layer == LayerMask.NameToLayer("AbsorbEnemy"))
        {
            Transition(_ReturnState);
            return;
        }


        m_targetPos = m_enemyTransform.value.position;
        m_targetVec = m_targetPos - transform.position;
        m_targetVec.y = 0.0f;
        m_targetVec.Normalize();

        RotateToTargetEnemy(m_targetVec);

        if (m_attackFlg == true)
        {
            Transition(_NextState);
            return;
        }

    }

    private void RotateToTargetEnemy(Vector3 targetVec)
    {
        if (targetVec == Vector3.zero)
        {
            return;
        }

        //見つめる先の位置取得
        Vector3 nowTargetPos = Vector3.zero;
        if (m_myAnimator.TryGetComponent(out SetRigInfo rig))
        {
            if (rig == null)
            {
                return;
            }

            nowTargetPos = rig.TargetTransform.position;
            nowTargetPos.y = 0;

        }

        //目標の地点へ移動 
        nowTargetPos = Vector3.MoveTowards(nowTargetPos, m_targetPos, m_moveLookTargetSpeed.value * Time.fixedDeltaTime);


        //敵中心座標へのオフセット
        float enemyHeight = 0.0f;

        if (m_enemyCore.transform.TryGetComponent(out KinematicCharacterMotor kinematicCharacter))
        {
            enemyHeight = kinematicCharacter.Capsule.center.y;
        }

        rig.TargetTransform.position = nowTargetPos + new Vector3(0, enemyHeight, 0);


        //距離取得
        float dist = Vector3.Distance(m_targetPos, nowTargetPos);

        if (Vector3.Angle(targetVec, transform.forward) < 1f && dist == 0.0f)
        {
            if (transform.TryGetComponent(out PlayerSkillsParameters skill))
            {
                if (skill)
                {
                    skill.TargetPosition = rig.TargetTransform.position;
                }
            }

            if (m_shotInterval.value <= m_shotTime && m_attackFlg == false)
            {
                var shotCount = m_shotCount.value;
                shotCount--;
                m_shotCount.parameter.value = shotCount;

                m_attackFlg = true;

                return;
            }

            m_shotTime += Time.fixedDeltaTime;

        }

        targetVec.Normalize();

        if (transform.TryGetComponent(out MyCharacterController chara))
        {
            chara.LookVector = targetVec;
        }

    }


    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}

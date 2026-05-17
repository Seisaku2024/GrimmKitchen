using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

using System;

// キャラクター同士の押し出し処理コンポーネント（伊波）

public class PushOutCollider : MonoBehaviour
{
    [Header("吹き飛ばす力")]
    [SerializeField, Range(0f, 10f)] private float m_pushPower = 3f;
    [Header("強打になる時間")]
    [SerializeField, Range(0f, 10f)] private float m_stunStartTime = 3f;


    async void Start()
    {
        this.OnTriggerEnterAsObservable()
         .Where(_ => enabled)
         .Subscribe(collider =>
         {
             PushOut(collider);
         }
         ).AddTo(this);

        this.OnTriggerStayAsObservable()
         .Where(_ => enabled)
         .Subscribe(collider =>
         {
             PushOut(collider);
         }
         ).AddTo(this);

        await UniTask.Delay(TimeSpan.FromSeconds(m_stunStartTime));

        // NullCheck追加(山本)
        if (this == null)
        {
            return;
        }

        if (TryGetComponent(out AttackApplicant applicant))
        {
            applicant.AttackData.IsStrongAttack = true;
        }

        //DOVirtual.DelayedCall(m_stunStartTime, () =>
        //{
        //    if (TryGetComponent(out AttackApplicant applicant))
        //    {
        //        applicant.AttackData.IsStrongAttack = true;
        //    }
        //});
    }

    void PushOut(Collider collider)
    {
        // 自分自身なら処理しない
        if (collider.transform.root.name == transform.root.name) return;

        MyCharacterController hitMotor;
        if (!collider.TryGetComponent(out hitMotor)) return;

        // 相手との角度計算
        Vector3 targetVec = collider.transform.root.position - transform.root.position;
        targetVec.y = 0;
        //float angle = Vector3.SignedAngle(transform.root.forward, targetVec.normalized, Vector3.up);
        //if (Mathf.Abs(angle) >= 90) return;
        //if (angle > 0) angle += m_angleSpeed * Time.deltaTime; else angle -= m_angleSpeed * Time.deltaTime;
        //float rad = angle * Mathf.Deg2Rad;
        //Vector3 vec = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));

        // 相手の位置移動
        //float dist = myCapsuleCollider.radius + hitMotor.Capsule.radius;
        //Vector3 pos = transform.position;
        //pos.y = hitMotor.transform.position.y;
        //Vector3 nextPos = pos + (Quaternion.Euler(0, angle, 0) * transform.root.forward) * dist;
        //hitMotor.MoveCharacter(nextPos);
        hitMotor.AddVelocity(targetVec.normalized * m_pushPower);
        hitMotor.MomentaryRot = true;
        hitMotor.LookVector = -targetVec.normalized;
    }
}

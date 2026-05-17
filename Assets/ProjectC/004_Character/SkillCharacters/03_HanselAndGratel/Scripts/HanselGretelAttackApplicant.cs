using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class HanselGretelAttackApplicant : MonoBehaviour
{
    // ヘンゼルとグレーテルスキル用の当たったらダメージを与えるオブジェクト(山本)
    private AttackDamageData m_attackDamageData;

    private OwnerInfoTag m_ownerInfoTag;
    private List<Collider> m_hittedColliders = new List<Collider>();

    public void SetAttackData(AttackDamageData data)
    {
        if (data == null)
        {
            Debug.LogError("攻撃のデータが取れませんでした。AnimatorのAnimEventAttackの設定か、データベースを調べてください" + gameObject.name);
        }
        m_attackDamageData = data;
    }

    private void Start()
    {
        // オーナー情報
        m_ownerInfoTag = GetComponent<OwnerInfoTag>();
        if (m_ownerInfoTag == null) { return; }

        if (!TryGetComponent(out Collider myCol)) return;

        this.OnTriggerEnterAsObservable()
            .Where(_ => enabled)
            .Subscribe(collider =>
            {
                if (m_ownerInfoTag.Characore == null) { return; }


                // 自分との当たり判定なら進まない
                if (collider.transform.root.name == m_ownerInfoTag.Characore.name) return;
                //プレイヤーとストーリースキルキャラクターの攻撃は当たらない（山本）
                if (m_ownerInfoTag.transform.root.tag == "Player" && collider.transform.tag == "SkillCharacter")
                {
                    return;
                }

                GameObject otherObj = collider.gameObject;

                // ダメージをもらえるオブジェクトであれば、処理を行う
                var damageable = otherObj.GetComponent<IDamageable>();
                if (damageable == null) { return; }

                // 仮　攻撃可能でなければ終わり
                if (!m_ownerInfoTag.Characore.DoFriendlyFire &&
                !m_attackDamageData.DoFriendlyFire &&
                !damageable.IsAttackable(m_ownerInfoTag.GroupNo)) { return; }

                // 多段ヒット判断
                if (!m_attackDamageData.DoFriendlyFire && m_hittedColliders.Contains(collider)) { return; }

               

                DamageNotification damageNotification = new();
                damageNotification.m_status = m_ownerInfoTag.Characore.Status;
                // 与えるダメージ量
                damageNotification.m_attackData = m_attackDamageData;
                // 送信するヒットストップ時間
                damageNotification.m_hitStopTime = m_attackDamageData.HitStopTime;

                float damage = damageNotification.m_attackData.Attack;


                // ダメージ処理・相手側のヒットストップもこの中で
                damageable.Damaged(damageNotification, myCol,1.0f);

                //　当たったなら、当たった際の処理を実行
                if (damageNotification.m_replyIsHit)
                {
                    m_hittedColliders.Add(collider);


                    // ヒットエフェクト表示
                    if (m_attackDamageData.AssetHitEffect != null)
                    {
                        //修正：ヒットエフェクトを攻撃判定が当たった場所に表示にする(山本)
                        Instantiate(m_attackDamageData.AssetHitEffect,
                            position: collider.ClosestPoint(gameObject.transform.position),
                            rotation: Quaternion.identity,
                            parent: null);
                    }

                    if (!string.IsNullOrEmpty(m_attackDamageData.SoundData.m_soundName))
                    {
                        SoundManager.Instance.Start3DPlayback(m_attackDamageData.SoundData, transform.position);
                    }
                }
            }
            );


    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CharacterGroupNumber
{
    none,
    player,
    enemy,
    customer,
    storyskill,
    gangster,
    NPC,
}
public enum KnockBackType
{
    forward,
    radial
}

// ダメージを受ける者用のインターフェース(倉田)
public interface IDamageable
{

    public CharacterGroupNumber GroupNo { get; }


    // ノックバックあり
    public void Damaged(DamageNotification _dmgData, Collider _hitCol, float _knockBackMultiplier, bool _isStrongAttack, KnockBackType _knockBackType);
    
    // ノックバックなし
    public void Damaged(float _dmg, Collider _hitCol);

    // 指定HP以下にはならない（ヘンゼルとグレーテルスキル用）
    public void Damaged(DamageNotification _dmgData, Collider _hitCol, float _limitHP);


    // 一時的な、グループ比較用関数
    // 同じグループ以外であれば攻撃可能
    bool IsAttackable(CharacterGroupNumber _groupNo) { return GroupNo != _groupNo; }
}

// ダメージ通知用クラス
public class DamageNotification
{
    public CharacterStatus m_status;
    public AttackDamageData m_attackData;
    // ヒットストップの秒数
    public float m_hitStopTime = 0.0f;

    // 返信用データ(今は使わないのでコメント)
    public bool m_replyIsHit;
    public float m_finalDamageVal;
}
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// 現ステージの敵の難易度を保存しておく（伊波）

public class StageEnemyDifficultyLevel : BaseManager<StageEnemyDifficultyLevel>
{
    private StageData m_stageData;
    public void SetStageData(StageData data) { m_stageData = data; }
    public StageData StageData { get { return m_stageData; } }


    private StageEnemyStatus difficultyLevel = new StageEnemyStatus();
    public StageEnemyStatus DifficultyLevel { get { return difficultyLevel; } }

    private Difficulty m_difficulty = Difficulty.normal;
    public Difficulty Difficult { get { return m_difficulty; } }

    public void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.easy:
                difficultyLevel = new StageEnemyStatus(m_stageData.EasyStatus);
                m_difficulty = Difficulty.easy;
                break;
            case Difficulty.normal:
                difficultyLevel = new StageEnemyStatus(m_stageData.NormalStatus);
                m_difficulty = Difficulty.normal;
                break;
            case Difficulty.hard:
                difficultyLevel = new StageEnemyStatus(m_stageData.HardStatus);
                m_difficulty = Difficulty.hard;
                break;
        }

        BaseManager<EnemyDataBaseManager>.instance.ChangeEnemyLevel(difficultyLevel);
    }

    public enum Difficulty
    {
        none,
        easy,
        normal,
        hard,
    }
}


// ステージ難易度ごとの敵のステータス係数
[Serializable]
public class StageEnemyStatus
{
    // この辺追加したら EnemyData の ChangeEnemyLevel も追加しに行く

    [Header("移動速度係数")]
    [SerializeField, Range(0, 5)] private float m_moveSpeedCoefficient = 1.0f;
    public float MoveSpeedCoefficient { get { return m_moveSpeedCoefficient; } }

    [Header("戦闘関係")]
    [Tooltip("体力係数")]
    [SerializeField, Range(0, 5)] private float m_hpCoefficient = 1.0f;
    public float HPCoefficient { get { return m_hpCoefficient; } }

    [Tooltip("攻撃力係数")]
    [SerializeField, Range(0, 5)] private float m_attackCoefficient = 1.0f;
    public float AttackCoefficient { get { return m_attackCoefficient; } }

    [Tooltip("防御力係数")]
    [SerializeField, Range(0, 5)] private float m_defenseCoefficient = 1.0f;
    public float DefenseCoefficient { get { return m_defenseCoefficient; } }

    [Tooltip("攻撃間隔係数")]
    [SerializeField, Range(0, 5)] private float m_attackSpanCoefficient = 1.0f;
    public float AttackSpanCoefficient { get { return m_attackSpanCoefficient; } }


    [Header("索敵関係")]
    [Tooltip("索敵距離の加算量")]
    [SerializeField, Range(-5, 10)] private float m_searchDistAdd = 0f;
    public float SearchDistAdd { get { return m_searchDistAdd; } }

    [Tooltip("視野角の加算量")]
    [SerializeField, Range(-30, 180)] private int m_viewAngleAdd = 0;
    public int ViewAngleAdd { get { return m_viewAngleAdd; } }

    [Tooltip("チェイスする距離の加算量")]
    [SerializeField, Range(-5, 10)] private float m_chaseDistAdd = 0.0f;
    public float ChaseDistAdd { get { return m_chaseDistAdd; } }

    public StageEnemyStatus(StageEnemyStatus status)
    {
        m_moveSpeedCoefficient = status.m_moveSpeedCoefficient;
        m_hpCoefficient = status.m_hpCoefficient;
        m_attackCoefficient = status.m_attackCoefficient;
        m_defenseCoefficient = status.m_defenseCoefficient;
        m_attackSpanCoefficient = status.m_attackSpanCoefficient;
        m_searchDistAdd = status.m_searchDistAdd;
        m_viewAngleAdd = status.m_viewAngleAdd;
        m_chaseDistAdd = status.m_chaseDistAdd;
    }

    public StageEnemyStatus() { }
}
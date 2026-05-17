using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

//エネミーが範囲内にいればTrueを返す(山本)
//移動地点から離れていれば無視して戻る
//参考（EnemySearchTarget.cs）

[AddComponentMenu("")]
public class SearchEnemy : Decorator
{
    [SerializeField] private FlexibleTransform m_targetParameter;
    [SerializeField] private FlexibleComponent m_targetCore;


    [SerializeField]
    [SlotType(typeof(PlayerSkillsParameters))]
    private FlexibleComponent m_skillsParameters;

    private Collider m_myCollider;
    private readonly VisualFieldJudgment m_judgment = new();
    private ChaseData m_chaseData;

    protected override void OnStart()
    {
        m_myCollider = transform.root.GetComponentInChildren<Collider>();
        if (!m_myCollider)
        {
            Debug.LogError("Colliderが見つかりませんでした" + gameObject.name);
        }

        PlayerSkillsParameters parameters = m_skillsParameters.value as PlayerSkillsParameters;
        m_chaseData = parameters.ChaseData;
        if (m_chaseData == null)
        {
            Debug.Log("ChaseDataが取得できませんでした" + gameObject.name);
        }
    }

    protected override bool OnConditionCheck()
    {
        float nearestDist = m_chaseData.SearchCharacterDist;
        bool isFind = false;

        // 吸収攻撃を受けている敵は除外するように修正（山本）
        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara.GroupNo == CharacterGroupNumber.enemy && 
                chara.gameObject.layer != LayerMask.NameToLayer("AbsorbEnemy"))
            {

                if (chara.Status.m_hp.Value <= 0.0f)
                {
                    continue;
                }

                if(m_myCollider==null)
                {
                    continue;
                }

                if (m_judgment.SearchTarget(chara.gameObject, m_chaseData, m_myCollider, ref nearestDist))
                {
                    m_targetParameter.parameter.SetTransform(chara.transform);
                    //敵のCharacterCoreを参照
                    m_targetCore.parameter.SetComponent(chara);
                    isFind = true;
                    continue;
                }
            }
        }
        return isFind;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;
using Unity.VisualScripting;
using ConditionInfo;
using static Arbor.BehaviourTree.Decorator;

[AddComponentMenu("")]
public class ConditionCheck : Decorator
{

    [Header("この状態ではないときに処理を通す")]
    [SerializeField]
    private ConditionID m_conditionID = ConditionID.Normal;

    private CharacterCore m_characterCore = null;

    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
        foreach(var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if(core.GroupNo == CharacterGroupNumber.player)
            {
                m_characterCore = core;
            }
        }
    }

    protected override bool OnConditionCheck()
    {
        if (m_characterCore == null) return false;


        
        ConditionManager conditionmanager = m_characterCore.GetComponentInChildren<ConditionManager>();
        if (conditionmanager == null) return false;

        ICondition condition;
        // 指定のコンディションがるかどうか確認する
        for (int i = 0; i < conditionmanager.transform.childCount; i++)
        {
            condition = conditionmanager.transform.GetChild(i).GetComponent<ICondition>();
            if (condition == null) continue;
            if (condition.ConditionID == m_conditionID)
            {
                return false;
            }
        }

        return true;
    }

    protected override void OnEnd()
    {
    }
}

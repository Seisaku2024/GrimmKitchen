using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;
using ConditionInfo;

[AddComponentMenu("")]
public class AddCondition : ActionBehaviour
{
    [Header("与えるコンディション")]
    [SerializeField]
    private ConditionID m_conditionID = ConditionID.Normal;

    [SerializeField]
    private FlexibleTransform m_playerTrans;

    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
        if (m_playerTrans.value == null) return;

        var conditionData = ConditionDataBaseManager.instance.GetConditionData(m_conditionID);
        if (conditionData == null || conditionData.ConditionPrefab == null) return;

        ConditionManager conditionmanager = m_playerTrans.value.GetComponentInChildren<ConditionManager>();
        if (conditionmanager == null) return;

        conditionmanager.AddCondition(conditionData.ConditionPrefab,false);

    }

    protected override void OnExecute()
    {
    }

    protected override void OnEnd()
    {
    }
}

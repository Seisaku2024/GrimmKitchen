using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using ConditionInfo;

[AddComponentMenu("")]
public class AddConditionState : StateBehaviour
{
    [Header("与えるコンディション")]
    [SerializeField]
    private ConditionID m_conditionID = ConditionID.Normal;

    private CharacterCore m_playerCore = null;

    // Use this for initialization
    void Start()
    {

    }

    // Use this for awake state
    public override void OnStateAwake()
    {
        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if(core.GroupNo==CharacterGroupNumber.player)
            {
                m_playerCore = core;
            }
        }
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
       
    }

    // Use this for exit state
    public override void OnStateEnd()
    {
        if (m_playerCore == null) return;

        var conditionData = ConditionDataBaseManager.instance.GetConditionData(m_conditionID);
        if (conditionData == null || conditionData.ConditionPrefab == null) return;

        ConditionManager conditionmanager = m_playerCore.GetComponentInChildren<ConditionManager>();
        if (conditionmanager == null) return;

        conditionmanager.AddCondition(conditionData.ConditionPrefab, false);
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}

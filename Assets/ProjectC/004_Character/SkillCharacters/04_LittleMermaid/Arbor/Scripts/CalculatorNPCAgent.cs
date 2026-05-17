using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine.AI;

[BehaviourTitle("CalcAgentNPCParameters")]
[AddComponentMenu("")]
public class CalculatorNPCAgent : Calculator 
{
    [SerializeField, SlotType(typeof(PlayerSkillsParameters))]
    private FlexibleComponent m_storySkillParameters;

    [SerializeField] private OutputSlotFloat m_outMoveSpeed;
    [SerializeField] private OutputSlotTransform m_outTransform;
    [SerializeField] private OutputSlotFloat m_outStoppingDistance;
    [SerializeField] private OutputSlotBool m_vanishFlg;

    private PlayerSkillsParameters storySkillParameters;
    private NavMeshAgent agent;

    private Transform playerTrans;

    public override void OnCalculate() 
	{
        if(storySkillParameters==null)
        {
            storySkillParameters = m_storySkillParameters.value as PlayerSkillsParameters;
            if (storySkillParameters == null) return;
        }

        if(playerTrans==null)
        {
            foreach(var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if(core.GroupNo == CharacterGroupNumber.player)
                {
                    playerTrans = core.transform;
                }
            }

            if (playerTrans == null) return;
        }



        var characterCore = gameObject.transform.root.GetComponent<CharacterCore>();
        if (characterCore == null) return;

        m_outMoveSpeed.SetValue(characterCore.Status.WalkSpeed);

        m_outTransform.SetValue(playerTrans);

        if (!agent)
        {
            if (!storySkillParameters.TryGetComponent(out agent)) return;
        }
        else
        {
            m_outStoppingDistance.SetValue(agent.radius);
        }


        bool vanishFlg = false;
        storySkillParameters.DisappearTime -= Time.deltaTime;
        
        if (storySkillParameters.DisappearTime <= 0)
        {
            vanishFlg = true;
        }
        
        m_vanishFlg.SetValue(vanishFlg);

    }
}

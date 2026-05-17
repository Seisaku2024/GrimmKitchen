using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine.AI;
using HanselStageInfo;

[AddBehaviourMenu("Float/CalcNPCAgentParameters")]
[BehaviourTitle("CalcNPCAgentParameters")]
[AddComponentMenu("")]
public class CalculateNPCAgentParameters : Calculator
{
    [SerializeField, SlotType(typeof(NPCParameters))]
    private FlexibleComponent m_npcPrams;

    [SerializeField] private OutputSlotFloat m_outMoveSpeed;
    [SerializeField] private OutputSlotFloat m_outIdleMoveRadius;
    [SerializeField] private OutputSlotFloat m_outStoppingDistance;
    [SerializeField] private OutputSlot<Waypoint> m_waypoint;
    [SerializeField] private OutputSlotTransform m_targetTrans;
    [SerializeField] private OutputSlotBool m_moveFlg;
    [SerializeField] private OutputSlotBool m_noWaitPlayerFlg;

    [SerializeField]
    private float m_sightAngle = 180.0f;

    private NPCParameters npcParameters;
    private NavMeshAgent agent;
    private CharacterCore m_characterCore = null;


    public override void OnCalculate()
    {
        if (m_characterCore == null)
        {
            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.GroupNo == CharacterGroupNumber.player)
                {
                    m_characterCore = chara;
                    break;
                }
            }
        }

        if (!npcParameters)
        {
            npcParameters = m_npcPrams.value as NPCParameters;
            if (!npcParameters) return;
        }

        float moveSpeed = 0.0f;

        if (npcParameters.transform.root.TryGetComponent(out CharacterCore characterCore))
        {

            if (Vector3.Distance(characterCore.transform.position,
                m_characterCore.transform.position) >= npcParameters.ChanegMoveSpeedDist)
            {
                if (IsPlayerInSight()==false)
                {
                    moveSpeed = characterCore.Status.WalkSpeed;
                }
                else
                {
                    moveSpeed = characterCore.Status.DushSpeed;
                }
            }
            else
            {
                moveSpeed = characterCore.Status.DushSpeed;
            }

            npcParameters.MoveSpeed = moveSpeed;

        }

        m_outMoveSpeed.SetValue(moveSpeed);
        m_outIdleMoveRadius.SetValue(npcParameters.StopMoveDistance);

        // ターゲットTransform
        Transform targetTrans = null;
        targetTrans = npcParameters.TargetTransform;

        if (!agent)
        {
            if (!npcParameters.TryGetComponent(out agent)) return;
        }
        if (targetTrans == null)
        {
            m_outStoppingDistance.SetValue(agent.radius);
            return;
        }
        if (targetTrans.gameObject.TryGetComponent(out CapsuleCollider m_targetCollider))
        {
            float radius = transform.root.localScale.x * agent.radius;
            m_outStoppingDistance.SetValue(m_targetCollider.radius * (float)System.Math.Sqrt(2f) + radius);
        }
        else
        {
            m_outStoppingDistance.SetValue(agent.radius);
        }

        m_waypoint.SetValue(npcParameters.Waypoint);
        m_targetTrans.SetValue(targetTrans);
        m_moveFlg.SetValue(npcParameters.MoveTargetPositionFlg);
        m_noWaitPlayerFlg.SetValue(npcParameters.NoWaitPlayerFlg);

    }


    private bool IsPlayerInSight()
    {
        if (m_characterCore == null)
            return false;

        Vector3 directionToPlayer = (m_characterCore.transform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < m_sightAngle / 2)
        {
            return true;
        }


        return false;
    }

}

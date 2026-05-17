using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;
using HanselStageInfo;

[AddComponentMenu("")]
public class ChageNPCListTransform : ActionBehaviour
{
    [SerializeField, SlotType(typeof(NPCParameters))]
    private FlexibleComponent m_npcPrams;
    private NPCParameters npcParameters;

    protected override void OnAwake()
    {
        if (!npcParameters)
        {
            npcParameters = m_npcPrams.value as NPCParameters;
            if (!npcParameters) return;
        }
    }

    protected override void OnStart()
    {
        if (!npcParameters) return;
        if (npcParameters.ArriveTargetPositionFlg == false && npcParameters.TargetTransform) return;

        npcParameters.SetTargetTransform();
        npcParameters.ArriveTargetPositionFlg = false;


    }

    protected override void OnExecute()
    {
        FinishExecute(true);
        return;
    }

    protected override void OnEnd()
    {
    }
}

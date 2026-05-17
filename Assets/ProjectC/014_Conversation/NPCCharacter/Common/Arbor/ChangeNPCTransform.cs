using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using HanselStageInfo;

[AddComponentMenu("")]
public class ChangeNPCTransform : StateBehaviour {

    [SerializeField, SlotType(typeof(NPCParameters))]
    private FlexibleComponent m_npcPrams;
    private NPCParameters npcParameters;


    // Use this for initialization
    void Start () 
	{

        

    }

	// Use this for awake state
	public override void OnStateAwake() 
	{
        if (!npcParameters)
        {
            npcParameters = m_npcPrams.value as NPCParameters;
            if (!npcParameters) return;
        }
    }

	// Use this for enter state
	public override void OnStateBegin() 
	{
        if (!npcParameters) return;
        npcParameters.CheckListNum();
    }

	// Use this for exit state
	public override void OnStateEnd() {
	}
	
	// OnStateUpdate is called once per frame
	public override void OnStateUpdate() {
	}

	// OnStateLateUpdate is called once per frame, after Update has finished.
	public override void OnStateLateUpdate() {
	}
}

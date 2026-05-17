using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class CheckNPCFlg : Decorator {

    [SerializeField]
    private FlexibleBool m_enabled;

    protected override void OnAwake() {
	}

	protected override void OnStart() {
	}

	protected override bool OnConditionCheck() 
	{
        if (m_enabled.value == false)
        {
            return false;
        }
        else
        {
            return true;
        }

	}

	protected override void OnEnd() {
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class CheckFlg : Decorator
{
    [SerializeField]
    private FlexibleBool m_enabled;

    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
       

    }

    protected override bool OnConditionCheck()
    {
        if (m_enabled.value)
        {
            return true;
        }

        return false;

    }

    protected override void OnEnd()
    {
    }
}

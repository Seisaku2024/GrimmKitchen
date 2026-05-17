using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class CheckCompleteList : Decorator
{
    private CharacterCore m_characterCore = null;

    protected override void OnAwake()
    {

        if (transform.root.gameObject.TryGetComponent(out CharacterCore characterCore))
        {
            m_characterCore = characterCore;
        }
    }

    protected override void OnStart()
    {
    }

    protected override bool OnConditionCheck()
    {
        if (m_characterCore.NPCParameters.MoveTargetPositionFlg == false)
        {
            m_characterCore.NPCParameters.TargetTransform = null;
            m_characterCore.m_animator.SetBool("CallingFlg", false);
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override void OnEnd()
    {
    }
}

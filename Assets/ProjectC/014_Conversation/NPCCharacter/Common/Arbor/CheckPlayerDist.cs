using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;
using System;

[AddComponentMenu("")]
public class CheckPlayerDist : Decorator
{

    [SerializeField]
    private FlexibleFloat m_stopDist = new FlexibleFloat();
    [SerializeField]
    private FlexibleBool m_enabled;
    [SerializeField]
    private FlexibleBool m_noWaitFlg;

    private Transform m_playerTrans = null;
    private CharacterCore m_characterCore = null;


    protected override void OnAwake()
    {
        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (core.GroupNo == CharacterGroupNumber.player)
            {
                m_playerTrans = core.transform;
            }
        }

      
    }

    protected override void OnStart()
    {
       

    }

    protected override bool OnConditionCheck()
    {
        if (m_enabled.value == false)
        {
            return false;
        }

        // 待機しないフラグがONになったら必ずTrue
        if(m_noWaitFlg.value==true)
        {
            return true;
        }

        if (Vector3.Distance(m_playerTrans.position, transform.position) >= m_stopDist.value)
        {
            return false;
        }
        else
        {
     
            return true;
        }

    }

    protected override void OnEnd()
    {
    }
}

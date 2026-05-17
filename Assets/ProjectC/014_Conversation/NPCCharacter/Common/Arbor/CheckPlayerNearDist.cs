using UnityEngine;
using Arbor;
using Arbor.BehaviourTree;

[AddComponentMenu("")]
public class CheckPlayerNearDist : Decorator {
    [SerializeField]
    private FlexibleFloat m_stopDist = new FlexibleFloat();
    [SerializeField]
    private FlexibleBool m_enabled;
   

    private Transform m_playerTrans = null;
   

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

        if (Vector3.Distance(m_playerTrans.position, transform.position) >=  m_stopDist.value)
        {
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

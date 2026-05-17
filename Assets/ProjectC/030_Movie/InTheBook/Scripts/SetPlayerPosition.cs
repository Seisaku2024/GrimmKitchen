using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class SetPlayerPosition : MonoBehaviour
{
    [SerializeField]
    private Transform m_setTransform = null;

    [SerializeField]
    private Transform m_targetTransform = null;

    private CharacterCore m_playerCore = null;

    void Start()
    {
        foreach(var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if(core.GroupNo==CharacterGroupNumber.player)
            {
                m_playerCore = core;
                break;
            }
        }

    }

    public void SetPosition()
    {
        if (m_setTransform == null) return;
        if (m_playerCore == null) return;

        m_playerCore.CharaCtrl.SetPositionMotor(m_setTransform.position);

        Vector3 targetPos = m_targetTransform.position;
        targetPos.y = m_playerCore.transform.position.y;

        m_playerCore.SetRotateToTarget(targetPos,false);

    }
}

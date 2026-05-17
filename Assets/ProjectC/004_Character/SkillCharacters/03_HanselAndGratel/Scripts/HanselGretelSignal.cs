using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselGretelSignal : MonoBehaviour
{
    [SerializeField]
    private CinemachineBlendListCamera m_cinemachine;

    [SerializeField]
    private CharacterCore  m_hanselCore;

    [SerializeField]
    private CharacterCore m_gtetelCore;

    private float m_originTimeScale = 1.0f;


    public void SetPriority()
    {
        m_cinemachine.Priority = 0;
    }

    public void StopTimeScale()
    {
        if(m_hanselCore==null || m_gtetelCore ==null)
        {
            return;
        }

        foreach(var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if(core != m_hanselCore || core != m_gtetelCore)
            {
                core.m_animator.speed = 0.0f;

            }
        }
      
    }

    public void StartTimeScale()
    {
        if (m_hanselCore == null || m_gtetelCore == null)
        {
            return;
        }

        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (core != m_hanselCore || core != m_gtetelCore)
            {
                core.m_animator.speed = 1.0f;

            }
        }
    }

}

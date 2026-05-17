using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OPAnimatorSetTriger : MonoBehaviour
{
    enum OPAnimatorTriger
    {
        None,
        FemaleWalk,
        MaleWalk,
        Eating,
        SitIdle,
        Claim,
        Apologize,
        SitNod,
        SitLaugh,
        SitTalk,
        SitMaleIdle,
        SitFemaleIdle,
        StandTalk,
        StandLaugh,
        BasicWalk,
    }

    [SerializeField]
    private Animator m_animator = null;

    [SerializeField]
    private OPAnimatorTriger m_setTriger = OPAnimatorTriger.None;

    private void OnEnable()
    {
        ChangeAnimation();
    }

    void Start()
    {
        ChangeAnimation();
    }

    private void ChangeAnimation()
    {
        if (m_animator == null) { return; }

        switch (m_setTriger)
        {
            case OPAnimatorTriger.FemaleWalk:
                m_animator.SetTrigger("IsFemaleWalk");
                break;

            case OPAnimatorTriger.MaleWalk:
                m_animator.SetTrigger("IsMaleWalk");
                break;

            case OPAnimatorTriger.Eating:
                m_animator.SetTrigger("IsEating");
                break;

            case OPAnimatorTriger.SitIdle:
                m_animator.SetTrigger("IsSitIdle");
                break;

            case OPAnimatorTriger.Claim:
                m_animator.SetTrigger("IsClaim");
                break;

            case OPAnimatorTriger.Apologize:
                m_animator.SetTrigger("IsApologize");
                break;

            case OPAnimatorTriger.SitNod:
                m_animator.SetTrigger("IsSitNod");
                break;

            case OPAnimatorTriger.SitLaugh:
                m_animator.SetTrigger("IsSitLaugh");
                break;

            case OPAnimatorTriger.SitTalk:
                m_animator.SetTrigger("IsSitTalk");
                break;

            case OPAnimatorTriger.SitMaleIdle:
                m_animator.SetTrigger("IsSitMaleIdle");
                break;

            case OPAnimatorTriger.SitFemaleIdle:
                m_animator.SetTrigger("IsSitFemaleIdle");
                break;

            case OPAnimatorTriger.StandTalk:
                m_animator.SetTrigger("IsMaleWalk");
                break;

            case OPAnimatorTriger.StandLaugh:
                m_animator.SetTrigger("IsMaleWalk");
                break;
            case OPAnimatorTriger.BasicWalk:
                m_animator.SetTrigger("IsBasicWalk");
                break;

            default:
                break;

        }
    }


}

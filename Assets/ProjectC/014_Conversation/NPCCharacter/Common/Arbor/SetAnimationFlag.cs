using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;
using System;

[AddComponentMenu("")]
public class SetAnimationFlag : ActionBehaviour
{
    // 指定したフラグのONOFF（山本）
    [Header("フラグ")]
    [SerializeField] String m_flagName = "";
    [Header("True or False")]
    [SerializeField] private bool m_flg = false;

    [Header("現在どのステートでなければフラグをセットするのか")]
    [SerializeField] String m_nowStateName = "";


    private CharacterCore m_character;
    private Animator m_animator;

    protected override void OnAwake()
    {
        if (transform.root.gameObject.TryGetComponent(out CharacterCore characterCore))
        {
            m_character = characterCore;
            m_animator = characterCore.m_animator;

        }
    }

    protected override void OnStart()
    {
        if (m_animator)
        {
            if (m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_nowStateName) == false)
            {
                if (m_flg && (m_character.NPCParameters.NoChangeTurnAroundFlg == false))
                {
                    m_animator.SetBool("CallingFlg", true);
                }
                else
                {
                    m_animator.SetBool("CallingFlg", false);
                }

            }
            else
            {
                return;
            }
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

// 登録されているVFXを再生、停止するAnimatorEvent（山本）
[System.Serializable]
public class AnimatorEventPlayRegisterVFXEffect : AnimatorEvents.EventNodeBase
{
    [Header("ステート切り替え時に停止するか")]
    [SerializeField] private bool m_stopVFXExitAnimEventFlg = true;
    [Header("登録されているVFXの名前")]
    [SerializeField]
    private string m_registoryVFXName;
    [Header("VFXで再生するためのイベント名")]
    [SerializeField]
    private string m_startEventName;
    [Header("VFXで停止するするためのイベント名")]
    [SerializeField]
    private string m_stopEventName;

 
    private VisualEffect m_effect;

    public override void OnEvent(Animator animator)
    {
        if (animator.transform.root.TryGetComponent(out VFXEffectRegistry vFXEffectRegistry))
        {
            m_effect = vFXEffectRegistry.VFXResistoryList[m_registoryVFXName];
            if (m_effect)
            {
                m_effect.SendEvent(m_startEventName);
            }
        }
    }
    public override void OnExit(Animator animator)
    {
        base.OnExit(animator);
        if (m_stopVFXExitAnimEventFlg)
        {
            if (m_effect == null) return;
            m_effect.SendEvent(m_stopEventName);
        }
    }



}

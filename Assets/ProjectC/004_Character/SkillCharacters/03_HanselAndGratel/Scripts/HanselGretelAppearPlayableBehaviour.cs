using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class HanselGretelAppearBehaviour : PlayableBehaviour
{
    // ヘンゼルとグレーテルの出現イベントタイムラインで他のキャラクターのアニメーションを止める処理（山本）

    public CharacterCore m_hansel = null;
    public CharacterCore m_gretel = null;


    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {

    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        if (m_hansel == null || m_gretel == null)
        {
            return;
        }

        if (IMetaAI<CharacterCore>.Instance == null)
        {
            return;
        }

        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            core.StopAnimationSpeedFlg = false;
            core.m_animator.speed = 1.0f;

        }
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {

    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {

    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (m_hansel == null || m_gretel == null)
        {
            return;
        }

        if (IMetaAI<CharacterCore>.Instance == null)
        {
            return;
        }

        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (core == m_hansel || core == m_gretel)
            {
                core.m_animator.speed = 1.0f;
            }
            else
            {
                core.StopAnimationSpeedFlg = true;
                core.m_animator.speed = 0.0f;
            }
        }

    }
}

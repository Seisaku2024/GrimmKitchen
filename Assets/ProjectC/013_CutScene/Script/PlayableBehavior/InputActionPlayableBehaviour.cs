using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class InputActionPlayableBehaviour : PlayableBehaviour
{
    [SerializeField]
    private InputActionButtonController m_inputActionButtonController = null;
    public InputActionButtonController InputActionButtonController 
    {
        get { return m_inputActionButtonController; }
        set { m_inputActionButtonController = value; }
    }

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if(m_inputActionButtonController)
        {
            m_inputActionButtonController.OnUpdate();
        }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}

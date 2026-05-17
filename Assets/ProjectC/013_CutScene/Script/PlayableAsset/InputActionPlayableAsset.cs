using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class InputActionPlayableAsset : PlayableAsset
{
    [SerializeField]
    private ExposedReference<InputActionButtonController> m_inputActionButtonController;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        
        InputActionPlayableBehaviour behaviour = new InputActionPlayableBehaviour();

        behaviour.InputActionButtonController = m_inputActionButtonController.Resolve(graph.GetResolver());

        return ScriptPlayable<InputActionPlayableBehaviour>.Create(graph, behaviour);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class ChangeSceneMarker : Marker, INotification
{
    [Header("SceneTrasitionManagerがコンポーネントされているオブジェクト")]
    [SerializeField]
    private GameObject m_sceneTransitionManagerObj;
    public SceneTransitionManager SceneTransitionManager() 
    {
        if(m_sceneTransitionManagerObj==null)
        {
            return null;
        }

        if (m_sceneTransitionManagerObj.TryGetComponent(out SceneTransitionManager component))
        {
            return component;
        }

        return null;
    }

    public PropertyName id => new PropertyName("SceneChange");

}

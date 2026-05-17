using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]


public class TransitionMarker : Marker,INotification
{
    [SerializeField]
    private float m_startTransitionTime = 0.5f;
    [SerializeField]
    private float m_endTransitionTime = 0.5f;
    public float StartTransitionTime => m_startTransitionTime;
    public float EndTransitionTime => m_endTransitionTime;

    public PropertyName id => new PropertyName("Trnasition");
}


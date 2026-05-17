using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TrasitionMarkerReceiver : MonoBehaviour, INotificationReceiver
{
    [SerializeField]
    private Image m_image = null;

    [Header("最初にイメージのアルファ値を1.0にするかどうかのフラグ")]
    [SerializeField]
    private bool m_bFirstAlphaFullFlg = false;

    void Awake()
    {
        if (m_image == null) { return; }
        Color color = m_image.color;
        color.a = 0.0f;
        m_image.color = color;
    }

    void Start()
    {
        if (m_image == null) { return; }
        Color color = m_image.color;
        if (m_bFirstAlphaFullFlg)
        {
            color.a = 1.0f;
        }
        else
        {
            color.a = 0.0f;
        }
            m_image.color = color;
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        var marker = notification as TransitionMarker;
        if (marker == null)
        {
            return;
        }

        this.Transition(marker.StartTransitionTime, marker.EndTransitionTime);
    }

    private void Transition(float startTime, float endTime)
    {
        DOVirtual.Float(0.0f, 1.0f, startTime,
            (val) =>
            {
                Color color = m_image.color;
                color.a = val;
                m_image.color = color;
            }
            ).OnComplete
            (
            () => DOVirtual.Float(1.0f, 0.0f, endTime,
            (val) =>
            {
                Color color = m_image.color;
                color.a = val;
                m_image.color = color;
            }
            )
            );

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

using DG.Tweening;

using TMPro;

public class ShowSpeechBubble : MonoBehaviour
{
    private enum SpeechBubbleType
    {
        Alway,
        Appearance,
    }

    [Header("表示/非表示を行うキャンバス")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;

    [Header("DOスピード")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_doSpead = 0.2f;

    [Header("種類")]
    [SerializeField]
    private SpeechBubbleType m_type = SpeechBubbleType.Alway;

    [EnableIf("m_type", SpeechBubbleType.Appearance)]
    [Header("当たり判定を行うTag")]
    [SerializeField]
    [Tag]
    private string m_tag = "Player";

    private GameObject m_tagetObject = null;

    [EnableIf("m_type", SpeechBubbleType.Appearance)]
    [Header("距離")]
    [SerializeField]
    private float m_distance = 5.0f;

    // 現在表示中か
    private bool m_isShowing = false;

    private Tween m_fadeTween;

    private void Start()
    {
        GetTargetObject();
        InitText();
    }

    private void Update()
    {
        CalcDistanceTargetObject();
    }

    private void CalcDistanceTargetObject()
    {
        if (m_type == SpeechBubbleType.Alway) return;
        if (m_tagetObject == null) return;

        var pos = transform.position;
        var targetPos = m_tagetObject.transform.position;

        var range = Vector3.Distance(targetPos, pos);

        bool shouldShow = range <= m_distance;

        // 状態が変わっていなければ何もしない
        if (shouldShow == m_isShowing)
            return;

        m_isShowing = shouldShow;

        if (m_isShowing)
        {
            OnShow();
        }
        else
        {
            OnHide();
        }
    }

    private void GetTargetObject()
    {
        m_tagetObject = GameObject.FindGameObjectWithTag(m_tag);
    }

    private void InitText()
    {
        if (m_type == SpeechBubbleType.Alway) return;
        if (m_canvasGroup == null) return;

        m_canvasGroup.alpha = 0.0f;
        m_isShowing = false;
    }

    private void OnShow()
    {
        if (m_canvasGroup == null) return;

        m_fadeTween?.Kill();

        m_fadeTween = m_canvasGroup
            .DOFade(1.0f, m_doSpead);
    }

    private void OnHide()
    {
        if (m_canvasGroup == null) return;

        m_fadeTween?.Kill();

        m_fadeTween = m_canvasGroup
            .DOFade(0.0f, m_doSpead);
    }

    private void OnDestroy()
    {
        m_fadeTween?.Kill();
    }
}
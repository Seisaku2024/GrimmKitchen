using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenuButton : ButtonData
{
    // 制作者 田内

    [Header("CanvaGroup")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;

    [Header("点滅")]
    [SerializeField]
    private BlinkingCanvasGroup m_blinkingCanvasGroup = null;

    [SerializeField]
    private Image m_image = null;

    // 未選択時の透明度
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_deselectedAlpha = 0.5f;

    // 未選択時の色
    [SerializeField]
    private Color m_deselectedColor = Color.white;


    override public void OnSelectUpdate()
    {
        if (m_blinkingCanvasGroup) m_blinkingCanvasGroup.StartEasing();
        if (m_canvasGroup) m_canvasGroup.alpha = 1.0f;
        if (m_image) m_image.color = Color.white;
    }

    override public void OnUnselectUpdate()
    {
        if (m_blinkingCanvasGroup) m_blinkingCanvasGroup.EndEasing();
        if (m_canvasGroup) m_canvasGroup.alpha = m_deselectedAlpha;
        if (m_image) m_image.color = m_deselectedColor;
    }

}
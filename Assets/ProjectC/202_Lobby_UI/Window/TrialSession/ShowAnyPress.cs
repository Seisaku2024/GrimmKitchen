using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowAnyPress : WindowUpdateBase
{
    [SerializeField]
    private GameObject m_showObject = null;

    [SerializeField]
    private DoAlphaCanvasGroup m_doAlphaCanvasGroup = null;

    bool m_isShow = false;

    public override void OnInitialize()
    {
        if (m_showObject == null)
        {
            m_showObject = gameObject;
        }

        if (m_showObject != null)
        {
            m_showObject.SetActive(m_isShow);
        }
    }

    public override void OnUpdate()
    {
        if (PlayerInputManager.instance.IsPressCurrentAnyKey())
        {
            // 一度だけ
            if (m_isShow == false) OnShow();
        }
    }

    private void OnShow()
    {
        m_isShow = true;

        if (m_showObject != null)
        {
            m_showObject.SetActive(m_isShow);
        }
        if (m_doAlphaCanvasGroup != null)
        {
            m_doAlphaCanvasGroup.StartDoTween();
        }

    }
}

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    // ムービーのTransitionをコントロールする
    // 山本

    [Header("トランジション用の画像")]
    [SerializeField]
    private Image m_transitionImage = null;

    private Tweener m_tween;

    public void StartTransition(float _beginTransitionTime = 0.5f, float _endTransitionTime = 0.5f)
    {
        if (m_transitionImage == null)
        {
            return;
        }

        if (m_transitionImage.gameObject.activeSelf == false)
        {
            m_transitionImage.gameObject.SetActive(true);
        }

        m_tween = DOVirtual.Float(0.0f, 1.0f, _beginTransitionTime,
            (val) =>
            {
                ImageAlphaUpdate(val);
            }
            ).OnComplete
            (
            () =>
            {
                m_tween = DOVirtual.Float(1.0f, 0.0f, _endTransitionTime,
               (val) =>
               {
                   ImageAlphaUpdate(val);
               }
               ).SetEase<Tweener>(Ease.InSine);
            }
            ).SetUpdate(true);

    }

    public void StartTransition(float _beginTransitionTime = 0.5f, float _endTransitionTime = 0.5f,Action action = null)
    {
        if (m_transitionImage == null)
        {
            return;
        }

        if (m_transitionImage.gameObject.activeSelf == false)
        {
            m_transitionImage.gameObject.SetActive(true);
        }

        m_tween = DOVirtual.Float(0.0f, 1.0f, _beginTransitionTime,
            (val) =>
            {
                ImageAlphaUpdate(val);
            }
            ).OnComplete
            (
            () =>
            {
               
                // Transition完了時に何かの処理呼ぶならここ
                action?.Invoke();
                
                m_tween = DOVirtual.Float(1.0f, 0.0f, _endTransitionTime,
               (val) =>
               {
                   ImageAlphaUpdate(val);
               }
               ).SetEase<Tweener>(Ease.InSine);
            }
            ).SetUpdate(true);

    }

    public void ImageAlphaUpdate(float _val)
    {

        if (m_transitionImage == null)
        {
            m_tween?.Kill();
            return;
        }

        if (m_transitionImage.gameObject.activeSelf == false)
        {
            m_transitionImage.gameObject.SetActive(true);
        }

        Color color = m_transitionImage.color;
        color.a = _val;
        m_transitionImage.color = color;

    }

    private void OnDestroy()
    {
        if (m_tween != null)
        {
            m_tween.Kill();

            if (m_transitionImage == null)
            {
                return;
            }

            Color color = m_transitionImage.color;
            color.a = 0.0f;
            m_transitionImage.color = color;

        }
    }

}

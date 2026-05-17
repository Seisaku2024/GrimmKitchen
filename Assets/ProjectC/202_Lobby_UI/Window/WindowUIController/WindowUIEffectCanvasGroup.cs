using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowUIEffectCanvasGroup : BaseWindowUIEffect
{
    // 制作者 田内

    [Header("キャンバスグループ")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;


    [Header("実行中透明度")]
    [SerializeField]
    protected float m_alpha = 1.0f;

    [Header("非実行中透明度")]
    [SerializeField]
    protected float m_unAlpha = 0.5f;


    //=====================================================
    //                      実行処理
    //=====================================================

    public override UniTask PlayEffect()
    {

        SetAlpha(m_alpha);

        return base.PlayEffect();
    }


    public override UniTask UnPlayEffect()
    {

        SetAlpha(m_unAlpha);

        return base.UnPlayEffect();
    }


    private void SetAlpha(float _alpha)
    {
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }
        m_canvasGroup.alpha = _alpha;
    }
}

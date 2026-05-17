using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using NaughtyAttributes;

public class DoRotate : BaseDoTweenUI
{
    // 制作者 田内
    // UIを拡縮する

    [Header("UIRectTransform")]
    [SerializeField]
    private RectTransform m_rectTransform = null;

    [Header("初期の回転")]
    [SerializeField]
    private Vector3 m_initRotate = new Vector3(0.0f, 0.0f, 0.0f);

    [System.Serializable]
    private class DoRotateUIData : BaseEasingData
    {
        [Header("回転")]
        [SerializeField]
        private Vector3 m_targetRotate = Vector3.one;

        public Vector3 TargetScale
        {
            get { return m_targetRotate; }
        }
    }

    [Header("目標の大きさ")]
    [SerializeField]
    private List<DoRotateUIData> m_targetScaleList = new();


    //========================================================
    //                       実行処理
    //========================================================

    protected override void OnInitialize()
    {
        if (m_rectTransform == null)
        {
            // 無かったらアタッチされているオブジェクトを対象に行う
            if (gameObject.TryGetComponent<RectTransform>(out var rect))
            {
                m_rectTransform = rect;
            }
            else
            {
                Debug.LogError("RectTransformがシリアライズされていません");
                return;
            }
        }

        // 初期のサイズをセット
        m_rectTransform.localEulerAngles = m_initRotate;
    }


    public override void StartDoTween()
    {
        if (m_rectTransform == null)
        {
            Debug.LogError("RectTransformがシリアライズされていません");
            return;
        }


        // 一度初期化
        KillSequence();

        // 開始
        m_sequence = DOTween.Sequence();

        foreach (var data in m_targetScaleList)
        {
            m_sequence.Append(
            m_rectTransform.DOLocalRotate(data.TargetScale,data.Duration).
            SetDelay(data.Delay).
            SetEase(data.Ease).
            SetLink(gameObject)).
            SetUpdate(m_isGameStopMove);
        }

        m_sequence.SetLoops(m_loopCount, m_loopType);
        m_sequence.SetUpdate(m_isGameStopMove);

    }

}

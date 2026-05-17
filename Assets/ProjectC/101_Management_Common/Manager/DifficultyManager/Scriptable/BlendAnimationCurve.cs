//  http://kan-kikuchi.hatenablog.com/entry/BlendAnimationCurve
//  Created by kan.kikuchi

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif


/// <summary>
/// 複数のAnimationCurveをブレンドして使えるクラス
/// </summary>
[Serializable]
public class BlendAnimationCurve
{

    //カーブとその重みのペア
    [Serializable]
    public struct CurveWeightPair
    {
        public string Description;

        public enum CurveSourceType { Internal, External }
        public CurveSourceType sourceType;

        [SerializeField]
        private AnimationCurve _internalCurve;
        public AnimationCurve InternalCurve => _internalCurve;

        [SerializeField]
        private AnimationCurveData _externalCurve;
        public AnimationCurveData ExternalCurve => _externalCurve;

        [SerializeField]
        private float _weight;
        public float Weight => _weight;

        public AnimationCurve GetCurve()
        {
            return sourceType == CurveSourceType.External && _externalCurve != null
                ? _externalCurve.m_animationCurve
                : _internalCurve;
        }

        public CurveWeightPair(AnimationCurve curve, float weight)
        {
            Description = string.Empty;
            sourceType = CurveSourceType.Internal;
            _internalCurve = curve;
            _externalCurve = null;
            _weight = weight;
        }
    }

    [SerializeField]
    private List<CurveWeightPair> _curveWeightPairs = new List<CurveWeightPair>();

    //=================================================================================
    //初期化
    //=================================================================================

    public BlendAnimationCurve() { }

    public BlendAnimationCurve(params AnimationCurve[] curves)
    {
        _curveWeightPairs = curves.Select(curve => new CurveWeightPair(curve, 1)).ToList();
    }

    public BlendAnimationCurve(params CurveWeightPair[] curveWeightPairs)
    {
        _curveWeightPairs = curveWeightPairs.ToList();
    }

    //=================================================================================
    //追加
    //=================================================================================

    /// <summary>
    /// 重みを指定してカーブを追加
    /// </summary>
    public void Add(AnimationCurve curve, float weight = 1.0f)
    {
        _curveWeightPairs.Add(new CurveWeightPair(curve, weight));
    }

    //=================================================================================
    //取得
    //=================================================================================

    /// <summary>
    /// 指定時間の値を取得
    /// </summary>
    public virtual float Evaluate(float time)
    {
        if (_curveWeightPairs.Count == 0)
        {
            Debug.LogError($"CurveWeightPairが設定されていません");
            return 0;
        }

        float totalWeight = 0f;
        float blendedValue = 0f;

        foreach (var pair in _curveWeightPairs)
        {
            AnimationCurve curve = pair.GetCurve();
            if (curve == null) continue;

            totalWeight += pair.Weight;
            blendedValue += curve.Evaluate(time) * pair.Weight;
        }

        return totalWeight > 0f ? blendedValue / totalWeight : 0f;
    }

    /// <summary>
    /// DotweenのEase用の取得メソッド
    /// </summary>
    public float EaseEvaluate(float time, float duration, float overshootOrAmplitude, float period)
    {
        if (duration <= 0)
        {
            return 0f;
        }
        return Evaluate(time / duration);
    }

    public AnimationCurve GetBlendedCurve(int sampleCount = 50, float startTime = 0f, float endTime = 1f)
    {
        AnimationCurve blendedCurve = new AnimationCurve();

        if (_curveWeightPairs.Count == 0)
        {
            return blendedCurve;
        }

        float step = (endTime - startTime) / (sampleCount - 1);

        for (int i = 0; i < sampleCount; i++)
        {
            float time = startTime + step * i;
            float value = Evaluate(time);
            blendedCurve.AddKey(time, value);
        }

        return blendedCurve;
    }

}
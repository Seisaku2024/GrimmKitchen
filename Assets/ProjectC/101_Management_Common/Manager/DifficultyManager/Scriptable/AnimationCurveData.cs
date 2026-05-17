using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimationCurveData", menuName = "Animation/Animation Curve Data")]
public class AnimationCurveData : ScriptableObject
{
    public AnimationCurve m_animationCurve = null;
    public float Evaluate(float time)
    {
        return m_animationCurve.Evaluate(time);
    }
}

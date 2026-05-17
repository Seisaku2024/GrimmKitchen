using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerEnterProbabilityData", menuName = "ScriptableObjects/DifficultyData/CustomerEnterProbability")]
public class CustomerEnterProbabilityData : ScriptableObject
{
    [field: SerializeField, Tooltip("(0.00~1.00)% 経営時間(0~1)割合")]
    [Header("通行客入店確率")]
    public BlendAnimationCurve m_customerEnterProbability;
}

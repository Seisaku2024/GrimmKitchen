using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GangsterAppearProbabilityData", menuName = "ScriptableObjects/DifficultyData/GangsterAppearProbability")]
public class GangsterAppearProbabilityData : ScriptableObject
{
    [Header("迷惑客生成確率")]
    [field: SerializeField, Tooltip("(0.00~1.00)% 経営時間(0~1)割合")]
    public BlendAnimationCurve m_gangsterApeearProbability;
    [Header("迷惑客生成クールタイム")]
    [field: SerializeField, Tooltip("(0.1=1秒) 経営時間(0~1)割合")]
    public BlendAnimationCurve m_gangsterAppearCoolTime;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;

[CreateAssetMenu(fileName = "ChallengeTypeData", menuName = "ScriptableObjects/Challenge/作成 ChallengeTypeData")]
public class ChallengeTypeData : ScriptableObject
{
    // 制作者 田内
    // チャレンジ情報

    //===================================
    // Type

    [Header("Type")]
    [SerializeField]
    private ChallengeType m_challengeType = ChallengeType.None;

    public ChallengeType ChalengeType
    {
        get { return m_challengeType; }
    }

    //====================================
    // チャレンジタイプカラー

    [Header("カラー")]
    [SerializeField]
    private Color m_challengeTypeColor = Color.white;

    public Color ChallengeTypeColor
    {
        get { return m_challengeTypeColor; }
    }
}

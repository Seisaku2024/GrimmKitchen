using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "ChallengePlayTypeData", menuName = "ScriptableObjects/Challenge/作成 ChallengePlayTypeData")]
public class ChallengePlayTypeData : ScriptableObject
{
    // 制作者 田内
    // チャレンジのプレイタイプデータ


    //=============================

    [Header("プレイタイプ")]
    [SerializeField]
    private ChallengePlayType m_challengePlayType = ChallengePlayType.None;

    public ChallengePlayType ChallengePlayType
    {
        get { return m_challengePlayType; }
    }

    //==============================

    [Header("名前")]
    [SerializeField]
    private LocalizedString m_playTypeName = new();

    public LocalizedString PlayTypeName
    {
        get
        {
            return m_playTypeName;
        }
    }
}

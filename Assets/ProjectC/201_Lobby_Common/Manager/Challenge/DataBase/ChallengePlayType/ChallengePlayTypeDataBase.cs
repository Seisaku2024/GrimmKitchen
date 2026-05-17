using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChallengePlayTypeDataBase", menuName = "ScriptableObjects/Challenge/作成 ChallengePlayTypeDataBase")]
public class ChallengePlayTypeDataBase : ScriptableObject
{
    // 制作者 田内
    // チャレンジタイプデータをまとめるデータベース

    [Header("チャレンジリスト")]
    [SerializeField]
    private List<ChallengePlayTypeData> m_challengePlayTypeDataList = new();

    public List<ChallengePlayTypeData> ChallengePlayTypeDataList
    {
        get { return m_challengePlayTypeDataList; }
    }
}

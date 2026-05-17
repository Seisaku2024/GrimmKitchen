using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChallengeTypeDataBase", menuName = "ScriptableObjects/Challenge/作成 ChallengeTypeDataBase")]
public class ChallengeTypeDataBase : ScriptableObject
{
    // 制作者 田内
    // チャレンジタイプデータをまとめるデータベース

    [Header("チャレンジリスト")]
    [SerializeField]
    private List<ChallengeTypeData> m_challengeTypeDataList = new();

    public List<ChallengeTypeData> ChallengeTypeDataList
    {
        get { return m_challengeTypeDataList; }
    }

}

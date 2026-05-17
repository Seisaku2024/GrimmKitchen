using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ChallengeDataBase", menuName = "ScriptableObjects/Challenge/作成 ChallengeDataBase")]
public class ChallengeDataBase : ScriptableObject
{
    // 制作者 田内
    // チャレンジデータをまとめるデータベース

    [Header("チャレンジリスト")]
    [SerializeField]
    private List<ChallengeData> m_challengeDataList = new();

    public List<ChallengeData> ChallengeDataList
    {
        get { return m_challengeDataList; }
    }

}

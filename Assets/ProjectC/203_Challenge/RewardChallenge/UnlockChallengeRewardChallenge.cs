using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;

public class UnlockChallengeRewardChallenge : BaseRewardChallengeData
{
    // 制作者 田内
    // チャレンジをアンロックする報酬


    [Header("アンロックチャレンジID")]
    [SerializeField]
    private ChallengeID m_challengeID = ChallengeID.None;


    //=============================================
    //                  実行処理
    //=============================================

    public override void UpdateRewardChallenge()
    {
        var data = ChallengeDataBaseManager.instance.GetData(m_challengeID);
        if (data == null) return;

        data.ClearChallengeData.IsLock = false;
    }

}

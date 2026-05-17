using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageInfo;

public class UnlockStageRewardChallenge : BaseRewardChallengeData
{
    // 制作者 田内
    // ステージをアンロックする報酬

    [Header("アンロックステージID")]
    [SerializeField]
    private StageID m_stageID = StageID.Stage01;

    //===================================================
    //                  実行処理
    //===================================================
    public override void UpdateRewardChallenge()
    {
        var data = StageDataBaseManager.instance.GetStageData(m_stageID);
        if (data == null) return;

        // ステージをアンロック
        data.SetUnLock();
    }

}

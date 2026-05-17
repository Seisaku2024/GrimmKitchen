using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMoneyRewardChallenge : BaseRewardChallengeData
{
    // 制作者 田内
    // お金を追加するチャレンジ報酬

    [Header("追加する金額")]
    [SerializeField]
    [Min(1)]
    private int m_addMoney = 10000;

    //================================================
    //                  実行処理
    //================================================

    public override void UpdateRewardChallenge()
    {
        // 金額を追加
        ManagementDataManager.instance.TotalEarnedMoney += m_addMoney;
    }

}

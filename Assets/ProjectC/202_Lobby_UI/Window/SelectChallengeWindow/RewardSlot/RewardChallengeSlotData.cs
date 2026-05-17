using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(RewardChallengeDescription))]
public class RewardChallengeSlotData : MonoBehaviour
{
    // 制作者 田内
    // 報酬スロット

    //=======================
    // 説明文
    private RewardChallengeDescription m_rewardChallengeDescription = null;

    //=======================
    // 報酬データ

    private BaseRewardChallengeData m_rewardChallengeData = null;

    //==========================================================
    //                       実行処理
    //==========================================================

    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void SetData(BaseRewardChallengeData _data)
    {
        // データをセット
        m_rewardChallengeData = _data;

        // 説明文を更新
        if (m_rewardChallengeDescription == null) m_rewardChallengeDescription = gameObject.GetComponent<RewardChallengeDescription>();
        m_rewardChallengeDescription.UpdateDescription(m_rewardChallengeData);
    }


}

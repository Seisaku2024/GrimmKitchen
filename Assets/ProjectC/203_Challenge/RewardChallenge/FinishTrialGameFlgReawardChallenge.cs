using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrialGameFlgReawardChallenge : BaseRewardChallengeData
{
    public override void UpdateRewardChallenge()
    {
        // ToDo 体験版用（山本）
        // フラグオン
        var FinishFlg = StoryProgressManager.instance.
            GetStoryProgressData(StoryProgressType.FinishLastStoryCharenge);

        if(FinishFlg)
        {
            FinishFlg.SetFinish();
        }

    }
}

using AkazukinStageInfo;
using Cysharp.Threading.Tasks;
using HanselStageInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkazukinStageUpdate_CheckStoryProgress : BaseAkazukinStageStateUpdate
{
    [Header("各ストーリー進捗度に応じた変更先State")]
    [SerializeField]
    private SerializableDictionary<StoryProgressType, AkazukinStageState> m_nextGameStateList;

    public override UniTask OnInitialize()
    {
        if (StoryProgressManager.instance == null)
        {
            return base.OnInitialize();
        }

        // 赤ずきんステージのストーリーを終えたかどうか
        StoryProgressData data = StoryProgressManager.instance.
            GetStoryProgressData(StoryProgressType.CompleteAkazukinStory);

        if (data.AchieveFlg == false)
        {
            SetEnd(m_nextGameStateList[data.StoryProgressType]);
            return base.OnInitialize();
        }

        SetEnd(m_nextAkazukinStageState);

        return base.OnInitialize();
    }
}

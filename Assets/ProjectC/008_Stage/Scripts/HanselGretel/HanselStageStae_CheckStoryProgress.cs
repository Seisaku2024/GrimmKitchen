using Cysharp.Threading.Tasks;
using HanselStageInfo;
using LobbyStateInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselStageStae_CheckStoryProgress : BaseHanselStageStateUpdate
{
    [Header("各ストーリー進捗度に応じた変更先State")]
    [SerializeField]
    private SerializableDictionary<StoryProgressType, GameStageState> m_nextGameStateList;

    public override UniTask OnInitialize()
    {
        if (StoryProgressManager.instance == null)
        {
            return base.OnInitialize();
        }

        // ヘンゼルとグレーテルシーンのチュートリアルを終えたかどうか
        StoryProgressData data = StoryProgressManager.instance.
            GetStoryProgressData(StoryProgressType.CompleteHanselGretelTutrial);

        if(data.AchieveFlg==false)
        {
            SetEnd(m_nextGameStateList[data.StoryProgressType]);
            return base.OnInitialize();
        }

        SetEnd(m_nextHanselStageState);

        return base.OnInitialize();
    }

}

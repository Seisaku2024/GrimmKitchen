using Cysharp.Threading.Tasks;
using LobbyStateInfo;
using StageInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyStateUpdate_CheckProgressState : BaseLobbyStateUpdate
{
    [Header("各ストーリー進捗度に応じた変更先State")]
    [SerializeField]
    private SerializableDictionary<StoryProgressType, LobbyState> m_nextLobbyStateList;



    public override UniTask OnInitialize()
    {
        if (StoryProgressManager.instance == null)
        {
            return base.OnInitialize();
        }

        // 初めてロビーシーンに到達したらチュートリアルへ移行
        StoryProgressData data = StoryProgressManager.instance.
            GetStoryProgressData(StoryProgressType.FirstVisitLobbyScene);

        if (data.AchieveFlg == false)
        {
            SetEnd(m_nextLobbyStateList[data.StoryProgressType]);
            return base.OnInitialize();
        }

        // 経営に関するチュートリアルを終えたかどうか
        data = StoryProgressManager.instance.
            GetStoryProgressData(StoryProgressType.CompleteManagementTutrial);

        if (data.AchieveFlg == false)
        {
            SetEnd(m_nextLobbyStateList[data.StoryProgressType]);
            return base.OnInitialize();
        }

        //　赤ずきんステージをアンロックできたかどうか
        data = StoryProgressManager.instance.
            GetStoryProgressData(StoryProgressType.GetAkazukinBook);

        var stageData = StageDataBaseManager.instance.GetStageData(StageID.Stage02);

        // 赤ずきんステージがアンロックできてないなら
        if (data.AchieveFlg == false && stageData.ClearStageData.IsLock)
        {
            SetEnd(m_nextLobbyState);
            return base.OnInitialize();
        }


        // 赤ずきんステージがアンロックされたら
        if (data.AchieveFlg == false && (stageData.ClearStageData.IsLock == false))
        {
            data.SetFinish();
            SetEnd(m_nextLobbyStateList[data.StoryProgressType]);
            return base.OnInitialize();
        }


        // 最後のチャレンジを終えたかどうか
        data = StoryProgressManager.instance.
            GetStoryProgressData(StoryProgressType.FinishLastStoryCharenge);

        var finishData = StoryProgressManager.instance.GetStoryProgressData(StoryProgressType.FinishTrialGame);

        if (data.AchieveFlg == false)
        {
            SetEnd(m_nextLobbyState);
            return base.OnInitialize();
        }
        else if(data.AchieveFlg && (finishData.AchieveFlg==false))
        {
            SetEnd(m_nextLobbyStateList[data.StoryProgressType]);
            finishData.SetFinish();
            return base.OnInitialize();
        }

        SetEnd(m_nextLobbyState);

        return base.OnInitialize();
    }



}

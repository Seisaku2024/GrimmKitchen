using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;
using UniRx;
using IngredientInfo;



[RequireComponent(typeof(ChallengeSaveLoader))]
public class ChallengeManager : BaseManager<ChallengeManager>
{
    // 制作者 田内
    // チャレンジを管理するマネージャー


    //======================
    // 現在のチャレンジID


    private ReactiveProperty<ChallengeID> m_challengeIDRP = new(ChallengeID.None);

    public System.IObservable<ChallengeID> ChallengeIDRP
    {
        get { return m_challengeIDRP; }
    }

    public ChallengeID ChallengeID
    {
        get { return m_challengeIDRP.Value; }
    }

    //============================================
    //                 実行処理
    //============================================

    protected override void Load()
    {
        var saveLoad = ChallengeSaveLoader.Load();
        m_challengeIDRP.Value = saveLoad.ChallengeID;

        base.Load();
    }

    /// <summary>
    /// 選択中のチャレンジIDを更新
    /// </summary>
    public void SetChallengeID(ChallengeID _id)
    {
        m_challengeIDRP.Value = _id;
    }

}

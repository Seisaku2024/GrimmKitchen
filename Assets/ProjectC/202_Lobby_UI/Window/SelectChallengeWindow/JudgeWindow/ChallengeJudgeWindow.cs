using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;

public class ChallengeJudgeWindow : JudgeWindow
{
    // 制作者 田内
    // チャレンジに対してジャッジするウィンドウ

    [Header("チャレンジ説明文")]
    [SerializeField]
    private ChallengeDescription m_challengeDescription = null;

    private ChallengeData m_challengeData = null;

    //=======================================================
    //                  実行処理
    //=======================================================

    public void SetData(ChallengeID _id)
    {
        m_challengeData = ChallengeDataBaseManager.instance.GetData(_id);
    }

    public void SetData(ChallengeData _data)
    {
        m_challengeData = _data;
    }


    public override async UniTask OnInitialize()
    {

        #region nullチェック
        if(m_challengeDescription==null)
        {
            Debug.LogError("ChallengeDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            m_challengeDescription.UpdateDescription(m_challengeData);
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}

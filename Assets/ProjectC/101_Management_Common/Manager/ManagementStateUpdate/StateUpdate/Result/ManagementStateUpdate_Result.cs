using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;
using Cysharp.Threading.Tasks;

public class ManagementStateUpdate_Result : BaseManagementStateUpdate
{

    // 制作者 田内
    // 経営終了処理

    [Header("新規獲得アイテム確認ウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_windowController = null;


    // 作成したウィンドウコントローラー
    private WindowController m_createWindowController = null;

    private ChallengeID m_challengeID = ChallengeID.None;

    //====================================================
    //                   実行処理
    //====================================================


    public override async UniTask OnInitialize()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 一度プレイしたことを記憶
            var data = StoryProgressManager.instance.GetStoryProgressData(StoryProgressType.PlayManagement);
            if (data) data.SetFinish();

            // 経営データを更新
            ManagementGameDataManager.instance.SettingManagementData();

            // チャレンジを更新
            m_challengeID = ChallengeManager.instance.ChallengeID;
            ManagementGameDataManager.instance.ClearChallenge();

            await CreateToUpdateWindow();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }


    override public async UniTask OnExit()
    {

        DestoryWindowController();

        await UniTask.CompletedTask;
    }


    // ウィンドウを作成する
    private async UniTask CreateToUpdateWindow()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_windowController == null)
            {
                Debug.LogError("ウィンドウがシリアライズされていません");
                return;
            }

            m_createWindowController = Instantiate(m_windowController);

            // 処理を行う
            await m_createWindowController.CreateWindow<ManagementResultWindow>(onBeforeInitialize:async _=>
            {
                _.SetData(m_challengeID);
                await UniTask.CompletedTask;
            });
            cancelToken.ThrowIfCancellationRequested();

            // 削除
            DestoryWindowController();

            // 終了
            SetEnd(m_nextManagementState);

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    // ウィンドウを削除する
    private void DestoryWindowController()
    {
        // 削除
        if (m_createWindowController != null)
        {
            Destroy(m_createWindowController.gameObject);
        }
    }


}

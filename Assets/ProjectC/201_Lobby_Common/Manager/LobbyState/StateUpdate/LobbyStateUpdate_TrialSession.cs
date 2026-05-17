using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LobbyStateInfo;
using Cysharp.Threading.Tasks;

using PocketItemDataInfo;

public class LobbyStateUpdate_TrialSession : BaseLobbyStateUpdate
{
    // 体験会用の動作
    // 制作者　田内

    [Header("体験会ウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_trialSessionWindowController = null;

    [Header("試遊会初期化コントローラー")]
    [SerializeField]
    private TrialSessionController m_trialSessionController = null;


    // 作成したウィンドウ
    private WindowController m_createWindowController = null;


    //====================================================
    //                   実行処理
    //====================================================

    public override async UniTask OnInitialize()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ウィンドウを作成/処理
            await CreateWindow();
            cancelToken.ThrowIfCancellationRequested();

            await UniTask.CompletedTask;
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }


    override public async UniTask OnExit()
    {
        // 試遊会用初期化
        InitializeTrialSession();

        // ウィンドウを削除
        DestoryWindowController();

       
        await UniTask.CompletedTask;
    }


    // 経営ウィンドウを作成する
    private async UniTask CreateWindow()
    {
        #region nullチェック
        if (m_trialSessionWindowController == null)
        {
            Debug.LogError("体験会用sウィンドウコントローラーがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ウィンドウ作成
            m_createWindowController = Instantiate(m_trialSessionWindowController);

            // 処理を行う
            var window= await m_createWindowController.CreateWindow<BaseWindow>();
            cancelToken.ThrowIfCancellationRequested();

            // ウィンドウを削除
            DestoryWindowController();

            // 終了
            SetEnd(m_nextLobbyState);

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // 作成したウィンドウを削除する
    void DestoryWindowController()
    {
        // 削除
        if (m_createWindowController != null)
        {
            Destroy(m_createWindowController.gameObject);
        }
    }


    // 試遊会用初期化
    private void InitializeTrialSession()
    {
        #region nullチェック
        if (m_trialSessionController == null)
        {
            Debug.LogError("TrialSessionControllerがシリアライズされていません");
            return;
        }
        #endregion

        m_trialSessionController.InitializeTrialSession();
    }

}

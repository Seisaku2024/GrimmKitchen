using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using ChallengeInfo;
using NaughtyAttributes;

public class ManagementResultWindow : BaseWindow
{
    // 制作者 田内
    // 経営のリザルトウィンドウ

    [Header("UIコントローラー")]
    [SerializeField]
    private WindowUIController m_windowUIController = null;

    [Header("チャレンジ確認ウィンドウ")]
    [SerializeField]
    private WindowController m_challengeResultConfirmationWindowController = null;

    [BoxGroup("シーン")]
    [SerializeField]
    [Header("シーン遷移ボタン")]
    private InputActionButton m_sceneChangeButton = null;

    [BoxGroup("シーン")]
    [Header("シーン変更")]
    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;

    private ChallengeID m_challengeID = ChallengeID.None;

    //==============================================
    //              実行処理
    //==============================================

    public void SetData(ChallengeID _id)
    {
        m_challengeID = _id;
    }

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_windowUIController == null)
        {
            Debug.LogError("WindowUIController がシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            await m_windowUIController.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_windowUIController == null)
        {
            Debug.LogError("WindowUIController がシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await CreateChallengeResultConfirmationWindow();
            cancelToken.ThrowIfCancellationRequested();

            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                await m_windowUIController.OnUpdate();

                await m_windowUIController.OnLateUpdate();

                // 終了
                if (await Transition()) return;
                cancelToken.ThrowIfCancellationRequested();

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // チャレンジ確認ウィンドウを作成
    private async UniTask CreateChallengeResultConfirmationWindow()
    {
        // チャレンジが設定されていなければ
        if (m_challengeID == ChallengeID.None) return;
        if (m_challengeResultConfirmationWindowController == null) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 選択中のデータを取得
            var data = ChallengeDataBaseManager.instance.GetData(m_challengeID);


            var controller = Instantiate(m_challengeResultConfirmationWindowController);
            await controller.CreateWindow<ChallengeResultConfirmationWindow>(false, async _ =>
                {
                    _.SetData(data);
                    await UniTask.CompletedTask;
                });
            cancelToken.ThrowIfCancellationRequested();

            if (controller != null) Destroy(controller.gameObject);
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // シーン遷移を行う
    // 遷移できればtrueを返す
    private async UniTask<bool> Transition()
    {
        #region nullチェック
        if (m_sceneTransitionManager == null)
        {
            Debug.LogError("SceneTransitionManagerがシリアライズされていません");
            return false;
        }
        if (m_sceneChangeButton == null)
        {
            Debug.LogError("SceneChangeButtonがシリアライズされていません");
            return false;
        }
        #endregion

        if (m_sceneChangeButton.IsInputActionTrriger())
        {
            var cancelToken = this.GetCancellationTokenOnDestroy();
            try
            {
                await m_sceneTransitionManager.SceneChange();
                cancelToken.ThrowIfCancellationRequested();

                return true;
            }
            catch (System.OperationCanceledException ex)
            {
                Debug.Log(ex);
            }
        }

        return false;
    }

}

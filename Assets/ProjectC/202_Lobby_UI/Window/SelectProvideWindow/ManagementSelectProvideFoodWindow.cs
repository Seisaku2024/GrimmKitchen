using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class ManagementSelectProvideFoodWindow : BaseWindow
{
    // 制作者　田内
    // 提供する料理を選択する

    [Header("チュートリアルウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_tutorialWindowController = null;

    [Header("ウィンドウUIコントローラー")]
    [SerializeField]
    private WindowUIController m_windowUIController = null;

    [Header("スロット作成")]
    [SerializeField]
    private CreateProvideFoodSlotList m_createProvideFoodSlotList = null;

    [Header("説明文")]
    [SerializeField]
    private ChangeManagementProvideFoodDescription m_changeFoodItemDescription = null;

    //===================================================
    //                    実行処理
    //===================================================

    public override async UniTask OnInitialize()
    {
        #region nullチェック

        if (m_windowUIController == null)
        {
            Debug.LogError("WindowUIControllerがシリアライズされていません");
            return;
        }

        if (m_changeFoodItemDescription == null)
        {
            Debug.LogError("ChangeFoodItemDescriptionがシリアライズされていません");
            return;
        }

        if (m_createProvideFoodSlotList == null)
        {
            Debug.LogError("CreateProvideFoodSlotListがシリアライズされていません");
            return;
        }

        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 初期化
            await m_windowUIController.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // スロット作成
            _ = m_createProvideFoodSlotList.CreateSlot();

            // 説明文
            m_changeFoodItemDescription.OnInitialize();

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
            Debug.LogError("WindowUIコントローラーがシリアライズされていません");
            return;
        }

        if (m_changeFoodItemDescription == null)
        {
            Debug.LogError("ChangeFoodItemDescriptionがシリアライズされていません");
            return;
        }

        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // チュートリアルを表示
            await CreateTutorialWindow();
            cancelToken.ThrowIfCancellationRequested();

            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();


                // ウィンドウUI処理
                await m_windowUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // 詳細UI処理
                m_changeFoodItemDescription.OnUpdate();

                // ウィンドウUI後処理
                await m_windowUIController.OnLateUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // ウィンドウを閉じる
                if (IsClose())
                {
                    return;
                }

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    // チュートリアルウィンドウを表示
    private async UniTask CreateTutorialWindow()
    {
        #region nullチェック
        if (m_tutorialWindowController == null)
        {
            Debug.LogError("TutorialWindowControllerがシリアライズされていません");
            return;
        }
        #endregion

        // チュートリアルを表示
        var data = StoryProgressManager.instance.GetStoryProgressData(StoryProgressType.PlayManagement);
        if (data.AchieveFlg == false)
        {
            var controller = Instantiate(m_tutorialWindowController);
            await controller.CreateWindow<BaseWindow>();
            if (controller != null) Destroy(controller.gameObject);
        }

        await UniTask.CompletedTask;
    }

}

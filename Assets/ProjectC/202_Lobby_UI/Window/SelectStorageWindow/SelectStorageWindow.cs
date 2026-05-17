using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelectUIInfo;
using ButtonInfo;

public class SelectStorageWindow : BaseWindow
{
    // 制作者 田内
    // ストレージ選択ウィンドウ



    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("アイテム移動ウィンドウ")]
    [SerializeField]
    private WindowController m_moveItemWindowController = null;

    [Header("経営ストレージウィンドウ")]
    [SerializeField]
    private WindowController m_managementStorageWindowController = null;

    //========================================
    //              実行処理
    //========================================

    public override async UniTask OnInitialize()
    {

        #region nullチェック

        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }



    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択の更新
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // ボタンを選択
                await SelectButton();
                cancelToken.ThrowIfCancellationRequested();

                // 選択UIの後処理
                m_selectUIController.OnLateUpdate();

                // 閉じる
                if (IsClose()) return;
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


    private async UniTask SelectButton()
    {

        var cancelToken = this.destroyCancellationToken;

        try
        {
            // 選択したボタン
            var id = m_selectUIController.IsPressButton();

            switch (id)
            {
                // ストレージウィンドウ作成
                case ButtonID.MoveItem:
                    {
                        if (m_moveItemWindowController == null)
                        {
                            Debug.LogError("MoveItemWindowControllerがシリアライズされていません");
                            return;
                        }

                        var controller = Instantiate(m_moveItemWindowController);
                        await controller.CreateWindow<BaseWindow>();
                        cancelToken.ThrowIfCancellationRequested();
                        if (controller != null) Destroy(controller.gameObject);
                        break;
                    }

                // 経営ストレージウィンドウ作成
                case ButtonID.ManagementStorage:
                    {
                        if (m_managementStorageWindowController == null)
                        {
                            Debug.LogError("ManagementStorageWindowControllerがシリアライズされていません");
                            return;
                        }

                        var controller = Instantiate(m_managementStorageWindowController);
                        await controller.CreateWindow<BaseWindow>();
                        cancelToken.ThrowIfCancellationRequested();
                        if (controller != null) Destroy(controller.gameObject);
                        break;
                    }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }



}

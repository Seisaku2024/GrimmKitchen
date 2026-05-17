using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ButtonInfo;
using Cysharp.Threading.Tasks;

public class SelectStaffMenuWindow : BaseWindow
{
    // 経営シーンを操作する、移動するウィンドウ
    // 制作者 田内

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("ランダムスタッフウィンドウ")]
    [SerializeField]
    private WindowController m_randomStaffWindowController = null;

    [Header("スタッフストレージウィンドウ")]
    [SerializeField]
    private WindowController m_staffStorageWindowController = null;

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

                // 閉じる or 経営開始
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
                // 経営開始ウィンドウを作成
                case ButtonID.StaffStorage:
                    {
                        if (m_staffStorageWindowController == null)
                        {
                            Debug.LogError("StaffStorageWindowControllerがシリアライズされていません");
                            return;
                        }

                        var controller = Instantiate(m_staffStorageWindowController);
                        await controller.CreateWindow<BaseWindow>();
                        cancelToken.ThrowIfCancellationRequested();
                        if (controller != null) Destroy(controller.gameObject);
                        break;
                    }

                case ButtonID.RandomStaff:
                    {
                        if (m_randomStaffWindowController == null)
                        {
                            Debug.LogError("RandomStaffWindowControllerがシリアライズされていません");
                            return;
                        }

                        var controller = Instantiate(m_randomStaffWindowController);
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

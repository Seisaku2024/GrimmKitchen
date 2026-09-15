using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;
using ButtonInfo;

using Cysharp.Threading.Tasks;
using UniRx.Triggers;
using System;


public class SelectManagementWindow : BaseWindow
{
    // 経営シーンを操作する、移動するウィンドウ
    // 制作者 田内


    [Header("スタッフスロット作成（ホール）")]// (追加：吉田)
    [SerializeField]
    private CreateStaffPointSlotList m_createHallStaffPointSlotList = null;
    [Header("スタッフスロット作成（シェフ）")]// (追加：吉田)
    [SerializeField]
    private CreateStaffPointSlotList m_createChefStaffPointSlotList = null;


    [Header("提供料理スロット作成")]
    [SerializeField]
    private CreateProvideFoodSlotList m_createManagementProvideFoodSlotList = null;


    [Header("スロットコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    [Header("提供料理メニューウィンドウ")]
    [SerializeField]
    private WindowController m_provideFoodWindowController = null;


    [Header("スタッフ選択ウィンドウ")]
    [SerializeField]
    private WindowController m_selectStaffWindowController = null;


    [Header("チャレンジウィンドウ")]
    [SerializeField]
    private WindowController m_challengeWindowController = null;


    [Header("開始InputActionButton")]
    [SerializeField]
    private StartManagementControllerInputActionButton m_managementIbputActionButton = null;
    [Header("シーン切り替え")]
    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;


    [Header("経営スタートウィンドウ")]
    [SerializeField]
    private WindowController m_startManagementWindowController = null;


    // 追加（吉田）
    [Header("更新処理が必要なスクリプト")]
    [SerializeField]
    private List<WindowUpdateBase> m_windowUpdateBaseList = new();


    //========================================
    //              実行処理
    //========================================

    public override async UniTask OnInitialize()
    {

        #region nullチェック
        if (m_createHallStaffPointSlotList == null)
        {
            Debug.LogError("CreateHallStaffPointSlotListがシリアライズされていません");
            return;
        }
        if (m_createChefStaffPointSlotList == null)
        {
            Debug.LogError("CreateChefStaffPointSlotListがシリアライズされていません");
            return;
        }
        if (m_createManagementProvideFoodSlotList == null)
        {
            Debug.LogError("reateManagementProvideFoodSlotListがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();


            await m_createManagementProvideFoodSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            await m_createHallStaffPointSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            await m_createChefStaffPointSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 追加（吉田）
            foreach (var window in m_windowUpdateBaseList)
            {
                window.OnInitialize();
            }

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

                // 追加（吉田）
                foreach (var window in m_windowUpdateBaseList)
                {
                    window.OnUpdate();
                }

                // 選択UIの後処理
                m_selectUIController.OnLateUpdate();

                // 閉じる or 経営開始
                if (IsClose() || await StartManagement()) return;
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
                case ButtonID.Start:
                    {
                        /* StartManagementInputActionButtonからシーン変更が行われるため、不要
                         
                        if (m_startManagementWindowController == null)
                        {
                            Debug.LogError("StartManagementWindowControllerがシリアライズされていません");
                            return;
                        }

                        var controller = Instantiate(m_startManagementWindowController);
                        await controller.CreateWindow<BaseWindow>();
                        cancelToken.ThrowIfCancellationRequested();
                        if (controller != null) Destroy(controller.gameObject);
                        */
                        break;
                    }

                // スタッフ選択ウィンドウを作成
                case ButtonID.SelectStaff:
                    {
                        if (m_selectStaffWindowController == null)
                        {
                            Debug.LogError("SelectStaffWindowControllerがシリアライズされていません");
                            return;
                        }

                        await OpenChildWindow(m_selectStaffWindowController);
                        cancelToken.ThrowIfCancellationRequested();
                        break;
                    }
                // 提供料理選択ウィンドウを作成
                case ButtonID.SelectProvideFood:
                    {
                        if (m_provideFoodWindowController == null)
                        {
                            Debug.LogError("ProvideFoodWindowControllerがシリアライズされていません");
                            return;
                        }

                        await OpenChildWindow(m_provideFoodWindowController);
                        cancelToken.ThrowIfCancellationRequested();
                        break;
                    }
                case ButtonID.Challenge:
                    {
                        if (m_challengeWindowController==null)
                        {
                            Debug.LogError("ChallengeWindowControllerがシリアライズされていません");
                            return;
                        }

                        await OpenChildWindow(m_challengeWindowController);
                        cancelToken.ThrowIfCancellationRequested();

                        break;
                    }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    private async UniTask OpenChildWindow(WindowController prefab)
    {
        var controller = Instantiate(prefab);
        try
        {
            await controller.CreateWindow<BaseWindow>();
        }
        finally
        {
            // 子を閉じた直後も、経営開始ウィンドウが残っていれば操作停止を継続する。
            if (this != null && !destroyCancellationToken.IsCancellationRequested &&
                PlayerInputManager.instance != null)
            {
                PlayerInputManager.instance.SetGameplayInputActive(false);
            }

            if (controller != null) Destroy(controller.gameObject);
        }
    }

    private async UniTask<bool> StartManagement()
    {
        #region nullチェック
        if (m_managementIbputActionButton == null)
        {
            Debug.LogError("ManagementIbputActionButtonがシリアライズされていません");
            return false;
        }
        #endregion

        if (m_managementIbputActionButton.IsInputActionTrriger())
        {
            // スタッフに給料を支払う
            ManagementDataManager.instance.TotalEarnedMoney -= StaffManager.instance.GetTotalSalaryPrice();

            // シーン遷移
            await m_sceneTransitionManager.SceneChange();
            return true;
        }


        await UniTask.CompletedTask;
        return false;
    }


}

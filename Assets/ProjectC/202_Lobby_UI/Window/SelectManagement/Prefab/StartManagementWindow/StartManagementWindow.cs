using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManagementWindow : BaseWindow
{
    // 制作者 田内
    // 経営開始選択ウィンドウ


 

    [Header("経営説明文")]
    [SerializeField]
    private ChangeStartManagementDescription m_changeStartManagementDescription = null;

    [Header("開始InputActionButton")]
    [SerializeField]
    private StartManagementControllerInputActionButton m_managementIbputActionButton = null;


    [Header("シーン切り替え")]
    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;

    //==========================================
    //              実行処理
    //==========================================


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if(m_changeStartManagementDescription==null)
        {
            Debug.LogError("ChangeStartManagementDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 説明文更新
            m_changeStartManagementDescription.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }




    public override async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                await Start();

                // 閉じる
                if (IsClose()) return;

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    private async UniTask Start()
    {
        #region nullチェック
        if(m_managementIbputActionButton==null)
        {
            Debug.LogError("ManagementIbputActionButtonがシリアライズされていません");
            return;
        }
        #endregion

        if (m_managementIbputActionButton.IsInputActionTrriger())
        {
            var cancelToken = this.destroyCancellationToken;

            try
            {
                // スタッフに給料を支払う
                ManagementDataManager.instance.TotalEarnedMoney -= StaffManager.instance.GetTotalSalaryPrice();

                // シーン遷移
                _ = m_sceneTransitionManager.SceneChange();
            }
            catch (System.OperationCanceledException ex)
            {
                Debug.Log(ex);
            }
        }

        await UniTask.CompletedTask;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ButtonInfo;
using Cysharp.Threading.Tasks;

public class StaffStatusUpController : MonoBehaviour
{
    // 制作者 田内
    // ステータスアップコントローラー

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("ジャッジウィンドウ")]
    [SerializeField]
    private WindowController m_judgeWindowController = null;

    [Header("確認ウィンドウ")]
    [SerializeField]
    private WindowController m_confirmationWindowController = null;

    // ステータスを上げるスタッフデータ
    private StaffStatusData m_staffStatusData = null;

    //=================================
    //           実行処理
    //=================================

    public void SetData(StaffStatusData _data)
    {
        m_staffStatusData = _data;
    }


    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize()
    {
    }


    /// <summary>
    /// 実行処理
    /// </summary>
    public async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await Set();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    // セットする
    private async UniTask Set()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        if (m_selectUIController.IsPress == false) return;
        if (m_selectUIController.CurrentSelectUI == null) return;

        var data = m_selectUIController.CurrentSelectUI.GetComponent<StaffStatusUpSlotData>();
        if (data == null) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // 選択ウィンドウ作成
            if (await CreateJudgeWindow(data.StaffStatusUpData) == false) return;
            cancelToken.ThrowIfCancellationRequested();

            // 強化前データ
            StaffStatusData beforeData = new(m_staffStatusData);

            // 強化
            m_staffStatusData.StatusUp(data.StaffStatusUpData);

            // 確認ウィンドウ作成
            await CreateConfirmationWindow(beforeData, m_staffStatusData);
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }



    // ジャッジウィンドウ
    private async UniTask<bool> CreateJudgeWindow(StaffStatusUpData _data)
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        bool judge = false;
        try
        {
            var controller = Instantiate(m_judgeWindowController);
            var window = await controller.CreateWindow<JudgeStaffStatusUpWindow>(_bSelef: true, onBeforeInitialize: async _ =>
            {
                // データセット
                _.SetData(_data);

                await UniTask.CompletedTask;
            });
            cancelToken.ThrowIfCancellationRequested();

            judge = await window.OnSelfUpdate();
            cancelToken.ThrowIfCancellationRequested();

            await window.OnClose();
            cancelToken.ThrowIfCancellationRequested();

            await window.OnDestroy();
            cancelToken.ThrowIfCancellationRequested();

            if (controller != null) Destroy(controller.gameObject);
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        return judge;
    }


    // 確認ウィンドウ作成
    private async UniTask CreateConfirmationWindow(StaffStatusData _before, StaffStatusData _after)
    {
        if (m_confirmationWindowController == null)
        {
            return;
        }

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            var controller = Instantiate(m_confirmationWindowController);
            await controller.CreateWindow<ConfirmationStaffStatusUpWindow>(onBeforeInitialize: async _ =>
             {
                 _.SetData(_before, _after);
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

}

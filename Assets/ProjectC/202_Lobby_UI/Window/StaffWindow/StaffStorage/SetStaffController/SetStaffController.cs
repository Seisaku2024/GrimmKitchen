using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class SetStaffController : MonoBehaviour
{
    // 制作者 田内
    // 選択中のStaffPointのStaffStatusを選択中のStaffStatusと入れ替えるコントローラー


    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スタッフストレージWindowController")]
    [SerializeField]
    private WindowController m_staffStorageWindowController = null;

    //==============================================
    //                実行処理
    //==============================================


    /// <summary>
    /// 実行処理
    /// </summary>
    public async UniTask OnUpdate()
    {
        var cancelToken = this.destroyCancellationToken;

        try
        {
            // ウィンドウ作成
            await CreateStaffStorageWindow();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    /// <summary>
    /// 後実行処理
    /// </summary>
    public void OnLateUpdate()
    {

    }



    /// <summary>
    /// 入れ替え用にスタッフストレージウィンドウを作成する
    /// </summary>
    private async UniTask CreateStaffStorageWindow()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        if (m_staffStorageWindowController == null)
        {
            Debug.LogError("StaffStorageWindowControllerがシリアライズされていません");
            return;
        }
        #endregion

        // 選択されれば
        if (m_selectUIController.IsPress == false) return;

        // 選択中のUI
        var currentSelectUI = m_selectUIController.CurrentSelectUI;
        if (currentSelectUI == null) return;

        // StaffPointSlotDataであるときのみ処理を実行する
        if (currentSelectUI.TryGetComponent<StaffPointSlotData>(out var slotData) == false) return;


        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // ウィンドウを作成
            var controller = Instantiate(m_staffStorageWindowController);

            await UniTask.NextFrame();
            cancelToken.ThrowIfCancellationRequested();

            // 作成・初期化・表示まで
            var createWindow = await controller.CreateWindow<StaffStorageWindow>(_bSelef: true, onBeforeInitialize: async _ =>
                  {
                      // 初期データをセット
                      _.SetData(slotData.StaffStatusData);
                      await UniTask.CompletedTask;
                  });
            cancelToken.ThrowIfCancellationRequested();

            // 実行処理・選択したスタッフステータスを取得
            StaffStatusData selectStaffStatus = await createWindow.OnUpdateStaffStatus();
            cancelToken.ThrowIfCancellationRequested();

            // セットする
            StaffManager.instance.SetStaffPointStaffStatus(slotData.StaffPointData, selectStaffStatus);

            // 閉じる・削除
            await createWindow.OnClose();
            cancelToken.ThrowIfCancellationRequested();

            await createWindow.OnDestroy();
            cancelToken.ThrowIfCancellationRequested();

            // コントローラーも削除
            if (controller != null) Destroy(controller.gameObject);


        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        return;
    }

}

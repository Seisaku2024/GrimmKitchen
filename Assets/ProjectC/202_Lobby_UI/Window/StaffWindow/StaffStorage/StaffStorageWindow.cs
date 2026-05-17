using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffStorageWindow : BaseWindow
{
    // 制作者 田内
    // スタッフをセットするウィンドウ

    [Header("スクロールビュー")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("スタッフストレージスロット作成")]
    [SerializeField]
    private CreateStaffStorageSlotList m_createStaffStorageSlotList = null;

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スタッフ説明文変更")]
    [SerializeField]
    private ChangeStaffStatusDataDescription m_changeStaffStatusDataDescription = null;

    [Header("スタッフ使用用途選択コントローラー")]
    [SerializeField]
    private SelectUseStaffController m_slectUseStaffController = null;

    [Header("スタッフ整頓コントローラー")]
    [SerializeField]
    private TidyingStaffController m_tidyingStaffController = null;

    private StaffStatusData m_selectStaffStatusData = null;

    //========================================
    //              実行処理
    //========================================

    public void SetData(StaffStatusData _data)
    {
        m_selectStaffStatusData = _data;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_createStaffStorageSlotList == null)
        {
            Debug.LogError("CreateStaffStorageSlotListがシリアライズされていません");
            return;
        }

        if (m_changeStaffStatusDataDescription == null)
        {
            Debug.LogError("ChangeStaffStatusDataDescriptionがシリアライズされていません");
            return;
        }
        #endregion


        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // スタッフストレージスロットを作成
            await m_createStaffStorageSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 説明文更新
            m_changeStaffStatusDataDescription.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    /// <summary>
    /// Updateの種類が多いのでひとまとまりにしています
    /// OnUpdateなどの処理に変更を加えたい場合はここを変更してください
    /// </summary>
    private async UniTask<StaffStatusData> ExecuteUpdate(bool _checkStaffStatus)
    {
        #region nullチェック

        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPositionがシリアライズされていません");
            return null;
        }

        if (m_changeStaffStatusDataDescription == null)
        {
            Debug.LogError("ChangeStaffStatusDataDescriptionがシリアライズされていません");
            return null;
        }

        if (m_createStaffStorageSlotList == null)
        {
            Debug.LogError("CreateStaffStorageSlotListがシリアライズされていません");
            return null;
        }

        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return null;
        }

        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();


                // UI選択コントローラーの実行処理
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // 使用用途
                if (m_slectUseStaffController != null)
                {
                    await m_slectUseStaffController.OnUpdate();
                    cancelToken.ThrowIfCancellationRequested();
                }

                // 選択
                if (m_tidyingStaffController != null)
                {
                    await m_tidyingStaffController.OnUpdate();
                    cancelToken.ThrowIfCancellationRequested();
                }

                // 説明文の更新
                m_changeStaffStatusDataDescription.OnUpdate();


                // 選択されれば返信し終了
                if (_checkStaffStatus)
                {
                    if (SelectStaffStatus() == true)
                    {
                        return m_selectStaffStatusData;
                    }
                }

                // スクロールビューの座標更新
                m_changeScrollViewPosition.OnUpdate();


                // UI選択コントローラーの後実行処理
                m_selectUIController.OnLateUpdate();


                // ウィンドウを閉じる
                if (IsClose()) return m_selectStaffStatusData;


                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        return null;
    }





    /// <summary>
    /// 実行処理
    /// </summary>
    public override async UniTask OnUpdate()
    {
        var cancelToken = this.destroyCancellationToken;
        try
        {
            await ExecuteUpdate(false);
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }



    /// <summary>
    /// SelectUIControllerで選択したステータスを返す
    /// そのほかは通常の処理
    /// </summary>
    public async UniTask<StaffStatusData> OnUpdateStaffStatus()
    {
        var cancelToken = this.destroyCancellationToken;
        try
        {
            var data = await ExecuteUpdate(true);
            cancelToken.ThrowIfCancellationRequested();

            return data;
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        return null;
    }


    private bool SelectStaffStatus()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return false;
        }
        #endregion

        // 選択されたかどうか
        if (m_selectUIController.IsPress == false) return false;

        // 選択中のUIがあるかどうか
        var currentSelectUI = m_selectUIController.CurrentSelectUI;
        if (currentSelectUI == null) return false;

        // スタッフステータスのスリットデータかどうか
        if (currentSelectUI.TryGetComponent<StaffStatusSlotData>(out var slotData) == false) return false;

        // 選択スタッフデータをセット
        m_selectStaffStatusData = slotData.StaffStatusData;
        return true;

    }
}

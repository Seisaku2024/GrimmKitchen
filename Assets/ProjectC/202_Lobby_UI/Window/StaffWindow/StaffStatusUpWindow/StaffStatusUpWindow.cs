using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class StaffStatusUpWindow : BaseWindow
{
    // 制作者 田内
    // スタッフを強化するウィンドウ

    [Header("ステータスアップコントローラー")]
    [SerializeField]
    private StaffStatusUpController m_staffStatusUpController = null;

    [Header("説明文")]
    [SerializeField]
    private StaffStatusDataDescription m_staffStatusDataDescription = null;

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スロット作成")]
    [SerializeField]
    private CreateStaffStatusUpSlotList m_createStaffStatusUpSlotList = null;

    [Header("説明文(スタッフステータスアップ)")]
    [SerializeField]
    private ChangeStaffStatusUpDescription m_changeStaffStatusUpDescription = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    // スタッフデータ
    private StaffStatusData m_staffStatusData = null;

    //===========================================================
    //                      実行処理
    //===========================================================

    public void SetData(StaffStatusData _data)
    {
        #region nullチェック
        if (m_staffStatusUpController == null)
        {
            Debug.LogError("StaffStatusUpControllerがシリアライズされていません");
            return;
        }
        if (m_staffStatusDataDescription == null)
        {
            Debug.LogError("StaffStatusDataDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        m_staffStatusData = _data;

        m_staffStatusUpController.SetData(_data);
        m_staffStatusDataDescription.UpdateDescription(_data);
    }

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_staffStatusUpController == null)
        {
            Debug.LogError("StaffStatusUpControllerがシリアライズされていません");
            return;
        }
        if (m_createStaffStatusUpSlotList == null)
        {
            Debug.LogError("CreateStaffStatusUpSlotListがシリアライズされていません");
            return;
        }
        if (m_changeStaffStatusUpDescription == null)
        {
            Debug.LogError("m_changeStaffStatusUpDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            await m_createStaffStatusUpSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            m_changeStaffStatusUpDescription.OnInitialize();

            m_staffStatusUpController.OnInitialize();


        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPositionがシリアライズされていません");
            return;
        }
        if (m_staffStatusUpController == null)
        {
            Debug.LogError("StaffStatusUpControllerがシリアライズされていません");
            return;
        }
        if (m_changeStaffStatusUpDescription == null)
        {
            Debug.LogError("m_changeStaffStatusUpDescriptionがシリアライズされていません");
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

                // UI選択コントローラーを更新
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // 座標変更
                m_changeScrollViewPosition.OnUpdate();


                m_changeStaffStatusUpDescription.OnUpdate();

                // ステータスアップ
                await m_staffStatusUpController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択コントローラーを後更新
                m_selectUIController.OnLateUpdate();

                // ウィンドウを閉じる
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


}

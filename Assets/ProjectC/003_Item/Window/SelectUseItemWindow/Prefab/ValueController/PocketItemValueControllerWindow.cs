using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using Cysharp.Threading.Tasks;
using TMPro;

public class PocketItemValueControllerWindow : BaseWindow
{
    // 制作者 田内
    // 値を操作するコントローラーウィンドウ

    [Header("値コントローラー")]
    [SerializeField]
    private PocketItemValueController m_pocketItemValueController = null;

    [Header("説明文")]
    [SerializeField]
    private ChangeValueControllerDescription m_changeValueControllerDescription = null;


    //=====================================
    // ターゲットアイテムデータ

    private PocketType m_pocketType = PocketType.Inventory;
    private PocketItemData m_pocketItemData = null;

    //========================================
    //              実行処理
    //========================================

    /// <summary>
    /// ターゲットのアイテムデータをセット
    /// </summary>
    public void SetPocketItemData(PocketType _type, PocketItemData _data)
    {
        m_pocketType = _type;
        m_pocketItemData = _data;
    }


    public int GetNum()
    {
        if (m_pocketItemValueController == null) return 0;
        return m_pocketItemValueController.CurrentValue;
    }


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_changeValueControllerDescription == null)
        {
            Debug.LogError("ChangeValueControllerDescriptionがシリアライズされていません");
            return;
        }
        if (m_pocketItemValueController == null)
        {
            Debug.LogError("PocketItemValueControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // コントローラー更新
            m_pocketItemValueController.SetData(m_pocketType, m_pocketItemData);

            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            m_changeValueControllerDescription.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    override public async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_pocketItemValueController == null)
        {
            Debug.LogError("PocketItemValueControllerがシリアライズされていません");
            return;
        }
        if (m_changeValueControllerDescription == null)
        {
            Debug.LogError("ChangeValueControllerDescriptionがシリアライズされていません");
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

                // ランダムスタッフコントローラーを更新
                await m_pocketItemValueController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                m_changeValueControllerDescription.OnUpdate();

                // 値コントローラーを更新
                m_pocketItemValueController.OnLateUpdate();

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

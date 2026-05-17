using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

public class CheckNewItemWindow : BaseWindow
{
    // ポケットアイテムを表示するウィンドウ
    // 制作者　田内

    [Header("スロット作成")]
    [SerializeField]
    protected CreatePocketItemSlotList m_createItemSlotList = null;

    [Header("説明欄")]
    [SerializeField]
    protected ChangeItemDescription m_changeItemSlotDescription = null;

    [Header("スロットコントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;

    [Header("スクロール")]
    [SerializeField]
    protected ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("移動アイテムコントローラー")]
    [SerializeField]
    protected SelectKeepMoveNewItemController m_selectKeepPocketItemController = null;

    //============================================
    //              実行処理
    //============================================

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_createItemSlotList == null)
        {
            Debug.LogError("CreateItemSlotListコンポーネントがアタッチされていません");
            return;
        }

        if (m_changeItemSlotDescription == null)
        {
            Debug.LogError("ChangeItemSlotDescriotionコンポーネントがアタッチされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();


            // スロット作成
            await m_createItemSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 選択コントローラー初期化
            await m_selectKeepPocketItemController.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            await UniTask.DelayFrame(1);
            cancelToken.ThrowIfCancellationRequested();

            // 説明分の初期化
            m_changeItemSlotDescription.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    public override async UniTask OnUpdate()
    {
        #region nullチェック
        // Nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SlotContorollerコンポーネントがアタッチされていません");
            return;
        }
        if (m_changeItemSlotDescription == null)
        {
            Debug.LogError("ChangeItemSlotDescriptionコンポーネントがアタッチされていません");
            return;
        }
        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPositionがシリアライズされていません");
            return;
        }
        if (m_selectKeepPocketItemController == null)
        {
            Debug.LogError("SelectKeepPocketItemControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択の更新
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // スクロールの更新
                m_changeScrollViewPosition.OnUpdate();

                // 説明文の更新
                m_changeItemSlotDescription.OnUpdate();

                // キープ
                await m_selectKeepPocketItemController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択の後処理
                m_selectUIController.OnLateUpdate();

                // 閉じる
                if (IsClose())
                {
                    // 移動を行う
                    bool isEnd = await m_selectKeepPocketItemController.Move();
                    cancelToken.ThrowIfCancellationRequested();

                    if (isEnd) return;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MoveItemWindowUI : BaseWindowUI
{
    // 制作者 田内
    // アイテム移動用UI

    [Header("スロット作成")]
    [SerializeField]
    private CreatePocketItemSlotList m_createItemSlotList = null;

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("アイテム使用用途選択コントローラー")]
    [SerializeField]
    private SelectUseItemController m_selectUseItemController = null;

    [Header("整頓コントローラー")]
    [SerializeField]
    private TidyingPocletItemController m_tidyingPocletItemController = null;

    [Header("移動コントローラー")]
    [SerializeField]
    private ShortCutMovePocketItemController m_shortCutMovePocketItemController = null;

    //=========================================
    //              実行処理
    //=========================================

    override public async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_createItemSlotList == null)
        {
            Debug.Log("CreateItemSlotListがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await m_createItemSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }



    public override async UniTask OnSelectUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("選択コントローラーがシリアライズされていません");
            return;
        }
        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPositionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {

            await m_selectUIController.OnUpdate();
            cancelToken.ThrowIfCancellationRequested();

            m_changeScrollViewPosition.OnUpdate();

            if (m_tidyingPocletItemController != null)
            {
                await m_tidyingPocletItemController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();
            }

            if (m_selectUseItemController != null)
            {
                await m_selectUseItemController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();
            }

            if (m_shortCutMovePocketItemController != null)
            {
                await m_shortCutMovePocketItemController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();
            }

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
        await UniTask.CompletedTask;
    }



    override public async UniTask OnLateUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("選択コントローラーがシリアライズされていません");
            return;
        }
        #endregion
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 後処理
            m_selectUIController.OnLateUpdate();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
        await UniTask.CompletedTask;
    }


}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using SelectUseItemInfo;


public class InventoryWindow : BasePocketItemControllerWindow
{
    // 制作者 よしだ
    // インベントリ


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
            //await base.OnInitialize();
            //cancelToken.ThrowIfCancellationRequested();


            // スロット作成
            await m_createItemSlotList.OnInitialize();
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
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {
                // UI選択の更新
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // スクロールの更新
                m_changeScrollViewPosition.OnUpdate();

                // 説明文の更新
                m_changeItemSlotDescription.OnUpdate();

                // アイテム使用用途選択ウィンドウを作成する
                if (m_selectUseItemController != null)
                {
                    await m_selectUseItemController.OnUpdate();
                    cancelToken.ThrowIfCancellationRequested();
                }

                // 整頓
                if (m_tidyingPocletItemController != null)
                {
                    await m_tidyingPocletItemController.OnUpdate();
                    cancelToken.ThrowIfCancellationRequested();
                }

                // UI選択の後処理
                m_selectUIController.OnLateUpdate();

                return;
                //// 閉じる
                //if (IsClose()) return;

                //await UniTask.DelayFrame(1);
                //cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SelectUseItemInfo;
using PocketItemDataInfo;
using ItemInfo;

public class SelectUseItemController : MonoBehaviour
{
    // 制作者 田内
    // アイテムの使い道を決めるコントローラー

    [System.Serializable]
    private class CreatePocketItemValueControllerData
    {
        public SelectUseItemID SelectUseItemID;

        public WindowController WindowControlle;
    }

    [Header("SelectUIController")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("SelectUseItemWindowコントローラー")]
    [SerializeField]
    private WindowController m_selectUseItemWindowController = null;

    [Header("表示リスト")]
    [SerializeField]
    private List<SelectUseItemID> m_selectUseItemIDList = new();

    // ポケットアイテム数
    [Header("アイテムウィンドウ")]
    [SerializeField]
    private List<CreatePocketItemValueControllerData> m_createPocketItemValueControllerDataList = new();


    //===============================================
    //                  実行処理
    //===============================================

    public async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await SelectUI();
            cancelToken.ThrowIfCancellationRequested();


        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    private async UniTask SelectUI()
    {
        #region nullチェック
        if (m_selectUseItemWindowController == null)
        {
            Debug.LogError("SelectUseItemWindowControllerがシリアライズされていません");
            return;
        }
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーが登録されていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 選択したか ・ 選択しているUIがなければ
            if (m_selectUIController.IsPress == false) return;
            if (m_selectUIController.CurrentSelectUI == null) return;

            // 選択しているUIがアイテムスロットであれば
            var slotData = m_selectUIController.CurrentSelectUI.GetComponent<PocketItemSlotData>();
            if (slotData == null || slotData.PocketItemData == null) return;

            // ウィンドウを作成/動作
            var controller = Instantiate(m_selectUseItemWindowController);
            var window = await controller.CreateWindow<SelectUseItemWindow>(true, async _ =>
             {
                 if (slotData.ItemData != null)
                 {
                     _.SetData(m_selectUseItemIDList, slotData.ItemData.ItemTypeID, slotData.ItemData.ItemID);
                 }
                 await UniTask.CompletedTask;
             });
            cancelToken.ThrowIfCancellationRequested();

            // 処理
            SelectUseItemID selectUseItemID = await window.OnUpdate();
            cancelToken.ThrowIfCancellationRequested();

            // 閉じる
            await window.OnClose();
            cancelToken.ThrowIfCancellationRequested();

            // 削除
            await window.OnDestroy();
            cancelToken.ThrowIfCancellationRequested();

            // ウィンドウコントローラーを削除
            if (controller != null) Destroy(controller.gameObject);

            // 使用用途で更新
            await Use(selectUseItemID, slotData);
            cancelToken.ThrowIfCancellationRequested();

            return;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
            return;
        }
    }


    private async UniTask Use(SelectUseItemID _id, PocketItemSlotData _slotData)
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // 動作を決める
            switch (_id)
            {
                case SelectUseItemID.Eat:
                    {
                        var useItemInstance = gameObject.AddComponent<UseItemInstance>();
                        useItemInstance.SetItemData(_slotData.PocketType, _slotData.PocketItemData.ItemTypeID, _slotData.PocketItemData.ItemID);
                        useItemInstance.UseItem(_slotData.PocketType, _slotData.PocketItemData);
                        Destroy(useItemInstance);

                        break;
                    }

                case SelectUseItemID.Dispose:
                    {
                        await CreateWindow(SelectUseItemID.Dispose, _slotData.PocketType, _slotData.PocketItemData);
                        break;
                    }

                case SelectUseItemID.MoveInventory:
                    {
                        await CreateWindow(SelectUseItemID.MoveInventory, _slotData.PocketType, _slotData.PocketItemData);
                        break;
                    }

                case SelectUseItemID.MoveManagementStorage:
                    {
                        await CreateWindow(SelectUseItemID.MoveManagementStorage, _slotData.PocketType, _slotData.PocketItemData);
                        break;
                    }

                case SelectUseItemID.Exit:
                    {
                        // 何もせず戻る
                        break;
                    }

                default:
                    {
                        Debug.Log("IDが当てはまりませんでした");
                        break;
                    }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    private async UniTask CreateWindow(SelectUseItemID _id, PocketType _pocketType, PocketItemData _data)
    {
        foreach (var data in m_createPocketItemValueControllerDataList)
        {
            if (data.WindowControlle == null)
            {
                Debug.LogError("WindowControllerがシリアライズされていません");
                continue;
            }

            // IDが一致すれば
            if (data.SelectUseItemID == _id)
            {
                var cancelToken = this.GetCancellationTokenOnDestroy();
                try
                {
                    var controller = Instantiate(data.WindowControlle);
                    PocketItemValueControllerWindow window = await controller.CreateWindow<PocketItemValueControllerWindow>(onBeforeInitialize: async _ =>
                    {
                        _.SetPocketItemData(_pocketType, _data);
                        await UniTask.CompletedTask;
                    });

                    if (controller != null) Destroy(controller.gameObject);
                }
                catch (System.OperationCanceledException ex)
                {
                    Debug.Log(ex);
                }
            }
        }
    }


}

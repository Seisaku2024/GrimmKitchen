using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using Cysharp.Threading.Tasks;
using UniRx;

public class CreateManagementProvideFoodDataSlotList : BaseCreateSlotList
{
    // 制作者　田内
    // 提供料理のスロットを作成する
    // マネージャーの提供料理リストに変更が加わるたびに自動的に更新

    [Tooltip("経営ゲーム用、提供可能数や売り上げ数などを更新する")]

    //====================================================
    //                  実行処理
    //====================================================

    private void Start()
    {
        ManagementGameDataManager.instance.ProvideFoodDataRPRC.ObserveCountChanged().Subscribe(_ =>
            {
                CreateSlot();
            }).AddTo(this);
    }


    override protected async UniTask CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        DestroySlotList();

        // 提供料理スロット作成
        foreach (var data in ManagementGameDataManager.instance.ProvideFoodDataRPRC)
        {
            if (data == null || data.Value == null) continue;

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<ManagementProvideFoodDataSlotData>(out var slotData))
            {
                // スロットのデータをセット
                var itemData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Food, (uint)data.Value.FoodID);
                slotData.SetProvideFoodData(data.Value);
                slotData.SetItemSlotData(itemData, ProvideFoodManager.instance.PocketType);
            }
            else
            {
                Debug.LogError("ItemSlotDataコンポーネントがアタッチされていません");
            }

            // リストに追加
            m_slotList.Add(slot);

            // UIcontrollerに追加
            AddSelectUIControler(slot);
        }

        await UniTask.CompletedTask;
    }
}

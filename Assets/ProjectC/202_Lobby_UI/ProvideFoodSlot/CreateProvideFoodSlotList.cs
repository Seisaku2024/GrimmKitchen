using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using Cysharp.Threading.Tasks;
using UniRx;

public class CreateProvideFoodSlotList : BaseCreateSlotList
{
    // 制作者　田内
    // 提供料理のスロットを作成する

    [Tooltip("確認用")]

    //====================================================
    //                  実行処理
    //====================================================

    private void Start()
    {
        // 提供料理リスト変更を受信
        ProvideFoodManager.instance.ProvideFoodIDRC.ObserveCountChanged().Subscribe(_ =>
        {
            // スロットを再度作成
            CreateSlot();

        }).AddTo(this);
    }


    override protected async UniTask CreateSlotInstance()
    {

        DestroySlotList();

        // 提供料理スロット作成
        foreach (var id in ProvideFoodManager.instance.ProvideFoodIDList)
        {
            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<ItemSlotData>(out var slotData))
            {
                // スロットのデータをセット
                var itemData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Food, (uint)id);
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

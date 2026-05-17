using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PocketItemDataInfo;
using ItemInfo;
using UniRx;
using Cysharp.Threading.Tasks;

public class CreatePocketItemSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // 対応ポケットの所持アイテムスロットを作成する

    protected enum SlotListType
    {
        MaxSlotDisplay, // スロットを最大まで作成する
        MinSlotDisplay, // スロットを最少で作成する
    }

    [Header("アイテムの種類")]
    [SerializeField]
    protected ItemTypeID m_itemTypeID = ItemTypeID.ALL;

    [Header("スロットリストの種類")]
    [SerializeField]
    protected SlotListType m_slotListType = SlotListType.MaxSlotDisplay;

    [Header("ポケットマネージャーの種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;

    //====================================================
    //                  実行処理
    //====================================================

    private void Start()
    {
        m_pocketType.GetPocketItemDataManager().ItemDataRC.ObserveAdd().Subscribe(_ =>
        {
            SetSlotData(_.Value);

        }).AddTo(this);
    }


    // スロットを作成する
    override protected async UniTask CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        // 現在所持しているアイテムリスト
        var itemList = m_pocketType.GetPocketItemDataManager().GetItemList(m_itemTypeID);

        // 作成するスロットサイズ
        int size = GetSlotSize(itemList);

        // スロット作成
        for (int i = 0; i < size; ++i)
        {
            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<PocketItemSlotData>(out var slotData))
            {
                if (i < itemList.Count)
                {
                    // アイテムスロットのデータをセット
                    var itemData = ItemDataBaseManager.instance.GetItemData(itemList[i].ItemTypeID, itemList[i].ItemID);
                    slotData.SetPocketItemData(itemList[i]);
                    slotData.SetItemSlotData(itemData, m_pocketType);

                }
                else
                {
                    // スロットのデータを初期化し、コンポーネントを削除
                    slotData.InitializeSlotData();
                }
            }
            else
            {
                Debug.LogError("PocketItemSlotDataコンポーネントがアタッチされていません");
            }

            // リストに追加
            m_slotList.Add(slot);

            // UIcontrollerに追加
            AddSelectUIControler(slot);

        }

        await UniTask.CompletedTask;
    }



    // 一番最新のスロットにデータをセットする
    private void SetSlotData(PocketItemData _data)
    {
        foreach(var data in m_slotList)
        {
            if (data == null) continue;
            if(data.TryGetComponent<PocketItemSlotData>(out var slotData))
            {
                if (slotData.PocketItemData != null) continue;

                var itemData = ItemDataBaseManager.instance.GetItemData(_data.ItemTypeID, _data.ItemID);
                slotData.SetPocketItemData(_data);
                slotData.SetItemSlotData(itemData, m_pocketType);

                break;
            }
        }
    }



    protected int GetSlotSize(List<PocketItemData> _list)
    {
        //　ポケットの最大サイズ
        int size = 0;

        switch (m_slotListType)
        {
            case SlotListType.MaxSlotDisplay:
                {
                    // 最大までスロット作成
                    size = m_pocketType.GetPocketItemDataManager().ListMaxSize;
                    break;
                }

            case SlotListType.MinSlotDisplay:
                {
                    // 最小でスロット作成
                    size = _list.Count;
                    break;
                }
        }

        return size;
    }

}

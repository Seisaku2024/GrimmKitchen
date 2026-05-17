using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ItemInfo;
using PocketItemDataInfo;

public class InventoryManager : BasePocketItemDataController
{
    // インベントリの管理マネージャー(シングルトン)
    // 制作者(田内)

    //==================
    // シングルトン

    public static InventoryManager instance;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (InventoryManager)FindObjectOfType(typeof(InventoryManager));

            DontDestroyOnLoad(gameObject);

            StartInstance();

        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }



    //===============================================
    //                 実行処理
    //===============================================

    override protected void Load()
    {
        var data = PocketItemSaveLoader.Load(PocketType.Inventory);
        m_itemDataRC = new ReactiveCollection<PocketItemData>(data.PocketItemDataList);

        base.Load();
    }


    override public bool AddItem(ItemTypeID _itemTypeID, uint _itemID)
    {
        if (base.AddItem(_itemTypeID, _itemID) == true)
        {
            // 新規獲得アイテムとして追加
            NewItemManager.instance.AddItem(_itemTypeID, _itemID);
            return true;
        }
        else return false;
    }


    public override bool AddItem(PocketItemData _pocketItemData)
    {
        if (base.AddItem(_pocketItemData))
        {
            // 新規獲得アイテムとして追加
            NewItemManager.instance.AddItem(_pocketItemData.ItemTypeID, _pocketItemData.ItemID);

            return true;
        }
        else return false;
    }



    override public bool RemoveItem(ItemTypeID _itemTypeID, uint _itemID)
    {
        if (base.RemoveItem(_itemTypeID, _itemID))
        {
            // 新規獲得アイテムとして減算
            NewItemManager.instance.RemoveItem(_itemTypeID, _itemID);

            return true;
        }
        else return false;
    }


    public override bool RemoveItem(PocketItemData _pocketItemData)
    {
        if (base.RemoveItem(_pocketItemData))
        {
            // 新規獲得アイテムとして追加
            NewItemManager.instance.RemoveItem(_pocketItemData.ItemTypeID, _pocketItemData.ItemID);

            return true;
        }
        else return false;
    }


}

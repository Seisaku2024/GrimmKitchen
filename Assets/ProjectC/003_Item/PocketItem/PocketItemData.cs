using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ItemInfo;

[System.Serializable]
public class PocketItemData
{
    // 制作者　田内
    // ポケットアイテムデータ


    /// <summary>
    /// 変更時イベント
    /// </summary>
    public class GlobalChangePocketItemDataEvent
    {
        public PocketItemData PocketItemData;
    }


    public PocketItemData(ItemTypeID _itemTypeID, uint _itemID, int _num)
    {
        m_itemTypeID = _itemTypeID;
        m_itemID = _itemID;
        m_num = _num;
    }

    //=======================
    // アイテム所持数

    [Header("所持数")]
    [SerializeField]
    private int m_num = 1;

    public int Num
    {
        get
        {
            return m_num;
        }
        set
        {
            m_num = value;
            PublishGlobalChangePocketItemListEvent();
        }
    }

    //======================
    // アイテムの種類ID

    [Header("アイテムタイプ")]
    [SerializeField]
    private ItemTypeID m_itemTypeID = new();

    public ItemTypeID ItemTypeID
    {
        get
        {
            return m_itemTypeID;
        }
        set
        {
            m_itemTypeID = value;
            PublishGlobalChangePocketItemListEvent();
        }
    }

    //================
    // アイテムID

    private uint m_itemID = 0;

    public uint ItemID
    {
        get
        {
            return m_itemID;
        }
        set
        {
            m_itemID = value;
            PublishGlobalChangePocketItemListEvent();
        }
    }

    //================
    // セーブするか

    private bool m_isSave = true;

    public bool IsSave
    {
        get { return m_isSave; }
        set { m_isSave = value; }
    }

    //==================================
    //           実行処理
    //==================================

    /// <summary>
    /// 引数アイテムデータを作成する
    /// </summary>
    public static PocketItemData CreateItemData(ItemTypeID _itemTypeID, uint _itemID, int _num)
    {
        return new PocketItemData(_itemTypeID, _itemID, _num);
    }

    /// <summary>
    /// イベント発信
    /// </summary>
    public void PublishGlobalChangePocketItemListEvent()
    {
        // イベント送信
        GlobalChangePocketItemDataEvent eve = new();
        eve.PocketItemData = this;
        MessageBroker.Default.Publish<GlobalChangePocketItemDataEvent>(eve);
    }

}

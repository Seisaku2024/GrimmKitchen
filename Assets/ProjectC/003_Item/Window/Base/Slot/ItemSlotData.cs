using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using UniRx;


[RequireComponent(typeof(ItemDescription))]
public class ItemSlotData : MonoBehaviour
{
    // 制作者 田内

    //==============================================
    // ポケットの種類

    [Header("ポケット種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;

    public PocketType PocketType
    {
        get { return m_pocketType; }
    }

    //============================================
    // アイテムデータ
    protected BaseItemData m_itemData = null;

    public BaseItemData ItemData
    {
        get { return m_itemData; }
    }

    //========================================================
    // 説明文
    protected ItemDescription m_itemDescription = null;


    //======================================================================
    //                          実行処理
    //======================================================================


    virtual protected void Start()
    {
        // ポケットアイテムデータ変更イベントを受信
        MessageBroker.Default.Receive<GlobalChangePocketItemListEvent>().Subscribe(_ =>
        {
            // 一致すれば更新
            if (m_pocketType.GetPocketItemDataManager() == _.PocketItemDataController && m_itemData.ItemTypeID == _.PocketItemData.ItemTypeID && m_itemData.ItemID == _.PocketItemData.ItemID)
            {
                SetItemSlotData(m_itemData, m_pocketType);
            }

        }).AddTo(this);
    }



    #region メソッド説明
    /// <summary>
    // アイテムスロットのデータをセットする
    /// </summary>
    #endregion
    // アイテムスロットのデータをセットする
    virtual public void SetItemSlotData(BaseItemData _itemData, PocketType _pocketType)
    {
        if (_itemData == null)
        {
            Debug.LogError("引数アイテムデータが存在しません");
            return;
        }

        // データをセット
        m_itemData = _itemData;
        m_pocketType = _pocketType;

        // 説明文更新
        if (m_itemDescription == null) m_itemDescription = gameObject.GetComponent<ItemDescription>();
        m_itemDescription.UpdateDescription(m_itemData, m_pocketType);
    }


    virtual public void InitializeSlotData()
    {
        // 説明文更新
        if (m_itemDescription == null) m_itemDescription = gameObject.GetComponent<ItemDescription>();
        m_itemDescription.UpdateDescription(null, m_pocketType);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using PocketItemDataInfo;

public class PocketItemSlotData : ItemSlotData
{
    // 制作者 田内
    // ポケット情報のアイテムスロットデータ
    // ポケットアイテム情報
    private PocketItemData m_pocketItemData = null;

    public PocketItemData PocketItemData
    {
        get { return m_pocketItemData; }
    }

    //=========================================
    //              実行処理
    //=========================================


    override protected void Start()
    {
        // ポケットアイテムデータ変更イベントを受信
        MessageBroker.Default.Receive<PocketItemData.GlobalChangePocketItemDataEvent>().Subscribe(_ =>
        {
            // 一致すれば更新
            if (_.PocketItemData == m_pocketItemData)
            {
                SetItemSlotData(m_itemData, m_pocketType);

                if (_.PocketItemData.Num <= 0)
                {
                    InitializeSlotData();
                }
            }

        }).AddTo(this);


        MessageBroker.Default.Receive<GlobalRemoveItemEvent>().Subscribe(_ =>
        {
            // 一致すれば更新
            if (_.PocketItemData == m_pocketItemData)
            {
                if (m_pocketType.GetPocketItemDataManager().IsHave(m_pocketItemData) == false)
                {
                    InitializeSlotData();
                }
            }

        }).AddTo(this);
    }


    public void SetPocketItemData(PocketItemData _pocketItemData)
    {
        m_pocketItemData = _pocketItemData;
    }

    #region メソッド説明
    /// <summary>
    // アイテムスロットのデータをセットする
    /// </summary>
    #endregion
    // アイテムスロットのデータをセットする
    override public void SetItemSlotData(BaseItemData _itemData, PocketType _pocketType)
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
        m_itemDescription.UpdateDescription(m_itemData, m_pocketType, m_pocketItemData);
    }


    override public void InitializeSlotData()
    {
        m_pocketItemData = null;
        m_itemData = null;

        // 説明文更新
        if (m_itemDescription == null) m_itemDescription = gameObject.GetComponent<ItemDescription>();
        m_itemDescription.UpdateDescription(null, m_pocketType, null);
    }
}

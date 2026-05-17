using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using PocketItemDataInfo;
using UniRx;

public partial class ItemDescription : MonoBehaviour
{
    // 制作者 田内
    // アイテム説明文

    //======================================
    // ポケット種類
    protected PocketType m_pocketType = PocketType.Inventory;

    //====================================
    // 選択中のアイテムデータ
    protected BaseItemData m_itemData = null;

    //====================================
    // ポケットデータ
    protected PocketItemData m_pocketItemData = null;

    //======================================================================
    //                          実行処理
    //======================================================================

    virtual protected void Start()
    {
        // ポケットアイテムデータ変更イベントを受信
        MessageBroker.Default.Receive<GlobalChangePocketItemListEvent>().Subscribe(_ =>
        {
            if (m_pocketType.GetPocketItemDataManager() == _.PocketItemDataController)
            {
                // 更新
                UpdateDescription(m_itemData, m_pocketType, m_pocketItemData);
            }

        }).AddTo(this);
    }


    /// <summary>
    /// 引数アイテムIDを基に説明文を更新
    /// </summary>
    public void UpdateDescription(ItemTypeID _typeID, uint _itemID, PocketType _pocketType, PocketItemData _pocketItemData = null)
    {
        // データをセット
        m_itemData = ItemDataBaseManager.instance.GetItemData(_typeID, _itemID);
        m_pocketType = _pocketType;
        m_pocketItemData = _pocketItemData;

        UpdateDescription(m_itemData, m_pocketType, m_pocketItemData);
    }


    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void UpdateDescription(BaseItemData _data, PocketType _pocketType, PocketItemData _pocketItemData = null)
    {
        // データをセット
        m_itemData = _data;
        m_pocketType = _pocketType;
        m_pocketItemData = _pocketItemData;

        // 初期化
        InitDescription();

        // 説明文を更新
        SetDescription();

        // アクティブをセット
        SetActiveList();
    }



    // 説明文のテキストをセットする
    virtual protected void SetDescription()
    {
        // 共通説明文
        SetCommonDescription();

        // 料理説明文
        SetFoodDescription();

        //食材説明文
        SetIngredientDescription();

        // ポケット説明文
        SetPocketDescription();
    }


    // 説明文を初期化
    virtual public void InitDescription()
    {
        // 共通説明文
        InitilizeCommonDescription();

        // 料理説明文
        InitilizeFoodDescription();

        //食材説明文
        InitilizeIngredientDescription();

        // ポケット説明文
        InitilizePocketDescription();
    }

    // アクティブをセット
    virtual protected void SetActiveList()
    {
        // 共通説明文
        SetCommonActiveList();

        // 料理説明文
        SetFoodActiveList();

        // ポケット説明文
        SetPocketActiveList();

    }

}

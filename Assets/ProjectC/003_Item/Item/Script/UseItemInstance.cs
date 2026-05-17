using ItemInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;

public class UseItemInstance : MonoBehaviour
{
    //作成者　山本 田内
    //アイテム食事状態時に選んだアイテムのインスタンス生成とプレイヤーに食事効果を付与する
    //参考→PutItemInstance.cs,ThrowItemInstance.cs

    //===================================================================================

    [Header("アイテムのインスタンスを作成するタグ")]
    [SerializeField]
    private string m_tag = "Player";

    private GameObject m_targetObject = null;
    private CharacterCore m_characterCore = null;


    private PocketType m_pocketType = PocketType.Inventory;

    private ItemTypeID m_itemTypeID = new();

    private uint m_itemID = new();

    //===================================================
    //                      実行処理
    //===================================================

    private void Awake()
    {

        // Playerを探す
        GameObject[] getObj = GameObject.FindGameObjectsWithTag(m_tag);

        foreach (var obj in getObj)
        {
            m_targetObject = obj;
            break;
        }

        if (m_targetObject == null) return;
        m_characterCore = m_targetObject.GetComponent<CharacterCore>();
    }

    public void SetItemData(PocketType _pocketType, ItemTypeID _itemTypeID, uint _itemID)
    {
        m_pocketType = _pocketType;
        m_itemTypeID = _itemTypeID;
        m_itemID = _itemID;
    }


    // アイテムのインスタンスをHoldPointに作成する
    public void UseItem()
    {
        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(m_itemTypeID, m_itemID);
        if (data == null || data.ItemPrefab == null || m_characterCore == null) return;


        //食べられないなら早期リターン
        if (data.IsFoodType(FoodData.FoodType.DebuffCondition))
        {
            Debug.LogError(m_itemTypeID + ":" + m_itemID + " : この料理は食べられません");
            return;
        }

        //回復料理ならHP回復
        if (data.IsFoodType(FoodData.FoodType.Heal))
        {
            m_characterCore.Heal(data.HealingValue());
        }

        // 強化アイテムの処理
        else if (data.IsFoodType(FoodData.FoodType.StrengtheningStatus))
        {
            m_characterCore.PlayerParameters.Strengthening(data.AddPlayerStatus);
        }

        // 使用したアイテムをポケットから取り除く
        m_pocketType.GetPocketItemDataManager().RemoveItem(data.ItemTypeID, data.ItemID);
    }



    // アイテムのインスタンスをHoldPointに作成する
    public void UseItem(PocketType _pocketType,PocketItemData _data)
    {
        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(_data.ItemTypeID, _data.ItemID);
        if (data == null || data.ItemPrefab == null || m_characterCore == null) return;


        //食べられないなら早期リターン
        if (data.IsFoodType(FoodData.FoodType.DebuffCondition))
        {
            Debug.LogError(_data.ItemTypeID + ":" + _data.ItemID + " : この料理は食べられません");
            return;
        }

        //回復料理ならHP回復
        if (data.IsFoodType(FoodData.FoodType.Heal))
        {
            m_characterCore.Heal(data.HealingValue());
        }

        // 強化アイテムの処理
        else if (data.IsFoodType(FoodData.FoodType.StrengtheningStatus))
        {
            m_characterCore.PlayerParameters.Strengthening(data.AddPlayerStatus);
        }

        // 使用したアイテムをポケットから取り除く
        _pocketType.GetPocketItemDataManager().RemoveItem(_data);
    }
}

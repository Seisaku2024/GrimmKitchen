using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using FoodInfo;
using IngredientInfo;

[DefaultExecutionOrder(-100)]
public class ItemDataBaseManager : BaseManager<ItemDataBaseManager>
{
    // 制作者 田内
    // アイテムデータベースを管理するマネージャークラス


    [Header("食材データベース")]
    [SerializeField]
    private IngredientDataBase m_ingredientDataBase = null;

    public IngredientDataBase IngredientDataBase
    {
        get
        {
            IngredientDataBase database = new();
            if (m_ingredientDataBase != null) database = m_ingredientDataBase;
            return database;
        }
    }


    [Header("料理データベース")]
    [SerializeField]
    private FoodDataBase m_foodDataBase = null;

    public FoodDataBase FoodDataBase
    {
        get
        {
            FoodDataBase database = new();
            if (m_foodDataBase != null) database = m_foodDataBase;
            return database;
        }
    }

    //=================================================================
    //                  実行処理
    //=================================================================

    protected override void Load()
    {
        // 料理データセット
        foreach (var data in m_foodDataBase.FoodDataBaseList)
        {
            if (data == null) continue;
            data.SetData();

            var saveLoadData = RecipeSaveLoader.Load((FoodID)data.ItemID);
            data.Load(saveLoadData);
        }

        // 食材データセット
        foreach (var data in m_ingredientDataBase.IngredientDataBaseList)
        {
            if (data == null) continue;
            data.SetData();
        }
    }


    /// <summary>
    /// 引数アイテムデータを取得する
    /// </summary>
    public BaseItemData GetItemData(ItemTypeID _itemTypeID, uint _itemID)
    {
        switch (_itemTypeID)
        {
            case ItemTypeID.Food:
                {
                    foreach (var data in m_foodDataBase.FoodDataBaseList)
                    {
                        if (data.ItemTypeID != _itemTypeID) continue;
                        if (data.ItemID != _itemID) continue;

                        return data;
                    }
                    break;
                }
            case ItemTypeID.Ingredient:
                {
                    foreach (var data in m_ingredientDataBase.IngredientDataBaseList)
                    {
                        if (data.ItemTypeID != _itemTypeID) continue;
                        if (data.ItemID != _itemID) continue;

                        return data;
                    }
                    break;
                }
        }

        Debug.LogError("シリアライズされていません : " + _itemTypeID.ToString() + "," + _itemID.ToString());
        return null;

    }


    public T GetItemData<T>(ItemTypeID _itemTypeID, uint _itemID) where T : BaseItemData
    {
        switch (_itemTypeID)
        {
            case ItemTypeID.Food:
                {
                    foreach (var data in m_foodDataBase.FoodDataBaseList)
                    {
                        if (data.ItemTypeID != _itemTypeID) continue;
                        if (data.ItemID != _itemID) continue;

                        if (data is T)
                        {
                            return data as T;
                        }
                        else
                        {
                            Debug.LogError("変換エラー : " + _itemTypeID.ToString() + "," + _itemID.ToString());
                            return null;
                        }
                    }
                    break;
                }
            case ItemTypeID.Ingredient:
                {
                    foreach (var data in m_ingredientDataBase.IngredientDataBaseList)
                    {
                        if (data.ItemTypeID != _itemTypeID) continue;
                        if (data.ItemID != _itemID) continue;

                        if (data is T)
                        {
                            return data as T;
                        }
                        else
                        {
                            Debug.LogError("変換エラー : " + _itemTypeID.ToString() + "," + _itemID.ToString());
                            return null;
                        }
                    }
                    break;
                }

        }

        Debug.LogError("シリアライズされていません : " + _itemTypeID.ToString() + "," + _itemID.ToString());
        return null;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using PocketItemDataInfo;
using ItemInfo;
using IngredientInfo;
using FoodInfo;

public class AddItemButton : ButtonData
{
    // 制作者 田内
    // アイテムを追加するボタン

    [Header("ポケット種類")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;

    [System.Serializable]
    private struct AddPocketItem
    {
        [Header("所持数")]
        [SerializeField]
        [Min(0)]
        public int Num;

        [Header("アイテムタイプ")]
        [SerializeField]
        public ItemTypeID ItemTypeID;

        [Header("アイテムタイプ:Ingredientの場合")]
        public IngredientID IngredientID;

        [Header("アイテムタイプ:Foodの場合")]
        public FoodID FoodID;
    }

    [Header("追加アイテムリスト")]
    [SerializeField]
    private List<AddPocketItem> m_addPocketItemList = new();

    //=============================================
    //                  実行処理
    //=============================================


    public override async UniTask OnPressUpdate()
    {
        foreach (var data in m_addPocketItemList)
        {
            switch (data.ItemTypeID)
            {
                case ItemTypeID.Food:
                    {
                        // アイテムを追加
                        for (int i = 0; i < data.Num; ++i)
                        {
                            m_pocketType.GetPocketItemDataManager().AddItem(data.ItemTypeID, (uint)data.FoodID);
                        }
                        break;
                    }
                case ItemTypeID.Ingredient:
                    {
                        // アイテムを追加
                        for (int i = 0; i < data.Num; ++i)
                        {
                            m_pocketType.GetPocketItemDataManager().AddItem(data.ItemTypeID, (uint)data.IngredientID);
                        }
                        break;
                    }
                default:
                    {
                        Debug.LogError("アイテムタイプが割り当て外のIDになっています : " + data.ItemTypeID.ToString());
                        break;
                    }
            }
        }
        await UniTask.CompletedTask;
    }

}

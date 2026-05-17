using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using ItemInfo;
using IngredientInfo;
using FoodInfo;

public class DebugPocketItemSaveLoaderController : MonoBehaviour
{
    // 制作者 田内
    // デバッグ用処理

    [Header("ポケットタイプ")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;

    [System.Serializable]
    private struct DebugPocketItemSaveLoadeData
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

    [Header("デバッグ用ポケットアイテムデータ")]
    [SerializeField]
    private List<DebugPocketItemSaveLoadeData> m_debugPocketItemSaveLoadeDataList = new();

    //===============================================
    //              実行処理
    //===============================================

    /// <summary>
    /// リスト上にあるデータをファイルにセーブする
    /// </summary>

    [ContextMenu("DebugSaveList")]
    private void DebugSaveList()
    {
        List<PocketItemData> pocketItemDataList = new();

        foreach (var data in m_debugPocketItemSaveLoadeDataList)
        {
            switch (data.ItemTypeID)
            {
                case ItemTypeID.Food:
                    {
                        pocketItemDataList.Add(PocketItemData.CreateItemData(ItemTypeID.Food, (uint)data.FoodID, data.Num));
                        break;
                    }
                case ItemTypeID.Ingredient:
                    {
                        pocketItemDataList.Add(PocketItemData.CreateItemData(ItemTypeID.Ingredient, (uint)data.IngredientID, data.Num));
                        break;
                    }
                default:
                    {
                        Debug.LogError("アイテムタイプが割り当て外のIDになっています : " + data.ItemTypeID.ToString());
                        break;
                    }
            }
        }

        // デバッグ作成
        PocketItemSaveLoad pocketItemSaveLoad = new();
        pocketItemSaveLoad.PocketItemDataList = pocketItemDataList;

        PocketItemSaveLoader.Save(m_pocketType, pocketItemSaveLoad);
    }


    /// <summary>
    /// 全てのデータを初期化してファイルにセーブする
    /// </summary>

    [ContextMenu("DebugDeleteAllData")]
    private void DebugDeleteAllData()
    {
        PocketItemSaveLoader.DeleteAllData(m_pocketType);
    }

    /// <summary>
    /// 現状のデータをファイルにセーブする
    /// ※実行中にのみ動作
    /// </summary>

    [ContextMenu("DebugSaveCurrentData")]
    private void DebugSaveCurrentData()
    {
        // 現状のデータをセーブ
        PocketItemSaveLoader.SaveCurrentData(m_pocketType);
    }
}

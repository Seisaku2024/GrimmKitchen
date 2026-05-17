using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using IngredientInfo;
using FoodInfo;
using PocketItemDataInfo;

public class AddRandomItemRewardChallenge : BaseRewardChallengeData
{

    // 制作者 田内
    // アイテムを追加する報酬

    [Header("追加先")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.ManagementStorage;

    [Header("追加数")]
    [SerializeField]
    [Min(1)]
    private int m_addNum = 1;

    [Header("種類")]
    [SerializeField]
    private ItemTypeID m_itemTypeID = ItemTypeID.Ingredient;

    [Header("料理")]
    [SerializeField]
    private List<FoodID> m_foodIDList = new();

    [Header("食材")]
    [SerializeField]
    private List<IngredientID> m_ingredientIDList = new();

    //================================================
    //                  実行処理
    //================================================

    public override void UpdateRewardChallenge()
    {
        uint id = 0;
        switch (m_itemTypeID)
        {
            case ItemTypeID.Food:
                {
                    id = (uint)m_foodIDList.GetRandom();
                    break;
                }

            case ItemTypeID.Ingredient:
                {
                    id = (uint)m_ingredientIDList.GetRandom();
                    break;
                }

            default:
                {
                    Debug.LogError("種類が存在しません");
                    return;
                }
        }

        // 追加
        for (int i = 0; i < m_addNum; ++i)
        {
            if (m_pocketType.GetPocketItemDataManager().AddItem(m_itemTypeID, id) == false) break;
        }
    }

}

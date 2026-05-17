using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodInfo;

public class UnlockRecipeRewardChallenge : BaseRewardChallengeData
{
    // 制作者 田内
    // レシピをアンロックする報酬

    [Header("アンロック料理ID")]
    [SerializeField]
    private FoodID m_foodID = FoodID.Omelette;

    //===================================================
    //                  実行処理
    //===================================================
    public override void UpdateRewardChallenge()
    {
        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemInfo.ItemTypeID.Food, (uint)m_foodID);
        if (data == null || data.LockRecipeData == null) return;

        // レシピをアンロック
        data.OnUnLock();
    }

}
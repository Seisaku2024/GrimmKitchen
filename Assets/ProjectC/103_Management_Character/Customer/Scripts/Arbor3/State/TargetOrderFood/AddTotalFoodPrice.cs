using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class AddTotalFoodPrice : BaseCustomerStateBehaviour
{
    // 制作者 上甲,田内
    // 支払う金額を貯めこむ

    //==================================================
    //                  実行処理
    //==================================================

    public override void OnStateBegin()
    {
        AddFoodList();
        AddPrice();
    }

    private void AddFoodList()
    {
        var data = GetCustomerData();
        if (data == null) return;
        var targetFood = data.TargetOrderFoodData;
        if (targetFood == null) return;

        data.EatFoodList.Add(targetFood.FoodID);
    }

    private void AddPrice()
    {
        var data = GetCustomerData();
        if (data == null) return;
        var targetFood = data.TargetOrderFoodData;
        if (targetFood == null) return;
        var foodData = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemInfo.ItemTypeID.Food, (uint)targetFood.FoodID);
        if (foodData == null) return;

        int price = foodData.Price();

        // 値段を変換して取得
        var challenge = ChallengeDataBaseManager.instance.GetData(ChallengeManager.instance.ChallengeID);
        if (challenge != null)
        {
            // 高額料理であれば更新
            price = challenge.GetConvertValueBusinessConditionsRatio(price, foodData.GetIngredientTypeID());
        }

        data.TotalPrice += price;
    }

}

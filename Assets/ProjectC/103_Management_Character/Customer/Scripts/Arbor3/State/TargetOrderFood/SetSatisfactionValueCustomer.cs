using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class SetSatisfactionValueCustomer : BaseCustomerStateBehaviour
{
    // 制作者 田内
    // 満足値をセット

    //========================================
    //				実行処理
    //========================================

    public override void OnStateBegin()
    {
        SetSatisfactionValue();
    }

    private void SetSatisfactionValue()
    {
        var data = GetCustomerData();
        if (data == null) return;
        var targetFood = data.TargetOrderFoodData;
        if (targetFood == null) return;
        var foodData = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemInfo.ItemTypeID.Food, (uint)targetFood.FoodID);
        if (foodData == null) return;

        int satisfactionValue = foodData.SatisfactionValue();

        // 満足値をセット
        var challenge = ChallengeDataBaseManager.instance.GetData(ChallengeManager.instance.ChallengeID);
        if (challenge != null)
        {
            // 高額料理であれば更新
            satisfactionValue = challenge.GetConvertValueBusinessConditionsRatio(satisfactionValue, foodData.GetIngredientTypeID());
        }

        data.SatisfactionValue += satisfactionValue;
    }
}

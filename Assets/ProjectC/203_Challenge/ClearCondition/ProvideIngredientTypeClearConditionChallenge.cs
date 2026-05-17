using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using IngredientInfo;

public class ProvideIngredientTypeClearConditionChallenge : BaseClearConditionChallengeData
{
    // 制作者 田内
    // 料理提供条件

    [Header("材料タイプ")]
    [SerializeField]
    private IngredientTypeID m_ingredientTypeID = IngredientTypeID.None;

    [Header("売る数")]
    [SerializeField]
    [Min(1)]
    private int m_provideNum = 5;

    //===========================================
    //              実行処理
    //===========================================

    private void Start()
    {
        MessageBroker.Default.Receive<ManagementProvideFoodData.GlobalAddProvideFoodDataEvent>().Subscribe(_ =>
        {
            var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemInfo.ItemTypeID.Food, (uint)_.ProvideFoodData.FoodID);
            if (data == null) return;

            if ((data.GetIngredientTypeID() & m_ingredientTypeID) != 0)
            {
                // イベントを発信
                PublishChangeClearConditionChallenge();
            }

        }).AddTo(this);

    }

    public override bool IsClear()
    {
        uint currentProvideNum = 0;
        foreach (var provideData in ManagementGameDataManager.instance.ProvideFoodDataRPRC)
        {
            if (provideData == null || provideData.Value == null) continue;

            var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemInfo.ItemTypeID.Food, (uint)provideData.Value.FoodID);
            if ((data.GetIngredientTypeID() & m_ingredientTypeID) != 0)
            {
                currentProvideNum += provideData.Value.SoldNum;
            }
        }

        if (m_provideNum <= currentProvideNum) return true;
        return false;
    }

    public override string GetConditionText()
    {
        uint currentProvideNum = 0;
        foreach (var provideData in ManagementGameDataManager.instance.ProvideFoodDataRPRC)
        {
            if (provideData == null || provideData.Value == null) continue;

            var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemInfo.ItemTypeID.Food, (uint)provideData.Value.FoodID);
            if ((data.GetIngredientTypeID() & m_ingredientTypeID) != 0)
            {
                currentProvideNum += provideData.Value.SoldNum;
            }
        }

        string text = currentProvideNum.ToString() + " / " + m_provideNum.ToString();
        return text;
    }

    public override string GetConditionNumText()
    {
        string text = "×" + m_provideNum.ToString();
        return text;
    }

    public override float GetClearRatio()
    {
        uint currentProvideNum = 0;
        foreach (var provideData in ManagementGameDataManager.instance.ProvideFoodDataRPRC)
        {
            if (provideData == null || provideData.Value == null) continue;

            var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemInfo.ItemTypeID.Food, (uint)provideData.Value.FoodID);
            if ((data.GetIngredientTypeID() & m_ingredientTypeID) != 0)
            {
                currentProvideNum += provideData.Value.SoldNum;
            }
        }

        float ratio = (float)currentProvideNum / (float)m_provideNum;

        ratio = Mathf.Clamp(value: ratio, max: 1.0f, min: 0.0f);

        return ratio;
    }


    //===========================================================
    //                  inspector処理
    //===========================================================

    // 間違えて複数のIDを選択した場合、ヒューマンエラーを発生させないようにエラーを発生させる
    private void OnValidate()
    {
        if (HasMultipleFlags(m_ingredientTypeID))
        {
            Debug.LogError($"ID '{name}' は複数のフラグが選択されています。一つだけ選択してください。");
            m_ingredientTypeID = IngredientTypeID.None; // デフォルトに戻す
        }
    }

    private bool HasMultipleFlags(IngredientTypeID id)
    {
        // ビットが複数立っているか確認
        return id != IngredientTypeID.None && (id & (id - 1)) != 0;
    }
}

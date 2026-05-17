using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using Cysharp.Threading.Tasks;

public class CreateProvideFoodUseIngredientSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // 経営で使用した材料スロットを作成する

    //======================================================
    //                     実行処理
    //======================================================

    protected override async UniTask CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        // 提供料理リスト
        foreach (var data in ManagementGameDataManager.instance.ProvideFoodDataRPRC)
        {
            if (data == null || data.Value == null) continue;

            // 料理データ
            var foodData = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)data.Value.FoodID);
            if (foodData == null) continue;

            // 必要材料リスト
            foreach (var need in foodData.NeedIngredientObjectList)
            {
                var ingredientData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Ingredient, (uint)need.IngredientID);
                if (ingredientData == null) continue;

                // 子オブジェクトにスロットを追加
                var slot = Instantiate(m_slot, transform);

                if (slot.TryGetComponent<ProvideFoodUseIngredientSlotData>(out var slotData))
                {
                    slotData.SetNeedProvideFoodUseIngredient(data.Value, need);
                    slotData.SetItemSlotData(ingredientData, ProvideFoodManager.instance.PocketType);
                }
                else
                {
                    Debug.LogError("スロットにNeedIngredientSlotDataコンポーネントがアタッチされていません");
                }

                // リストに追加
                m_slotList.Add(slot);

                // UIcontrollerに追加
                AddSelectUIControler(slot);
            }
        }

        await UniTask.CompletedTask;
    }
}

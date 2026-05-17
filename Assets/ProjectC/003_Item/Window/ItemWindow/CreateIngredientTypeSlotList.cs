using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngredientInfo;

public class CreateIngredientTypeSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // 食材種類IDを基にスロットを作成する

    private IngredientTypeID m_ingredientTypeID = IngredientTypeID.None;

    //==========================================
    //              実行処理
    //==========================================

    public void SetData(IngredientTypeID _id)
    {
        m_ingredientTypeID = _id;
    }


    protected override async UniTask CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        // 既存スロットを削除
        DestroySlotList();


        foreach (IngredientTypeID flag in System.Enum.GetValues(typeof(IngredientTypeID)))
        {
            // Noneの場合は表示しない
            if (flag == IngredientTypeID.None) continue;

            // 当てはまれば
            if ((m_ingredientTypeID & flag) != flag) continue;

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<IngredientTypeSlotData>(out var slotData))
            {
                var ingredientTypeData = IngredientTypeDataBaseManager.instance.GetData(flag);
                slotData.SetData(ingredientTypeData);
            }
            else
            {
                Debug.LogError("PocketItemSlotDataコンポーネントがアタッチされていません");
            }

            // リストに追加
            m_slotList.Add(slot);

            // UIcontrollerに追加
            AddSelectUIControler(slot);

        }

        await UniTask.CompletedTask;


    }


}

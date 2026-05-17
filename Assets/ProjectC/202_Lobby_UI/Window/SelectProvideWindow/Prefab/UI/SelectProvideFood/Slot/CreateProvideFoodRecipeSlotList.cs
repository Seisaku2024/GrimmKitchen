using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CreateProvideFoodRecipeSlotList : CreateRecipeSlotList
{
    // 制作者 田内

    [Header("ソートコントローラー")]
    [SerializeField]
    private SortProvideFoodController m_sortProvideFoodController = null;

    //================================
    //           実行処理
    //================================

    void Start()
    {
        m_pocketType = ProvideFoodManager.instance.PocketType;
    }

    override protected async UniTask CreateSlotInstance()
    {

        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        List<FoodData> foodList = ItemDataBaseManager.instance.FoodDataBase.FoodDataBaseList;

        // ソートする場合
        if (m_sortProvideFoodController != null)
        {
            foodList = m_sortProvideFoodController.Sort();
        }

        foreach (var data in foodList)
        {
            var foodData = ItemDataBaseManager.instance.GetItemData<FoodData>(data.ItemTypeID, data.ItemID);
            if (foodData == null || foodData.LockRecipeData == null) continue;

            // ロックされていれば作成しない
            if (m_lockDisplayFlg == false && foodData.LockRecipeData.IsLock == true) continue;

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<ItemSlotData>(out var slotData))
            {
                // アイテムスロットのデータをセット
                var itemData = ItemDataBaseManager.instance.GetItemData(data.ItemTypeID, data.ItemID);
                slotData.SetItemSlotData(itemData, m_pocketType);
            }
            else
            {
                Debug.LogError("RecipeItemSlotDataコンポーネントがアタッチされていません");
            }

            m_slotList.Add(slot);

            // UIcontrollerに追加
            AddSelectUIControler(slot);

        }

        await UniTask.CompletedTask;
    }


}

using UnityEngine;
using ItemInfo;
using PocketItemDataInfo;

using Cysharp.Threading.Tasks;

public class CreateRecipeSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // レシピスロットを作成する

    [Header("ポケットマネージャーの種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;

    [Header("ロック中の料理を表示")]
    [SerializeField]
    protected bool m_lockDisplayFlg = false;

    //==================================================
    //                  実行処理
    //==================================================

    override protected async UniTask CreateSlotInstance()
    {

        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }


        var foodList = ItemDataBaseManager.instance.FoodDataBase.FoodDataBaseList;

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

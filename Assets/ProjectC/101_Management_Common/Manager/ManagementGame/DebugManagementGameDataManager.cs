using FoodInfo;
using ItemInfo;
using PocketItemDataInfo;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DebugManagementGameDataManager : MonoBehaviour
{
    // 制作者 田内
    // デバッグ用

    [Header("デバッグするか")]
    [SerializeField]
    private bool m_isDebug = true;

    [Header("デバッグ用")]
    [SerializeField]
    private List<FoodID> m_foodIDList = new();

    [Header("何個作成可能にするか")]
    [SerializeField]
    [Range(1, 100)]
    private int m_num = 1;


    void Start()
    {
#if UNITY_EDITOR

        if (m_isDebug == false) return;

        foreach (var id in m_foodIDList)
        {
            var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)id);

            for (int n = 0; n < m_num; ++n)
            {
                foreach (var need in data.NeedIngredientObjectList)
                {
                    if (need == null) continue;
                    for (int i = 0; i < need.Num; ++i)
                    {
                        ProvideFoodManager.instance.PocketType.GetPocketItemDataManager().AddItem(ItemTypeID.Ingredient, (uint)need.IngredientID);
                    }
                }
            }

            ReactiveProperty<ManagementProvideFoodData> rpData = new();
            rpData.Value = new(id);
            ManagementGameDataManager.instance.ProvideFoodDataRPRC.Add(rpData);

        }
#endif
    }
}

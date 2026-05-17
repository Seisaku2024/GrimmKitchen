using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using FoodInfo;

public class SortProvideFoodController : MonoBehaviour
{
    // 制作者 田内
    // 提供料理のソート

    private enum SortType
    {
        None = 0,
        Provide,
    }

    [Header("ソートタイプ")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;

    [Header("ソートタイプ")]
    [SerializeField]
    private SortType m_sortType = SortType.None;

    //========================================
    //             実行処理
    //========================================

    public List<FoodData> Sort()
    {
        switch (m_sortType)
        {

            case SortType.Provide:
                {
                    return ProvideSort();
                }

            case SortType.None:
            default:
                {

                    return NoneSort();
                }
        }
    }

    // シート無し
    private List<FoodData> NoneSort()
    {
        var foodList = ItemDataBaseManager.instance.FoodDataBase.FoodDataBaseList;
        return foodList;
    }

    // 提供可能ソート
    private List<FoodData> ProvideSort()
    {
        var foodList = ItemDataBaseManager.instance.FoodDataBase.FoodDataBaseList;

        List<FoodData> sortList = new();
        List<FoodData> provideList = new();
        List<FoodData> unProvideList = new();


        foreach (var data in foodList)
        {
            if (data == null) continue;

            if (FoodData.IsProvide(m_pocketType, (FoodID)data.ItemID))
            {
                provideList.Add(data);
            }
            else
            {
                unProvideList.Add(data);
            }
        }

        sortList.AddRange(provideList);
        sortList.AddRange(unProvideList);

        return sortList;
    }


}

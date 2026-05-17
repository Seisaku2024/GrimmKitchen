using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FoodDataBase", menuName = "ScriptableObjects/Food/作成 FoodItemDataBase")]
public class FoodDataBase : ScriptableObject
{
    // 制作者 田内
    // 料理データベース

    public FoodDataBase()
    {
        m_foodDataBaseList = new();
    }


    [Header("料理のデータリスト")]
    [SerializeField]
    private List<FoodData> m_foodDataBaseList = new();


    public List<FoodData> FoodDataBaseList { get { return m_foodDataBaseList; } }
}

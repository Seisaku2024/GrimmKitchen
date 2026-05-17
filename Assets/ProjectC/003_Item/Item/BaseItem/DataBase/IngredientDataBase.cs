using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IngredientDataBase", menuName = "ScriptableObjects/Ingredient/作成 IngredientItemDataBase")]
public class IngredientDataBase : ScriptableObject
{

    // 制作者 田内
    // 食材データベース

    public IngredientDataBase()
    {
        m_ingredientDataBaseList = new();
    }

    [Header("食材データリスト")]
    [SerializeField]
    private List<IngredientData> m_ingredientDataBaseList = new();


    public List<IngredientData> IngredientDataBaseList { get { return m_ingredientDataBaseList; } }

}

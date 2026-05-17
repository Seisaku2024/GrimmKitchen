using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientTypeDataBase", menuName = "ScriptableObjects/IngredientType/作成 IngredientItemTypeDataBase")]
public class IngredientTypeDataBase : ScriptableObject
{
    // 制作者 田内
    // 食材種類データベース

    [Header("データリスト")]
    [SerializeField]
    private List<IngredientTypeData> m_ingredientTypeDataList = new();

    public List<IngredientTypeData> IngredientTypeDataList
    {
        get { return m_ingredientTypeDataList; }
    }

}

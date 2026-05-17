using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FoodInfo;
using NaughtyAttributes;
using ConditionInfo;

/// <summary>
/// 制作者　吉田
/// ItemDescription　の食材
/// </summary>
public partial class ItemDescription : MonoBehaviour
{
    [Foldout("食材")]
    [Header("-------------------------------------------------------")]
    [Header("食材情報をUIに渡す")]// 吉田
    [SerializeField]
    private DetailByIngredientController m_detailByIngredientController = null;

    private void SetIngredientDescription()
    {
        // キャストに失敗した場合初期化し終了
        if (m_itemData is IngredientData == false)
        {
            InitilizeIngredientDescription();
            return;
        }
        var ingredientData = m_itemData as IngredientData;

        // 食材情報をUIに渡す
        SetDetailByIngredient(ingredientData);
    }

    private void InitilizeIngredientDescription()
    {
        SetDetailByIngredient(null, false);
    }

    public void SetDetailByIngredient(IngredientData _data, bool _active = true)
    {
        if (m_detailByIngredientController == null) return;
        m_detailByIngredientController.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_data == null) return;

        // 食材情報をセット
        m_detailByIngredientController.SetDetailByIngredient(_data);

        // 表示
        m_detailByIngredientController.gameObject.SetActive(true);

    }
}

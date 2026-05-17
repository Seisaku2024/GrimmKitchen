using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IngredientTypeDescription))]
public class IngredientTypeSlotData : MonoBehaviour
{
    // 制作者 田内
    // 食材種類スロットデータ

    // 説明文
    private IngredientTypeDescription m_ingredientTypeDescription = null;

    // 保持データ
    private IngredientTypeData m_ingredientTypeData = null;

    //==========================================================
    //                       実行処理
    //==========================================================

    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void SetData(IngredientTypeData _data)
    {
        // データをセット
        m_ingredientTypeData = _data;

        // 説明文を更新
        if (m_ingredientTypeDescription == null) m_ingredientTypeDescription = gameObject.GetComponent<IngredientTypeDescription>();
        m_ingredientTypeDescription.UpdateDescription(m_ingredientTypeData);
    }


}

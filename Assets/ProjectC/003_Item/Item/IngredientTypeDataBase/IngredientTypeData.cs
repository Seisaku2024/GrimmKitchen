using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngredientInfo;

[CreateAssetMenu(fileName = "IngredientTypeData", menuName = "ScriptableObjects/IngredientType/作成 IngredientItemTypeData")]
public class IngredientTypeData : ScriptableObject
{
    // 制作者 田内
    // 食材種類データ

    //====================================

    [Header("ID")]
    [SerializeField]
    private IngredientTypeID m_ingredientTypeID = IngredientTypeID.None;

    public IngredientTypeID IngredientTypeID
    {
        get { return m_ingredientTypeID; }
    }

    //=====================================

    [Header("種類名")]
    [SerializeField]
    private string m_ingredientTypeName = "種類名";

    public string IngredientTypeName
    {
        get { return m_ingredientTypeName; }
    }
    //=====================================

    [Header("アイコン画像")]
    [SerializeField]
    private Sprite m_iconSprite = null;

    public Sprite IconSprite
    {
        get { return m_iconSprite; }
    }


    //===========================================================
    //                  inspector処理
    //===========================================================

    // 間違えて複数のIDを選択した場合、ヒューマンエラーを発生させないようにエラーを発生させる
    private void OnValidate()
    {
        if (HasMultipleFlags(m_ingredientTypeID))
        {
            Debug.LogError($"ID '{name}' は複数のフラグが選択されています。一つだけ選択してください。");
            m_ingredientTypeID = IngredientTypeID.None; // デフォルトに戻す
        }
    }

    private bool HasMultipleFlags(IngredientTypeID id)
    {
        // ビットが複数立っているか確認
        return id != IngredientTypeID.None && (id & (id - 1)) != 0;
    }
}

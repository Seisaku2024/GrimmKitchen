using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngredientInfo;
using ItemInfo;

[CreateAssetMenu(fileName = "IngredientData", menuName = "ScriptableObjects/Ingredient/作成 IngredientItemData")]
[System.Serializable]
public class IngredientData : BaseItemData
{
    // 制作者 田内
    // 食材データ

    public override void SetData()
    {
        m_itemTypeID = ItemTypeID.Ingredient;
        m_itemID = (uint)m_ingredientID;
    }


    //============================
    // この材料のID


    [Header("材料のID")]
    [SerializeField]
    private IngredientID m_ingredientID = IngredientID.SleepApple;


    //============================
    // 材料種類ID

    [Header("材料種類ID")]
    [SerializeField]
    private IngredientTypeID m_ingredientTypeID = IngredientTypeID.None;

    public IngredientTypeID IngredientTypeID
    {
        get { return m_ingredientTypeID; }
    }

    //============================
    // 回復量

    [Header("回復値")]
    [SerializeField]
    [Range(0, 300)]
    protected uint m_healValue = new();


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理の回復量を返す、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理の回復量(int)
    /// </returns>
    /// --------------------------------------
    #endregion
    public uint HealValue { get { return m_healValue; } }


    //===================================
    // 金額

    [Header("価格")]
    [SerializeField]
    [Min(0)]
    private int m_price = 100;

    public int Price { get { return m_price; } }


    //=========================================
    // 満足値

    [Header("満足値")]
    [SerializeField]
    [Min(0)]
    private int m_satisfactionValue = 5;

    public int SatisfactionValue
    {
        get { return m_satisfactionValue; }
    }


}

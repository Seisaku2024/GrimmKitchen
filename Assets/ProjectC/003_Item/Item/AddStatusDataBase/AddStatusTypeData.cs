using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngredientInfo;
using static FoodData.AddStatus;

[CreateAssetMenu(fileName = "AddStatusTypeData", 
    menuName = "ScriptableObjects/AddStatusType/作成 AddStatusTypeData")]
public class AddStatusTypeData : ScriptableObject
{
    // 制作者 吉田
    // ステータス種類データ

    //====================================

    [Header("AddStatusType")]
    [SerializeField]
    private AddStatusType m_addStatusType = AddStatusType.Attack;

    public AddStatusType AddStatusType
    {
        get { return m_addStatusType; }
    }

    //=====================================

    [Header("種類名")]
    [SerializeField]
    private UnityEngine.Localization.LocalizedString m_name;

    public UnityEngine.Localization.LocalizedString Name
    {
        get { return m_name; }
    }
    //=====================================

    [Header("アイコン画像")]
    [SerializeField]
    private Sprite m_iconSprite = null;

    public Sprite IconSprite
    {
        get { return m_iconSprite; }
    }

    //=====================================

    [Header("メインカラー")]
    [SerializeField]
    private Color m_mainColor = Color.white;
    public Color MainColor
    {
        get { return m_mainColor; }
    }

    [Header("サブカラー")]
    [SerializeField]
    private Color m_subColor = Color.white;
    public Color SubColor
    {
        get { return m_subColor; }
    }


    ////===========================================================
    ////                  inspector処理
    ////===========================================================

    //// 間違えて複数のIDを選択した場合、ヒューマンエラーを発生させないようにエラーを発生させる
    //private void OnValidate()
    //{
    //    if (HasMultipleFlags(m_addStatusType))
    //    {
    //        Debug.LogError($"ID '{name}' は複数のフラグが選択されています。一つだけ選択してください。");
    //        m_addStatusType = AddStatusType.None; // デフォルトに戻す
    //    }
    //}

    //private bool HasMultipleFlags(IngredientTypeID id)
    //{
    //    // ビットが複数立っているか確認
    //    return id != AddStatusType.None && (id & (id - 1)) != 0;
    //}
}

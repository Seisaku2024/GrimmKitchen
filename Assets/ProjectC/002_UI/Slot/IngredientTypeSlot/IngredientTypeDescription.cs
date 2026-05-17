using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientTypeDescription : MonoBehaviour
{
    // 制作者 田内
    // 食材種類説明文


    [Header("=================================")]
    [Header("種類名")]
    [SerializeField]
    protected TextMeshProUGUI m_nameText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_nameTextShowHideUIData = new();

    //================================================

    [Header("=================================")]
    [Header("アイコン画像")]
    [SerializeField]
    protected Image m_iconImage = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_iconImageShowHideUIData = new();

    //================================================

    [Header("=================================")]
    [Header("チャレンジの色")]
    [SerializeField]
    protected bool m_isChangeColor = false;

    [SerializeField]
    protected Color m_challengeColor = Color.white;

    protected Color m_saveImageColor = Color.white;
    protected Color m_saveTextColor = Color.white;
    //================================================

    // データ
    private IngredientTypeData m_ingredientTypeData = null;

    //==========================================================
    //                       実行処理
    //==========================================================

    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void UpdateDescription(IngredientTypeData _data)
    {
        // データをセット
        m_ingredientTypeData = _data;

        SetSlotData();

        SetActiveGameObjectList();
    }


    virtual protected void SetSlotData()
    {
        // Noneの場合は非表示 追加：吉田
        if (m_ingredientTypeData.IngredientTypeID == IngredientInfo.IngredientTypeID.None)
        {
            SetName(false);
            SetIconImage(false);
            return;
        }

        SetName();
        SetIconImage();
    }

    virtual protected void SetActiveGameObjectList()
    {
        if (m_nameTextShowHideUIData != null) m_nameTextShowHideUIData.SetActiveGameObjectList(m_nameText);
        if (m_iconImageShowHideUIData != null) m_iconImageShowHideUIData.SetActiveGameObjectList(m_iconImage);
    }


    // 種類名
    private void SetName(bool _active = true)
    {
        if (m_nameText == null) return;

        m_nameText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_ingredientTypeData == null) return;

        // 種類名をセット
        m_nameText.text = m_ingredientTypeData.IngredientTypeName;
        m_nameText.gameObject.SetActive(true);
    }

    // アイコン画像
    private void SetIconImage(bool _active = true)
    {
        if (m_iconImage == null) return;

        m_iconImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_ingredientTypeData == null) return;

        m_iconImage.sprite = m_ingredientTypeData.IconSprite;
        m_iconImage.gameObject.SetActive(true);
    }


}

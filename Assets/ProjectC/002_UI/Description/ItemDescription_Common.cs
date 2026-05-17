using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PocketItemDataInfo;
using NaughtyAttributes;
using ItemInfo;
using IngredientInfo;

public partial class ItemDescription : MonoBehaviour
{
    // 制作者 田内
    // 共通

    //==========================================================


    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("アイテム画像Image")]
    [SerializeField]
    private Image m_itemImage = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_itemImageList = new();


    //==============================================================


    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("アイテム名Text")]
    [SerializeField]
    protected TextMeshProUGUI m_nameTextMeshPro = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_nameList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("説明文Text")]
    [SerializeField]
    protected TextMeshProUGUI m_descriptionTextMeshPro = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_descriptionList = new();

    //===========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("アイテム種類画像")]
    [SerializeField]
    protected Image m_typeImage = null;

    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_typeImageList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("アイテム種類Text")]
    [SerializeField]
    protected TextMeshProUGUI m_typeTextMeshPro = null;

    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_typeList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("状態画像Image")]
    [SerializeField]
    protected Image m_conditionImage = null;

    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_conditionImageList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("最大保持数")]
    [SerializeField]
    protected TextMeshProUGUI m_maxNumText = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_maxNumTextList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("レベルText")]
    [SerializeField]
    protected TextMeshProUGUI m_levelTextMeshPro = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_levelList = new();

    //==========================================================


    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("食材種類スロット作成")]
    [SerializeField]
    protected CreateIngredientTypeSlotList m_createIngredientTypeSlotList = null;

    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_createIngredientTypeSlotListList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("状態画像CreateConditionImage")]
    [SerializeField]
    protected CreateConditionImage m_createConditionImage = null;

    [Foldout("食料")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_createConditionImageList = new();


    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("総所持数Text")]
    [SerializeField]
    protected TextMeshProUGUI m_numTextMeshPro = null;


    protected enum DisplayOneType
    {
        Disable,
        Enable,
    }


    [Foldout("共通")]
    [Header("0を表示するか")]
    [SerializeField]
    protected DisplayOneType m_displayOneType = DisplayOneType.Enable;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_numList = new();


    //==========================================================

    // 追加（吉田）
    [Foldout("アイテム状態 背景色変更")]
    [Header("=====================================")]
    [Header("状態 背景色変更 Image")]
    [SerializeField]
    protected Image m_conditionBackColor = null;

    [SerializeField]
    protected TwoColorGrad m_conditionBackColorGrad = null;

    [Foldout("アイテム状態 背景色変更")]
    [Header("状態 背景色 透明度")]
    [Range(0, 1)]
    [SerializeField]
    protected float m_conditionBackAlpha = 0.2f;



    //======================================================================
    //                          実行処理
    //======================================================================

    private void SetCommonDescription()
    {
        // アイテムの画像をセット
        SetItemImage();

        // 種類画像をセット
        SetTypeImage();

        // 種類テキストをセット
        SetTypeText();

        SetConditionImage();

        SetCreateConditionImage();

        // 名前テキストをセット
        SetNameText();

        // 説明文テキストをセット
        SetDescriptionText();

        // 最大所持数
        SetMaxNumText();

        // レベルテキストをセット
        SetLevelText();

        // 所持数を表示
        SetNumText();

        // 食材種類
        CreateIngredientTypeSlotList();

        // 色
        SetConditionBackColor();
    }

    private void InitilizeCommonDescription()
    {
        // アイテム画像
        SetItemImage(false);

        // 種類画像
        SetTypeImage(false);

        // 種類テキスト
        SetTypeText(false);

        // 名前
        SetNameText(false);

        SetConditionImage(false);

        SetCreateConditionImage(false);

        // 説明
        SetDescriptionText(false);

        // 最大所持数
        SetMaxNumText(false);

        // レベル
        SetLevelText(false);

        // 所持数
        SetNumText(false);

        // 食材種類
        CreateIngredientTypeSlotList(false);

        // 色
        SetConditionBackColor(false);
    }

    private void SetCommonActiveList()
    {
        // アイテム画像
        UIExtensions.CheckToSetActiveGameObjectList(m_itemImage, m_itemImageList);

        // 種類
        UIExtensions.CheckToSetActiveGameObjectList(m_typeTextMeshPro, m_typeList);

        // 種類画像
        UIExtensions.CheckToSetActiveGameObjectList(m_typeImage, m_typeImageList);

        // 名前
        UIExtensions.CheckToSetActiveGameObjectList(m_nameTextMeshPro, m_nameList);

        // 説明
        UIExtensions.CheckToSetActiveGameObjectList(m_descriptionTextMeshPro, m_descriptionList);

        // 最大所持数
        UIExtensions.CheckToSetActiveGameObjectList(m_maxNumText, m_maxNumTextList);

        // レベル
        UIExtensions.CheckToSetActiveGameObjectList(m_levelTextMeshPro, m_levelList);

        // 所持数
        UIExtensions.CheckToSetActiveGameObjectList(m_numTextMeshPro, m_numList);

    }



    // アイテム画像
    virtual protected void SetItemImage(bool _active = true)
    {
        if (m_itemImage == null) return;

        // 非表示
        m_itemImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;

        // 画像セット
        m_itemImage.sprite = m_itemData.ItemSprite;

        // 表示
        m_itemImage.gameObject.SetActive(true);

    }


    // アイテム種類テキスト
    virtual protected void SetTypeText(bool _active = true)
    {
        if (m_typeTextMeshPro == null) return;

        // 非表示
        m_typeTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;

        var data = ItemTypeDataBaseManager.instance.GetItemTypeData(m_itemData.ItemTypeID);
        if (data == null) return;


        // 名前テキストをセット
        m_typeTextMeshPro.text = data.ItemTypeName;

        // 表示
        m_typeTextMeshPro.gameObject.SetActive(true);

    }


    // アイテム種類画像
    virtual protected void SetTypeImage(bool _active = true)
    {
        if (m_typeImage == null) return;

        // 非表示
        m_typeImage.gameObject.SetActive(false);

        // 初期化用
        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;


        var data = ItemTypeDataBaseManager.instance.GetItemTypeData(m_itemData.ItemTypeID);
        if (data?.ItemTypeSprite == null) return;


        // 名前テキストをセット
        m_typeImage.sprite = data.ItemTypeSprite;

        // 表示
        m_typeImage.gameObject.SetActive(true);

    }


    // 最大保持数テキスト
    virtual protected void SetMaxNumText(bool _active = true)
    {
        if (m_maxNumText == null) return;

        // 非表示
        m_maxNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;

        // 名前テキストをセット
        m_maxNumText.text = m_itemData.MaxNum.ToString();

        // 表示
        m_maxNumText.gameObject.SetActive(true);

    }

    // アイテム名テキスト
    virtual protected void SetNameText(bool _active = true)
    {
        if (m_nameTextMeshPro == null) return;

        // 非表示
        m_nameTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;

        // 名前テキストをセット
        m_nameTextMeshPro.text = m_itemData.ItemName.GetLocalizedString();

        // 表示
        m_nameTextMeshPro.gameObject.SetActive(true);

    }



    // 説明文テキスト
    virtual protected void SetDescriptionText(bool _active = true)
    {
        if (m_descriptionTextMeshPro == null) return;

        // 非表示
        m_descriptionTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;

        // 説明文をセット
        m_descriptionTextMeshPro.text = m_itemData.ItemDescriptionText.GetLocalizedString();

        // 表示
        m_descriptionTextMeshPro.gameObject.SetActive(true);

    }



    // 総所持数テキスト
    virtual protected void SetNumText(bool _active = true)
    {
        if (m_numTextMeshPro == null) return;

        // 非表示
        m_numTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;

        var num = m_pocketType.GetPocketItemDataManager().GetItemNum(m_itemData.ItemTypeID, m_itemData.ItemID);


        // 0を表示しない設定であれば
        if (m_displayOneType == DisplayOneType.Disable)
        {
            if (num < 1) return;
        }

        // 保持数をセット
        m_numTextMeshPro.text = num.ToString();

        // 表示
        m_numTextMeshPro.gameObject.SetActive(true);

    }


    // レベルテキスト
    virtual protected void SetLevelText(bool _active = true)
    {
        if (m_levelTextMeshPro == null) return;

        // 非表示
        m_levelTextMeshPro.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;
        if (m_itemData.ItemLevel <= 0) return;

        // レベルをセット
        m_levelTextMeshPro.text = m_itemData.ItemLevel.ToString();

        // 表示
        m_levelTextMeshPro.gameObject.SetActive(true);
    }


    // 状態をセット
    virtual protected void SetConditionImage(bool _active = true)
    {
        if (m_conditionImage == null) return;

        // 非表示
        m_conditionImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;

        // 料理の場合
        switch (m_itemData.ItemTypeID)
        {
            case ItemTypeID.Food:
                {
                    // キャスト
                    var foodData = m_itemData as FoodData;
                    if (foodData == null) return;

                    // 回復用
                    if (foodData.IsFoodType(FoodData.FoodType.Heal))
                    {
                        var conditionData =
                            ConditionDataBaseManager.instance.GetConditionData(foodData.ConditionID);
                        if (conditionData == null) return;

                        m_conditionImage.sprite = conditionData.ConditionSprite;

                        m_conditionImage.gameObject.SetActive(true);
                        return;
                    }

                    // デバフ用
                    if (foodData.IsFoodType(FoodData.FoodType.DebuffCondition))
                    {
                        var conditionData =
                            ConditionDataBaseManager.instance.GetConditionData(foodData.ConditionID);
                        if (conditionData == null) return;

                        m_conditionImage.sprite = conditionData.ConditionSprite;

                        m_conditionImage.gameObject.SetActive(true);
                        return;
                    }

                    // 強化用
                    if (foodData.IsFoodType(FoodData.FoodType.StrengtheningStatus))
                    {
                        var addStatusData =
                            AddStatusTypeDataBaseManager.instance.GetData(foodData.AddPlayerStatus.AddType);
                        if (addStatusData == null) return;

                        m_conditionImage.sprite = addStatusData.IconSprite;

                        m_conditionImage.gameObject.SetActive(true);
                        return;
                    }
                    break;
                }


            case ItemTypeID.Ingredient:
                {
                    // キャスト
                    var ingredientData = m_itemData as IngredientData;
                    if (ingredientData == null) return;

                    var data = ConditionDataBaseManager.instance.GetConditionData(m_itemData.ConditionID);
                    if (data == null) return;

                    m_conditionImage.sprite = data.ConditionSprite;

                    m_conditionImage.gameObject.SetActive(true);
                    break;
                }

        }
    }


    /// <summary>
    /// 状態画像を作成
    /// </summary>
    virtual protected void SetCreateConditionImage(bool _active = true)
    {
        if (m_createConditionImage == null) return;

        // 非表示
        m_createConditionImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;


        // 画像を作成
        if (m_createConditionImage.CreateImage(m_itemData))
        {
            // 表示
            m_createConditionImage.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// 食材種類スロット作成
    /// </summary>
    private void CreateIngredientTypeSlotList(bool _active = true)
    {
        if (m_createIngredientTypeSlotList == null) return;

        // 初期化
        m_createIngredientTypeSlotList.DestroySlotList();
        if (_active == false) return;
        if (m_itemData == null) return;

        IngredientTypeID id = IngredientTypeID.None;

        // 料理の場合
        switch (m_itemData.ItemTypeID)
        {
            case ItemTypeID.Food:
                {
                    // キャスト
                    var foodData = m_itemData as FoodData;
                    if (foodData == null) return;

                    id = foodData.GetIngredientTypeID();

                    break;
                }


            case ItemTypeID.Ingredient:
                {
                    // キャスト
                    var ingredientData = m_itemData as IngredientData;
                    if (ingredientData == null) return;

                    id = ingredientData.IngredientTypeID;

                    break;
                }

        }

        m_createIngredientTypeSlotList.SetData(id);
        _ = m_createIngredientTypeSlotList.CreateSlot();
    }


    // 状態の背景色を変更（吉田）
    virtual protected void SetConditionBackColor(bool _active = true)
    {
        if (m_conditionBackColor == null) return;

        m_conditionBackColor.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_itemData == null) return;

        Color mainColor = Color.gray;
        Color subColor = Color.gray;


        switch (m_itemData.ItemTypeID)
        {
            case ItemTypeID.Food:
                {
                    var foodData = m_itemData as FoodData;
                    if (foodData == null) return;

                    // 回復用
                    if (foodData.IsFoodType(FoodData.FoodType.Heal))
                    {
                        var conditionData =
                            ConditionDataBaseManager.instance.GetConditionData(m_itemData.ConditionID);
                        if (conditionData == null) return;

                        // 色をセット(同色)
                        mainColor = conditionData.ConditionColor;
                        subColor = conditionData.ConditionColor;
                    }

                    // デバフ用
                    if (foodData.IsFoodType(FoodData.FoodType.DebuffCondition))
                    {
                        var conditionData =
                            ConditionDataBaseManager.instance.GetConditionData(m_itemData.ConditionID);
                        if (conditionData == null) return;

                        // 色をセット(同色)
                        mainColor = conditionData.ConditionColor;
                        subColor = conditionData.ConditionColor;
                    }

                    // 強化用
                    if (foodData.IsFoodType(FoodData.FoodType.StrengtheningStatus))
                    {
                        var addStatusData =
                        AddStatusTypeDataBaseManager.instance.GetData(foodData.AddPlayerStatus.AddType);
                        if (addStatusData == null) return;

                        // 色をセット
                        mainColor = addStatusData.MainColor;
                        subColor = addStatusData.SubColor;
                    }
                    break;
                }
            case ItemTypeID.Ingredient:
                {
                    var conditionData = ConditionDataBaseManager.instance.GetConditionData(m_itemData.ConditionID);
                    if (conditionData == null) return;

                    // 色をセット(同色)
                    mainColor = conditionData.ConditionColor;
                    subColor = conditionData.ConditionColor;
                    break;
                }
        }




        if (m_conditionBackColorGrad != null)
        {
            m_conditionBackColorGrad.SetColor( mainColor, subColor, m_conditionBackAlpha);
        }

        mainColor.a = m_conditionBackAlpha;
        m_conditionBackColor.color = mainColor;

        // 表示
        m_conditionBackColor.gameObject.SetActive(true);
    }


}

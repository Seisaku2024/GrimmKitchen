using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FoodInfo;
using NaughtyAttributes;
using ConditionInfo;

public partial class ItemDescription : MonoBehaviour
{
    // 制作者 田内
    // 料理説明文

    //==========================================================

    [Foldout("食料")]
    [Header("-------------------------------------------------------")]
    [Header("回復量Text")]
    [SerializeField]
    protected TextMeshProUGUI m_healingValueText = null;

    [Foldout("食料")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_healingValueTextList = new();


    [Foldout("料理")]
    [Header("-------------------------------------------------------")]
    [Header("価格テキスト")]
    [SerializeField]
    private TextMeshProUGUI m_priceText = null;

    [Foldout("料理")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_priceTextList = new();

    //=====================================================================

    [Foldout("料理")]
    [Header("-------------------------------------------------------")]
    [Header("提供可能数")]
    [SerializeField]
    private TextMeshProUGUI m_provideNumText = null;

    [Header("表示/非表示")]
    [SerializeField]
    private List<GameObject> m_provideNumTextList = null;

    //=====================================================================

    [Foldout("料理")]
    [Header("-------------------------------------------------------")]
    [Header("満足値テキスト")]
    [SerializeField]
    protected TextMeshProUGUI m_satisfactionValueText = null;

    [Foldout("料理")]
    [Header("表示/非表示用")]
    [SerializeField]
    protected List<GameObject> m_satisfactionValueTextList = new();

    //=====================================================================

    [Foldout("料理")]
    [Header("-------------------------------------------------------")]
    [Header("作成可能数テキスト")]
    [SerializeField]
    private TextMeshProUGUI m_createNumText = null;

    [Foldout("料理")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_createNumTextList = new();

    //=====================================================================

    [Foldout("料理")]
    [Header("-------------------------------------------------------")]
    [Header("必要材料スロットを表示するオブジェクト")]
    [SerializeField]
    private CreateNeedIngredientSlot m_createNeedIngredientSlot = null;

    //=====================================================================

    [Foldout("料理")]
    [Header("-------------------------------------------------------")]
    [Header("Food情報をUIに渡す")]// 吉田
    [SerializeField]
    private DetailByFoodTypeController m_detailByFoodTypeController = null;


    //=====================================================================
    //                      実行処理
    //=====================================================================


    /// <summary>
    /// 説明セット
    /// </summary>
    private void SetFoodDescription()
    {
        // キャストに失敗した場合初期化し終了
        if (m_itemData is FoodData == false)
        {
            InitilizeFoodDescription();
            return;
        }
        var foodData = m_itemData as FoodData;


        // 回復量テキストs
        SetHealingValueText(foodData);

        // 必要な材料スロットを作成
        CreateNeedIngredientSlot(foodData);

        // 価格テキスト
        SetPriceText(foodData);

        // 満足値
        SetSatisfactionValueText(foodData);

        // 作成可能数
        SetCreateNumText(foodData);

        // 提供可能数
        SetProvideNumText(foodData);

        // Foodの詳細
        SetDetailByFoodType(foodData);

    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void InitilizeFoodDescription()
    {
        SetHealingValueText(null, false);

        SetPriceText(null, false);

        SetSatisfactionValueText(null, false);

        SetCreateNumText(null, false);

        SetProvideNumText(null, false);

        SetDetailByFoodType(null, false);
    }

    /// <summary>
    /// アクティブセット
    /// </summary>
    private void SetFoodActiveList()
    {
        UIExtensions.CheckToSetActiveGameObjectList(m_priceText, m_priceTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_satisfactionValueText, m_satisfactionValueTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_createNumText, m_createNumTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_provideNumText, m_provideNumTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_healingValueText, m_healingValueTextList);
    }

    /// <summary>
    /// 回復量
    /// </summary>
    virtual protected void SetHealingValueText(FoodData _data, bool _active = true)
    {
        if (m_healingValueText == null) return;

        // 非表示
        m_healingValueText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_data == null) return;

        switch (_data.ConditionID)
        {
            case ConditionID.Normal:
                {
                    m_healingValueText.text = _data.HealingValue().ToString();
                    break;
                }
            default:
                {
                    m_healingValueText.text = "ー";
                    break;
                }
        }

        // 表示
        m_healingValueText.gameObject.SetActive(true);

    }

    /// <summary>
    /// 料理に必要なスロットを作成
    /// </summary>
    private void CreateNeedIngredientSlot(FoodData _itemData)
    {
        if (m_createNeedIngredientSlot == null) return;

        // 必要な材料を確認したい料理のIDをセットしてから作成
        m_createNeedIngredientSlot.SetFoodID((FoodID)_itemData.ItemID);
        _ = m_createNeedIngredientSlot.CreateSlot();

    }


    /// <summary>
    /// 値段
    /// </summary>
    private void SetPriceText(FoodData _data, bool _active = true)
    {
        if (m_priceText == null) return;

        // 非表示
        m_priceText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_data == null) return;

        m_priceText.text = _data.Price().ToString();

        // 表示
        m_priceText.gameObject.SetActive(true);
    }


    /// <summary>
    /// 満足値
    /// </summary>
    private void SetSatisfactionValueText(FoodData _data, bool _active = true)
    {
        if (m_satisfactionValueText == null) return;

        // 非表示
        m_satisfactionValueText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_data == null) return;

        m_satisfactionValueText.text = _data.SatisfactionValue().ToString();

        // 表示
        m_satisfactionValueText.gameObject.SetActive(true);

    }


    /// <summary>
    /// 作成可能数
    /// </summary>
    private void SetCreateNumText(FoodData _data, bool _active = true)
    {
        if (m_createNumText == null) return;

        // 非表示
        m_createNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_data == null) return;

        m_createNumText.text = FoodData.GetCreateNum(m_pocketType, (FoodID)_data.ItemID).ToString();

        // 表示
        m_createNumText.gameObject.SetActive(true);

    }


    /// <summary>
    /// 提供可能数
    /// </summary>
    private void SetProvideNumText(FoodData _data, bool _active = true)
    {
        if (m_provideNumText == null) return;

        // 非表示
        m_provideNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_data == null) return;

        m_provideNumText.text = FoodData.GetProvideNum(m_pocketType, (FoodID)_data.ItemID).ToString();

        // 表示
        m_provideNumText.gameObject.SetActive(true);

    }



    /// <summary>
    /// Foodの詳細を表示する
    /// </summary>
    // 吉田
    public void SetDetailByFoodType(FoodData _data, bool _active = true)
    {
        if (m_detailByFoodTypeController == null) return;
        m_detailByFoodTypeController.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (_data == null) return;

        // 表示
        m_detailByFoodTypeController.gameObject.SetActive(true);
        m_detailByFoodTypeController.SetFoodData(_data);

    }


}

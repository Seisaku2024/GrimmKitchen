using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using FoodInfo;
using PocketItemDataInfo;
using UniRx;
using UnityEngine.UI;

public class ProvideFoodRecipeSlotData : ItemSlotData
{
    // 制作者 田内
    // 提供料理用スロット

    [Header("=================================")]
    [Header("提供可能確認")]
    [SerializeField]
    private SetCanvasGroupAlpha m_setCanvasGroupAlpha = null;

    [Header("=================================")]
    [Header("提供料理セット時画像")]
    [SerializeField]
    private Image m_setProvideFoodImage = null;

    [Header("=================================")]
    [Header("高額料理画像")]
    [SerializeField]
    private Image m_businessConditionsImage = null;


    //=======================
    // 作成可能かどうか

    protected bool m_isProvide = false;

    public bool IsProvide { get { return m_isProvide; } }

    //============================================================================
    //                          実行処理
    //============================================================================


    override protected void Start()
    {
        base.Start();

        m_pocketType = ProvideFoodManager.instance.PocketType;

        // ポケットアイテムデータ変更イベントを受信
        ProvideFoodManager.instance.ProvideFoodIDRC.ObserveCountChanged().Subscribe(_ =>
        {
            // 問答無用で更新
            SetItemSlotData(m_itemData, m_pocketType);

        }).AddTo(this);

    }


    // スロットのデータをセットする
    public override void SetItemSlotData(BaseItemData _itemData, PocketType _pocketType)
    {
        base.SetItemSlotData(_itemData, _pocketType);

        // 提供可能か確認
        CheckProvide();

        // 提供料理セット画像
        SetProvideFoodImage();

        SetBusinessConditionsImage();

    }


    public override void InitializeSlotData()
    {
        base.InitializeSlotData();

        // 提供料理
        SetProvideFoodImage();

        // 提供可能か確認
        CheckProvide();

    }


    /// <summary>
    /// 提供料理セット画像
    /// </summary>
    private void SetProvideFoodImage()
    {
        if (m_setProvideFoodImage == null) return;

        if (ProvideFoodManager.instance.IsAddedProvideFood((FoodID)m_itemData.ItemID))
        {
            m_setProvideFoodImage.gameObject.SetActive(true);
        }
        else
        {
            m_setProvideFoodImage.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// 高額料理セット画像
    /// </summary>
    private void SetBusinessConditionsImage()
    {
        if (m_businessConditionsImage == null) return;

        if (m_itemData is FoodData == false) return;
        var foodData = m_itemData as FoodData;

        var challengeData = ChallengeDataBaseManager.instance.GetData(ChallengeManager.instance.ChallengeID);
        if (challengeData == null) return;

        var id = foodData.GetIngredientTypeID();

        if ((challengeData.BusinessConditionsIngredientTypeID & id) != 0)
        {
            m_businessConditionsImage.gameObject.SetActive(true);
        }
        else
        {
            m_businessConditionsImage.gameObject.SetActive(false);
        }

    }



    // 作成可能か確認
    virtual public void CheckProvide()
    {
        // 提供・選択可能かを確認
        if (FoodData.IsProvide(m_pocketType, (FoodID)m_itemData.ItemID) || ProvideFoodManager.instance.IsAddedProvideFood((FoodID)m_itemData.ItemID))
        {
            m_isProvide = true;
        }
        else
        {
            m_isProvide = false;
        }


        if (m_setCanvasGroupAlpha)
        {
            m_setCanvasGroupAlpha.CheckProvide(m_isProvide);
        }
    }

}

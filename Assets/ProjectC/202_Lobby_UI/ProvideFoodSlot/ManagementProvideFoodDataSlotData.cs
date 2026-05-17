using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using TMPro;
using UniRx;

public class ManagementProvideFoodDataSlotData : ItemSlotData
{
    // 制作者　田内
    // 提供料理スロット

    [Header("ボーナス")]
    [SerializeField]
    private TextMeshProUGUI m_bonusNumText = null;

    [Header("売り上げ数")]
    [SerializeField]
    private TextMeshProUGUI m_soldNumText = null;

    [Header("売上合計")]
    [SerializeField]
    private TextMeshProUGUI m_soldPriceText = null;

    [Header("売り切れリスト")]
    [SerializeField]
    private List<CanvasGroup> m_canvasGroupList = new();
    [SerializeField]
    private float m_minAlpha = 0.5f;
    [SerializeField]
    private float m_maxAlpha = 1.0f;

    //======================
    // 提供料理

    ManagementProvideFoodData m_provideFoodData = null;

    //====================================================
    //               実行処理
    //====================================================

    protected override void Start()
    {
        m_pocketType = ProvideFoodManager.instance.PocketType;

        base.Start();

        MessageBroker.Default.Receive<ManagementProvideFoodData.GlobalChangeProvideFoodDataEvent>().Subscribe(_ =>
        {
            if (_.ProvideFoodData == m_provideFoodData)
            {
                SetProvideFoodDataDescription();
            }

        }).AddTo(this);

    }


    public void SetProvideFoodData(ManagementProvideFoodData _data)
    {
        m_provideFoodData = _data;

        SetProvideFoodDataDescription();
    }


    override public void InitializeSlotData()
    {
        base.InitializeSlotData();

        SetBonusNumText(false);
        SetSoldNumText(false);
        SetSoldPriceText(false);
    }


    // 提供料理データを基に更新
    private void SetProvideFoodDataDescription()
    {
        SetBonusNumText();
        SetSoldNumText();
        SetSoldPriceText();
        SetSoldOutData();
    }



    private void SetBonusNumText(bool _active = true)
    {
        if (m_bonusNumText == null) return;

        // 非表示
        m_bonusNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_provideFoodData == null) return;

        m_bonusNumText.text = m_provideFoodData.BonusNum.ToString();

        // 表示
        m_bonusNumText.gameObject.SetActive(true);
    }


    private void SetSoldNumText(bool _active = true)
    {
        if (m_soldNumText == null) return;

        // 非表示
        m_soldNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_provideFoodData == null) return;

        m_soldNumText.text = m_provideFoodData.SoldNum.ToString();

        // 表示
        m_soldNumText.gameObject.SetActive(true);
    }

    private void SetSoldPriceText(bool _active = true)
    {
        if (m_soldPriceText == null) return;

        // 非表示
        m_soldPriceText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_provideFoodData == null) return;

        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)m_provideFoodData.FoodID);
        if (data == null) return;

        m_soldPriceText.text = (m_provideFoodData.SoldNum * data.Price()).ToString();

        // 表示
        m_soldPriceText.gameObject.SetActive(true);

    }

    // 売り切れ処理
    private void SetSoldOutData()
    {
        if (m_provideFoodData == null) return;
        if (m_provideFoodData.IsProvide())
        {
            foreach (var data in m_canvasGroupList)
            {
                if (data == null) continue;
                data.alpha = m_maxAlpha;
            }
        }
        else
        {
            foreach (var data in m_canvasGroupList)
            {
                if (data == null) continue;
                data.alpha = m_minAlpha;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FoodInfo;
using PocketItemDataInfo;
using UniRx;

public class RecipeItemSlotData : ItemSlotData
{
    // 料理が作成可能か確認などを行う
    // 作成者　田内

    [Header("作成可能確認")]
    [SerializeField]
    private SetCanvasGroupAlpha m_setCanvasGroupAlpha = null;

    //=======================
    // 作成可能かどうか

    protected bool m_isCreate = false;

    public bool IsCreate { get { return m_isCreate; } }

    //===========================================================
    //                       実行処理
    //===========================================================

    override protected void Start()
    {
        base.Start();

        // ポケットアイテムデータ変更イベントを受信
        MessageBroker.Default.Receive<GlobalChangePocketItemListEvent>().Subscribe(_ =>
        {
            // コントローラーが一致していれば問答無用で更新
            if (m_pocketType.GetPocketItemDataManager() == _.PocketItemDataController)
            {
                SetItemSlotData(m_itemData, m_pocketType);
            }

        }).AddTo(this);

    }

    // スロットのデータをセットする
    public override void SetItemSlotData(BaseItemData _itemData, PocketType _pocketType)
    {
        base.SetItemSlotData(_itemData, _pocketType);

        // 作成可能か確認
        CheckCreate();
    }


    public override void InitializeSlotData()
    {
        base.InitializeSlotData();

        // 作成可能か確認
        CheckCreate();
    }


    // 作成可能か確認
    virtual public void CheckCreate()
    {
        // 作成可能かを確認
        m_isCreate = FoodData.IsCreate(m_pocketType, (FoodID)m_itemData.ItemID);

        if (m_setCanvasGroupAlpha)
        {
            m_setCanvasGroupAlpha.CheckProvide(m_isCreate);
        }
    }


}

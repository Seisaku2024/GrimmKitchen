using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemInfo;
using FoodInfo;
using TMPro;
using NaughtyAttributes;
using UniRx;

public class CurrentSelectProvideFoodSlotData : ItemSlotData
{
    // 制作者　田内
    // 提供料理(確認)スロット

    [Header("レシピウィンドウのUI選択コントローラー")]
    [SerializeField]
    protected SelectUIController m_recipeSelectUIController = null;

    [Header("キャンバスグループ")]
    [SerializeField]
    protected CanvasGroup m_canvasGroup = null;

    [Header("表示時の透明度")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    protected float m_alpha = 0.5f;

    //===================================================

    [Foldout("Color")]
    [Header("色を変更したいUIList")]
    [SerializeField]
    private List<TextMeshProUGUI> m_colorTextList = new();

    [Foldout("Color")]
    [Header("作成可能状態のカラー")]
    [SerializeField]
    protected Color m_possibleColor = Color.white;

    [Foldout("Color")]
    [Header("作成不可能状態のカラー")]
    [SerializeField]
    protected Color m_inpossibleColor = Color.red;


    //============================================================================
    //                              実行処理
    //============================================================================


    override protected void Start()
    {
        base.Start();

        m_pocketType = ProvideFoodManager.instance.PocketType;
    }

    public void OnUpdate()
    {
        #region nullチェック
        if (m_recipeSelectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーがシリアライズされていません");
            return;
        }
        #endregion

        if (m_recipeSelectUIController.IsPress == true || m_recipeSelectUIController.IsSelectChangeFlg == true)
        {
            SetSlot();
        }
    }


    private void SetSlot()
    {
        #region nullチェック
        if (m_recipeSelectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーがシリアライズされていません");
            return;
        }
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }
        #endregion

        // 一度非表示
        m_canvasGroup.alpha = 0.0f;

        // 追加できなければ
        if (ProvideFoodManager.instance.IsAddList() == false) return;

        // 選択中のUIから取得
        if (m_recipeSelectUIController.CurrentSelectUI.TryGetComponent<RecipeItemSlotData>(out var itemSlotData))
        {
            // 提供料理に既に追加されていれば
            if (ProvideFoodManager.instance.IsAddedProvideFood((FoodID)itemSlotData.ItemData.ItemID) == true) return;

            // 表示
            m_canvasGroup.alpha = m_alpha;

            // 更新する
            var itemData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Food, itemSlotData.ItemData.ItemID);
            SetItemSlotData(itemData, m_pocketType);

            // 色をセット
            SetColor(itemSlotData);
        }
    }


    // 色要素を変更
    private void SetColor(RecipeItemSlotData _slotData)
    {

        if (_slotData.IsCreate == false)
        {
            // 作成できなければ
            foreach (var ui in m_colorTextList)
            {
                ui.color = m_inpossibleColor;
            }
        }
        else
        {
            // 作成できれば
            foreach (var ui in m_colorTextList)
            {
                ui.color = m_possibleColor;
            }
        }
    }

}

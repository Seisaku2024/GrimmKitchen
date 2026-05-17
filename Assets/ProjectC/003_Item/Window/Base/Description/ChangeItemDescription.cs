using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using UniRx;

[RequireComponent(typeof(ItemDescription))]
public partial class ChangeItemDescription : MonoBehaviour
{
    // 制作者 田内
    // チャレンジの説明文を表示


    //============================================
    [Header("コントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;

    //============================================
    [Header("ポケットマネージャーの種類")]
    [SerializeField]
    protected PocketType m_pocketType = PocketType.Inventory;

    //========================================================
    // 説明文
    protected ItemDescription m_itemDescription = null;

    //============================================
    // 選択中のアイテムデータ
    protected BaseItemData m_itemData = null;

    //============================================
    // 選択中のポケットアイテムデータ
    protected PocketItemData m_pocketItemData = null;


    //==============================================
    //              実行処理
    //==============================================

    virtual protected void Start()
    {
        // ポケットアイテムデータ変更イベントを受信
        MessageBroker.Default.Receive<GlobalChangePocketItemListEvent>().Subscribe(_ =>
        {
            if (m_pocketType.GetPocketItemDataManager() == _.PocketItemDataController)
            {
                // 現在選択中のアイテム説明文を更新
                SetDescription();
            }
        }).AddTo(this);
    }



    /// <summary>
    /// 初期化処理
    /// </summary>
    virtual public void OnInitialize()
    {
        // 初期化
        SetDescription();
    }


    /// <summary>
    /// 実行処理
    /// </summary>
    virtual public void OnUpdate()
    {
        if (IsChangeDescription())
        {
            SetDescription();
        }
    }


    // 説明文を変更できるか確認
    protected bool IsChangeDescription()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return false;
        }
        #endregion

        return m_selectUIController.IsSelectChangeFlg;
    }



    virtual protected void SetDescription()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }

        m_itemData = null;
        m_pocketItemData = null;

        var data = m_selectUIController.CurrentSelectUI;
        if (data != null)
        {
            if (data.TryGetComponent<PocketItemSlotData>(out var pocketSlotData) == true)
            {
                m_itemData = pocketSlotData.ItemData;
                m_pocketItemData = pocketSlotData.PocketItemData;
            }
            else if (data.TryGetComponent<ItemSlotData>(out var itemSlotData) == true)
            {
                m_itemData = itemSlotData.ItemData;
            }
        }

        // 説明文を更新
        if (m_itemDescription == null) m_itemDescription = gameObject.GetComponent<ItemDescription>();
        m_itemDescription.UpdateDescription(m_itemData, m_pocketType, m_pocketItemData);

    }

}

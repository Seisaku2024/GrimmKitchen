using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

using ItemInfo;
using FoodInfo;
using PocketItemDataInfo;

using NaughtyAttributes;

public class CreateFoodWindow : BaseWindow
{
    // 制作者 田内
    // 料理を作成するウィンドウ

    [Header("説明文")]
    [SerializeField]
    private ItemDescription m_itemDescription = null;

    [Header("料理作成コントローラー")]
    [SerializeField]
    private CreateFoodController m_createFoodController = null;

    [Header("料理作成コントローラーの説明文")]
    [SerializeField]
    private ChangeValueControllerDescription m_changeCreateFoodControllerDescription = null;

    [Header("操作する料理ID")]
    [SerializeField]
    private FoodID m_foodID = FoodID.Omelette;

    [Header("ポケットタイプ")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;

    [Header("テキストのコピーをするコンポーネント")]
    [SerializeField]
    private WindowUpdateBase m_copyText = null;

    //==========================================
    //              実行処理
    //==========================================


    /// <summary>
    /// 作成する料理のデータをセットする
    /// </summary>
    public void SetFoodData(PocketType _pocketType, FoodID _id)
    {
        m_pocketType = _pocketType;
        m_foodID = _id;
    }


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_itemDescription == null)
        {
            Debug.LogError("ItemDescriptionがシリアライズされていません");
            return;
        }
        if (m_createFoodController == null)
        {
            Debug.LogError("CreateFoodControllerがシリアライズされていません");
            return;
        }
        if (m_changeCreateFoodControllerDescription == null)
        {
            Debug.LogError("ChangeCreateFoodControllerDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // コントローラー更新
            m_createFoodController.SetData(m_pocketType, m_foodID);

            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 説明文更新
            m_itemDescription.UpdateDescription(ItemTypeID.Food, (uint)m_foodID, m_pocketType);

            // コントローラーの説明文初期化
            m_changeCreateFoodControllerDescription.OnInitialize();

            m_copyText.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_itemDescription == null)
        {
            Debug.LogError("ChangeItemDescriptionがシリアライズされていません");
            return;
        }
        if (m_createFoodController == null)
        {
            Debug.LogError("CreateFoodControllerがシリアライズされていません");
            return;
        }
        if (m_changeCreateFoodControllerDescription == null)
        {
            Debug.LogError("ChangeCreateFoodControllerDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // コントローラーの更新
                await m_createFoodController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // 説明文を更新
                if (m_createFoodController.IsSelectChangeFlg)
                {
                    m_itemDescription.UpdateDescription(ItemTypeID.Food, (uint)m_foodID, m_pocketType);
                    m_changeCreateFoodControllerDescription.OnUpdate();
                }

                m_copyText.OnUpdate();

                // コントローラーの後処理
                m_createFoodController.OnLateUpdate();

                // ウィンドウを閉じる
                if (IsClose()) return;

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }
}

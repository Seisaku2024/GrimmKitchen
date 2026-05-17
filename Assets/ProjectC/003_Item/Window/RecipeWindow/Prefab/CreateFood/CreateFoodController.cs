using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using FoodInfo;
using NaughtyAttributes;
using PocketItemDataInfo;
using ItemInfo;

public class CreateFoodController : ValueController
{
    // 制作者 田内
    // 料理を作成するコントローラー

    [Header("操作する料理ID")]
    [SerializeField]
    private FoodID m_foodID = FoodID.Omelette;

    [Header("ポケット種類")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;
    public PocketType PocketType
    {
        get { return m_pocketType; }
    }


    [BoxGroup("Window")]
    [Header("作成料理確認ウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_confirmationItemWindowController = null;


    //==============================================
    //              実行処理
    //==============================================

    /// <summary>
    /// 作成する料理のデータをセットする
    /// </summary>
    public void SetData(PocketType _pocketType, FoodID _id)
    {
        m_pocketType = _pocketType;
        m_foodID = _id;

        SetData();
    }

    override protected void SetData()
    {
        // 作成可能数
        int num = FoodData.GetCreateNum(m_pocketType, m_foodID);

        // 最小作成数を更新
        if (num <= 0) m_minValue = 0;
        else m_minValue = 1;

        // 最大作成数を更新
        m_maxValue = num;

        // 現在選択中の値を更新
        if (m_maxValue < m_currentValue) m_currentValue = m_maxValue;
        if (m_currentValue < m_minValue) m_currentValue = m_minValue;

        // スライダーの値更新
        SetSliderValue();

        m_isSelectChangeFlg = true;
    }

    /// <summary>
    /// 実行処理
    /// </summary>
    override public async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        // 作成料理確認ウィンドウを作成
        try
        {
            await base.OnUpdate();
            cancelToken.ThrowIfCancellationRequested();

            // 料理を作成する
            await CreateFood();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    /// <summary>
    /// 料理が作成可能か
    /// </summary>
    public override bool IsDecision()
    {
        // 選択可能数を超えていれば
        if (m_pocketType.GetPocketItemDataManager().IsInList(ItemTypeID.Food, (uint)m_foodID, m_currentValue) == false) return false;

        // 作成可能かどうか
        if (FoodData.IsCreate(m_pocketType, m_foodID) == false) return false;

        return true;
    }


    // 料理を作成する
    private async UniTask CreateFood()
    {
        #region nullチェック
        if (m_decisionInputActionButton == null)
        {
            Debug.LogError("DecisionInputActionButtonがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_decisionInputActionButton.IsInputActionTrriger())
            {
                if (IsDecision() == false) return;

                for (int i = 0; i < m_currentValue; ++i)
                {
                    // 料理を作成(素材は使用する)
                    FoodData.CreateFood(m_pocketType, m_foodID, true);
                }

                // 作成料理確認ウィンドウを作成
                await CreateConfirmationItemWindow();
                cancelToken.ThrowIfCancellationRequested();

                // 初期化
                SetData();
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // 作成料理確認ウィンドウを作成
    private async UniTask CreateConfirmationItemWindow()
    {
        #region nullチェック
        if (m_confirmationItemWindowController == null)
        {
            Debug.LogError("ConfirmationItemWindowControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        // 作成料理確認ウィンドウを作成
        try
        {
            var controller = Instantiate(m_confirmationItemWindowController);

            await controller.CreateWindow<ConfirmationItemWindow>(onBeforeInitialize: async _ =>
            {
                _.SetDescription(m_pocketType, ItemTypeID.Food, (uint)m_foodID);
                await UniTask.CompletedTask;
            });
            cancelToken.ThrowIfCancellationRequested();

            if (controller != null) Destroy(controller.gameObject);

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using Cysharp.Threading.Tasks;

public class MovePocketItemValueController : PocketItemValueController
{
    // 制作者 田内
    // 移動コントローラー

    [Header("移動させるポケット")]
    [SerializeField]
    private PocketType m_movePocketType = PocketType.Inventory;

    //======================================
    //          実行処理
    //======================================

    /// <summary>
    /// 実行処理
    /// </summary>
    override public async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnUpdate();
            cancelToken.ThrowIfCancellationRequested();

            // アイテムを移動
            await MoveItem();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    public override bool IsDecision()
    {
        if (m_pocketItemData == null) return false;

        if (m_currentValue <= 0 || m_currentValue < m_minValue || m_maxValue < m_currentValue) return false;

        if (m_movePocketType.GetPocketItemDataManager().IsInList(m_pocketItemData.ItemTypeID, m_pocketItemData.ItemID, m_currentValue) == false) return false;

        return true;
    }

    // 料理を作成する
    private async UniTask MoveItem()
    {
        #region nullチェック
        if (m_decisionInputActionButton == null)
        {
            Debug.LogError("DecisionInputActionButtonがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        // 移動
        try
        {
            if (m_decisionInputActionButton.IsInputActionTrriger())
            {
                // 選択可能でなければ
                if (IsDecision() == false) return;

                for (int i = 0; i < m_currentValue; ++i)
                {
                    // 追加
                    if (m_movePocketType.GetPocketItemDataManager().AddItem(m_pocketItemData.ItemTypeID, m_pocketItemData.ItemID))
                    {
                        // 取り除き
                        m_pocketType.GetPocketItemDataManager().RemoveItem(m_pocketItemData);
                    }
                }

                // 初期化
                SetData();
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }

}

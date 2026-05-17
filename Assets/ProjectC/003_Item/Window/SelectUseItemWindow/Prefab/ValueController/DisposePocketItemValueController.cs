using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using Cysharp.Threading.Tasks;

public class DisposePocketItemValueController : PocketItemValueController
{
    // 制作者 田内
    // 移動コントローラー

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

            // アイテムを削除
            await Dispose();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    // 料理を作成する
    private async UniTask Dispose()
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
                    // 取り除き
                    m_pocketType.GetPocketItemDataManager().RemoveItem(m_pocketItemData);
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

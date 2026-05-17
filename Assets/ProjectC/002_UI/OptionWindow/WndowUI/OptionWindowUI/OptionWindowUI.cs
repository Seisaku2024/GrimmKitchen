using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class OptionWindowUI : BaseWindowUI
{
    // 制作者 田内
    // オプションUI

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    //=================================================
    //                  実行処理
    //=================================================


    override public async UniTask OnSelectUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("選択コントローラーがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 処理
            await m_selectUIController.OnUpdate();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }


    override public async UniTask OnLateUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("選択コントローラーがシリアライズされていません");
            return;
        }
        #endregion

        // 後処理
        m_selectUIController.OnLateUpdate();

        await UniTask.CompletedTask;
    }


}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyResultWindowUI : BaseWindowUI
{
    // 制作者 田内
    // お金リザルト

    [Header("説明文")]
    [SerializeField]
    private ChangeResultDescription m_changeProvideFoodDescription = null;

    //==============================================
    //                  実行処理
    //==============================================


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_changeProvideFoodDescription == null)
        {
            Debug.LogError("ChangeProvideFoodDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 説明文を初期化
            await m_changeProvideFoodDescription.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }


    }

}

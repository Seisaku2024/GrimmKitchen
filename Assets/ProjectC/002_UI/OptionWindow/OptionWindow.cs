using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionWindow : BaseWindow
{
    // 制作者 田内
    // オプションウィンドウ


    [Header("ウィンドウUIコントローラー")]
    [SerializeField]
    private WindowUIController m_windowUIController = null;

    //============================================================
    //                      実行処理
    //============================================================


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_windowUIController == null)
        {
            Debug.LogError("WindowUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // UI初期化
            await m_windowUIController.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_windowUIController == null)
        {
            Debug.LogError("WindowUIコントローラーがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();


                // ウィンドウUI処理
                await m_windowUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // ウィンドウUI後処理
                await m_windowUIController.OnLateUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // ウィンドウを閉じる
                if (IsClose())
                {
                    return;
                }

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

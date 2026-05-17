using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItemWindow : BaseWindow
{
    // 制作者 田内
    // アイテムの移動を行うウィンドウ


    [Header("UIコントローラー")]
    [SerializeField]
    private WindowUIController m_windowUIController = null;

    //==================================================
    //                  実行処理
    //==================================================

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_windowUIController == null)
        {
            Debug.LogError("WindouUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            await m_windowUIController.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }


    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_windowUIController == null)
        {
            Debug.LogError("WindouUIControllerがシリアライズされていません");
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

                await m_windowUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                await m_windowUIController.OnLateUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // 閉じる
                if (IsClose()) return;

                await UniTask.DelayFrame(1);

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

}

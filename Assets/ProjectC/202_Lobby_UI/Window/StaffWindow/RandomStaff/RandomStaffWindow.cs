using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class RandomStaffWindow : BaseWindow
{
    // 制作者 田内
    // ランダムでスタッフを取得するウィンドウ

    [Header("ランダムスタッフコントローラー")]
    [SerializeField]
    private RandomStaffController m_randomStaffController = null;

    [Header("説明文")]
    [SerializeField]
    private ChangeValueControllerDescription m_changeValueControllerDescription = null;

    // 追加（吉田）
    [Header("更新処理が必要なスクリプト")]
    [SerializeField]
    private List<WindowUpdateBase> m_windowUpdateBaseList = new();


    //=======================================================================
    //                          実行処理
    //=======================================================================

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if(m_changeValueControllerDescription==null)
        {
            Debug.LogError("ChangeValueControllerDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            m_changeValueControllerDescription.OnInitialize();

            // 追加（吉田）
            foreach (var window in m_windowUpdateBaseList)
            {
                window.OnInitialize();
            }

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_randomStaffController == null)
        {
            Debug.LogError("RandomStaffControllerがシリアライズされていません");
            return;
        }
        if(m_changeValueControllerDescription==null)
        {
            Debug.LogError("ChangeValueControllerDescriptionがシリアライズされていません");
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


                // ランダムスタッフコントローラーを更新
                await m_randomStaffController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                m_changeValueControllerDescription.OnUpdate();

                // 追加（吉田）
                foreach (var window in m_windowUpdateBaseList)
                {
                    window.OnUpdate();
                }

                // ランダムスタッフコントローラーを更新
                m_randomStaffController.OnLateUpdate();

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

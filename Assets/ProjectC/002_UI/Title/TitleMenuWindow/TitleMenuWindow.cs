using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ButtonInfo;

public class TitleMenuWindow : BaseWindow
{
    // 制作者 田内
    // 値を操作するコントローラーウィンドウ

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("NewGame")]
    [SerializeField]
    private TitleNewGameController m_titleNewGameController = null;

    [Header("Continue")]
    [SerializeField]
    private TitleContinueController m_titleContinueController = null;

    [Header("Option")]
    [SerializeField]
    private TitleOptionController m_titleOptionController = null;

    [Header("Exit")]
    [SerializeField]
    private TitleExitController m_titleExitController = null;

    [Header("Credit")]
    [SerializeField]
    private TitleCreditController m_titleCreditController = null;

    //========================================
    //              実行処理
    //========================================

    public override async UniTask OnInitialize()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    override public async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
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


                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                await Select();
                cancelToken.ThrowIfCancellationRequested();

                m_selectUIController.OnLateUpdate();

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    private async UniTask Select()
    {
        if (m_selectUIController == null) return;


        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // ボタン取得
            var id = m_selectUIController.IsPressButton();
            if (id == ButtonID.None) return;

            switch (id)
            {
                case ButtonID.TitleNewGame:
                    {
                        if (m_titleNewGameController == null)
                        {
                            Debug.LogError("TitleNewGameControllerがシリアライズされていません");
                        }

                        await m_titleNewGameController.OnUpdate();
                        cancelToken.ThrowIfCancellationRequested();

                        break;
                    }

                case ButtonID.TitleContinue:
                    {
                        if (m_titleContinueController == null)
                        {
                            Debug.LogError("TitleContinueControllerがシリアライズされていません");
                        }

                        await m_titleContinueController.OnUpdate();
                        cancelToken.ThrowIfCancellationRequested();

                        break;
                    }

                case ButtonID.TitleOption:
                    {
                        if (m_titleOptionController == null)
                        {
                            Debug.LogError("TitleOptionControllerがシリアライズされていません");
                        }

                        await m_titleOptionController.OnUpdate();
                        cancelToken.ThrowIfCancellationRequested();

                        break;
                    }

                case ButtonID.TitleCredit:
                    {
                        if (m_titleCreditController == null)
                        {
                            Debug.LogError("TitleCreditControllerがシリアライズされていません");
                        }

                        await m_titleCreditController.OnUpdate();
                        cancelToken.ThrowIfCancellationRequested();

                        break;
                    }

                case ButtonID.TitleExit:
                    {
                        if (m_titleExitController == null)
                        {
                            Debug.LogError("TitleExitControllerがシリアライズされていません");
                        }

                        await m_titleExitController.OnUpdate();
                        cancelToken.ThrowIfCancellationRequested();

                        break;
                    }

                default:
                    {
                        // 対応ボタンではありませんでした
                        break;
                    }

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }


        await UniTask.CompletedTask;

    }


}

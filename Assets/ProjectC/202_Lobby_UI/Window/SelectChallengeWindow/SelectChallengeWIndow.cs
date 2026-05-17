using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectChallengeWIndow : BaseWindow
{
    // 制作者 田内
    // チャレンジウィンドウ


    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    [Header("チャレンジ選択コントローラー")]
    [SerializeField]
    private SelectChallengeController m_selectChallengeController = null;


    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("説明文")]
    [SerializeField]
    private ChangeChallengeDescription m_changeChallengeDescription = null;

    [Header("スロット作成")]
    [SerializeField]
    private CreateChallengeSlotList m_createChallengeSlotList = null;

    //=====================================================
    //                  実行処理
    //=====================================================

    public override async UniTask OnInitialize()
    {
        #region nullチェック

        if (m_createChallengeSlotList == null)
        {
            Debug.LogError("CreateChallengeSlotListがシリアライズされていません");
            return;
        }

        if (m_changeChallengeDescription == null)
        {
            Debug.LogError("ChangeChallengeDescriptionがシリアライズされていません");
            return;
        }

        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 初期化
            await m_createChallengeSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 説明文
            m_changeChallengeDescription.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    public override async UniTask OnUpdate()
    {
        #region nullチェック

        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPositionがシリアライズされていません");
            return;
        }

        if (m_selectChallengeController == null)
        {
            Debug.LogError("SelectChallengeControllerがシリアライズされていません");
            return;
        }

        if (m_changeChallengeDescription == null)
        {
            Debug.LogError("ChangeChallengeDescriptionがシリアライズされていません");
            return;
        }

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

                // UI選択更新
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // チャレンジ選択
                await m_selectChallengeController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // スクロール
                m_changeScrollViewPosition.OnUpdate();

                // 説明文更新
                m_changeChallengeDescription.OnUpdate();


                // UI選択後更新
                m_selectUIController.OnLateUpdate();


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

    // ウィンドウを閉じるか確認する
    override protected bool IsClose()
    {
        if (m_selectChallengeController != null)
        {
            if (m_selectChallengeController.IsClose())
            {
                return true;
            }
        }

        return base.IsClose();
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodInfo;
using Cysharp.Threading.Tasks;

public class SelectProvideFoodUI : BaseProvideFoodUI
{

    // 制作者　田内
    // 提供する料理を選択するUI

    [Header("スロット作成")]
    [SerializeField]
    private CreateRecipeSlotList m_createRecipeSlotList = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("提供料理選択コントローラー")]
    [SerializeField]
    private SelectProvideFoodController m_selectProvideFoodController = null;

    //===========================================
    //              実行処理
    //===========================================

    public override async UniTask OnInitialize()
    {
        if (m_createRecipeSlotList == null)
        {
            Debug.LogError("スロット作成が存在しません");
        }

        var cancelToken = this.destroyCancellationToken;

        try
        {
            await m_createRecipeSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }


    override public async UniTask OnSelectUpdate()
    {
        #region nullチェック

        if (m_selectUIController == null)
        {
            Debug.LogError("選択コントローラーがシリアライズされていません");
            return;
        }
        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPosコンポーネントがアタッチされていません");
            return;
        }
        if(m_selectProvideFoodController==null)
        {
            Debug.LogError("SelectProvideFoodControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // UI選択更新
            await m_selectUIController.OnUpdate();
            cancelToken.ThrowIfCancellationRequested();

            // スクロールビューの更新
            m_changeScrollViewPosition.OnUpdate();

            // 料理を選択
            m_selectProvideFoodController.OnUpdate();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }



}

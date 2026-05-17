using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using ButtonInfo;

public class SelectProvideFoodMenuUI : BaseProvideFoodUI
{

    // 制作者　田内
    // 提供する料理の提供数を選択するUI

    [Header("提供料理スロット作成")]
    [SerializeField]
    private CreateProvideFoodSlotList m_createManagementProvideFoodSlotList = null;

    [Header("選択中の提供料理スロット")]
    [SerializeField]
    private CurrentSelectProvideFoodSlotData m_selectProvideFoodSlotData = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    //==========================================================
    //                     実行処理
    //==========================================================

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_createManagementProvideFoodSlotList == null)
        {
            Debug.LogError("提供料理作成スクリプトがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.destroyCancellationToken;

        try
        {
            await m_createManagementProvideFoodSlotList.OnInitialize();
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
        if (m_selectProvideFoodSlotData == null)
        {
            Debug.LogError("SelectProvideFoodSlotDataがシリアライズされていません");
            return;
        }
        #endregion

        // 存在しないUIを取り除く
        m_selectUIController.NullCheck();

        // スクロールビュー更新
        m_changeScrollViewPosition.OnUpdate();

        // 選択中料理表示スロット更新
        m_selectProvideFoodSlotData.OnUpdate();

        await UniTask.CompletedTask;
    }


    public override async UniTask OnSelectInitialize()
    {
        #region nullチェック
        if (m_selectProvideFoodSlotData == null)
        {
            Debug.LogError("m_selectProvideFoodSlotDataがシリアライズされていません");
            return;
        }
        #endregion

        // 選択中料理表示スロットのアクティブを停止
        m_selectProvideFoodSlotData.gameObject.SetActive(false);

        await UniTask.CompletedTask;
    }

    public override async UniTask OnSelectUpdate()
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
            // UIの選択更新
            await m_selectUIController.OnUpdate();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch(System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }


    public override async UniTask OnSelectExit()
    {
        #region nullチェック
        if (m_selectProvideFoodSlotData == null)
        {
            Debug.LogError("m_selectProvideFoodSlotDataがシリアライズされていません");
            return;
        }
        #endregion

        // 選択中料理表示スロットのアクティブを開始
        m_selectProvideFoodSlotData.gameObject.SetActive(true);

        await UniTask.CompletedTask;
    }


}

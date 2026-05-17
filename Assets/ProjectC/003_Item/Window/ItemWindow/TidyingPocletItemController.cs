using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ItemInfo;
using PocketItemDataInfo;

public class TidyingPocletItemController : MonoBehaviour
{
    // 制作者　田内
    // 整頓コントローラー

    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;

    [SerializeField]
    private InputActionButton m_inputActionButton = null;

    [SerializeField]
    private GameObject m_gameObject = null;

    //======================================
    //              実行処理
    //======================================

    /// <summary>
    /// 実行処理
    /// </summary>
    public async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await Tidying();
            cancelToken.ThrowIfCancellationRequested();

            await UniTask.CompletedTask;
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // 整頓
    private async UniTask Tidying()
    {
        #region nullチェック
        if (m_inputActionButton == null)
        {
            Debug.LogError("InputActionButtonがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_inputActionButton.IsInputActionTrriger())
            {
                await m_pocketType.GetPocketItemDataManager().Tidying();
                cancelToken.ThrowIfCancellationRequested();

                // 画像作成
                if (m_gameObject != null)
                {
                    Instantiate(m_gameObject);
                }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}

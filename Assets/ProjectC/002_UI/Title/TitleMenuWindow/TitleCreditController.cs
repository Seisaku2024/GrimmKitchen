using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCreditController : MonoBehaviour
{
    // クレジットを開く

    [Header("クレジットウィンドウ")]
    [SerializeField]
    private WindowController m_creditWindowController = null;

    //========================================
    //              実行処理
    //========================================

    public async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await Create();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }

    private async UniTask Create()
    {
        if (m_creditWindowController == null)
        {
            Debug.LogError("CreditWindowControllerがシリアライズされていません");
            return;
        }

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            var controller = Instantiate(m_creditWindowController);
            await controller.CreateWindow();
            cancelToken.ThrowIfCancellationRequested();
            if (controller != null) Destroy(controller.gameObject);
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }
}

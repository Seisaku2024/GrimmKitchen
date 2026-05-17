using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleOptionController : MonoBehaviour
{
    // タイトル オプションを開く

    [Header("オプションウィンドウ")]
    [SerializeField]
    private WindowController m_optionWindowController = null;

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
        if (m_optionWindowController == null)
        {
            Debug.LogError("OptionWindowControllerがシリアライズされていません");
            return;
        }

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            var controller = Instantiate(m_optionWindowController);
            await controller.CreateWindow();
            cancelToken.ThrowIfCancellationRequested();
            if (controller != null) Destroy(controller.gameObject);
        }
        catch(System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

}

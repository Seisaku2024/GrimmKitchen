using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleContinueController : MonoBehaviour
{
    // タイトル 続きから開始する

    [Header("確認ウィンドウ")]
    [SerializeField]
    private WindowController m_checkWindowController = null;


    [Header("シーン移動")]
    [SerializeField]
    private SceneTransitionManager m_sceneChanger = null;

    //========================================
    //              実行処理
    //========================================

    public async UniTask OnUpdate()
    {
        try
        {
            if (m_sceneChanger == null)
            {
                Debug.LogError("SceneChangerがシリアライズされていません");
                return;
            }

            // セーブデータが存在しなければ
            if (SaveManager.instance.IsSave() == false)
            {
                await Create();
                return;
            }

            _ = m_sceneChanger.SceneChange();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }

    private async UniTask Create()
    {
        if (m_checkWindowController == null)
        {
            Debug.LogError("CheckWindowControllerがシリアライズされていません");
            return;
        }

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            var controller = Instantiate(m_checkWindowController);
            await controller.CreateWindow();
            cancelToken.ThrowIfCancellationRequested();
            if (controller != null) Destroy(controller);

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}

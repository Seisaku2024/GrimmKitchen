using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleNewGameController : MonoBehaviour
{
    // タイトル 新しくゲームを開始する

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
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_sceneChanger == null)
            {
                Debug.LogError("SceneChangerがシリアライズされていません");
                return;
            }

            // セーブデータが存在すれば確認表示
            if (SaveManager.instance.IsSave())
            {
                if (await Create() == false) return;
            }

            // セーブデータを削除する
            await SaveManager.instance.AllDelete();
            cancelToken.ThrowIfCancellationRequested();

            _ = m_sceneChanger.SceneChange();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }

    private async UniTask<bool> Create()
    {
        if (m_checkWindowController == null)
        {
            Debug.LogError("CheckWindowControllerがシリアライズされていません");
            return false;
        }

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            var controller = Instantiate(m_checkWindowController);
            var window = await controller.CreateWindow<JudgeWindow>(_bSelef: true);
            cancelToken.ThrowIfCancellationRequested();

            bool judge = await window.OnSelfUpdate();
            cancelToken.ThrowIfCancellationRequested();

            await window.OnClose();
            cancelToken.ThrowIfCancellationRequested();

            await window.OnDestroy();
            cancelToken.ThrowIfCancellationRequested();

            if (controller != null) Destroy(controller.gameObject);

            return judge;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        return false;
    }

}

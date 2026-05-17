using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using ButtonInfo;

public class SelectStageWindow : BaseWindow
{
    // ステージ選択ウィンドウ
    // 制作者　田内

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("ステージ選択コントローラー")]
    [SerializeField]
    private SelectStageController m_selectStageController = null;


    [Header("難易度選択ウィンドウ")]
    [SerializeField]
    private WindowController m_selectDifficultyWindowController = null;

    //======================================================
    //                  実行処理
    //======================================================


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

    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        if (m_selectStageController == null)
        {
            Debug.LogError("SelectStageControllerがシリアライズされていません");
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

                // UI選択の更新
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();


                // シーン遷移
                if (await Transion()) return;
                cancelToken.ThrowIfCancellationRequested();

                // UI選択の更新
                m_selectUIController.OnLateUpdate();


                // ウィンドウを閉じる
                if (IsClose()) return;

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();


            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    // シーン遷移を行う
    // 遷移できればtrueを返す
    private async UniTask<bool> Transion()
    {
        #region nullチェック
        if (m_selectStageController == null)
        {
            Debug.LogError("SelectStageControllerがシリアライズされていません");
            return true;
        }

        #endregion

        try
        {

            if (m_selectStageController.IsStart())
            {
                // 追加：次ステージを保存後　伊波
                StageEnemyDifficultyLevel stageEnemyDifficultyLevel = BaseManager<StageEnemyDifficultyLevel>.instance;
                if (!stageEnemyDifficultyLevel)
                {
                    Debug.LogError("StageEnemyDifficultyLevelが見つかりませんでした");
                }

                // ステージデータを取得
                var data = StageDataBaseManager.instance.GetStageData(m_selectStageController.CurrentSelectStageID);
                if (data == null) return false;

                stageEnemyDifficultyLevel.SetStageData(data);

                // 追加：難易度選択画面へ移行　伊波
                if (m_selectDifficultyWindowController == null)
                {
                    Debug.LogError("SelectDifficultyWindowControllerがシリアライズされていません");
                    return false;
                }
                var cancelToken = this.destroyCancellationToken;
                var controller = Instantiate(m_selectDifficultyWindowController);
                await controller.CreateWindow<BaseWindow>();
                cancelToken.ThrowIfCancellationRequested();

                // 難易度選択画面破棄→ステージ選択画面に戻る
                if (controller != null) Destroy(controller.gameObject);
                return false;


                //var data = m_selectStageController.GetCurrentSelectStageData();
                //m_sceneTransitionManager.m_sceneName = data.SceneName;
                //await m_sceneTransitionManager.SceneChange();

                //return true;
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        return false;
    }



}

using System.Collections.Generic;
using UnityEngine;

using ButtonInfo;

using Cysharp.Threading.Tasks;


// ステージの難易度選択画面　伊波
// SelectManagementWindowをまねした

public class SelectDifficultyWindow : BaseWindow
{
    [Header("スロットコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("シーン切り替え")]
    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;

    // 追加（吉田）
    [Header("更新処理が必要なスクリプト")]
    [SerializeField]
    private ChangeSelectStageDescription m_stageDescription = new();

    private StageEnemyDifficultyLevel.Difficulty m_selectDifficulty = StageEnemyDifficultyLevel.Difficulty.none;

    //========================================
    //              実行処理
    //========================================

    /// <summary>
    /// 初期設定(非表示の状態)
    /// </summary>
    public override async UniTask OnInitialize()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 他UIを非表示
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 説明文の初期化
            m_stageDescription.OnInitialize();

            await UniTask.CompletedTask;
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

                // ボタンを選択
                await SelectButton();
                cancelToken.ThrowIfCancellationRequested();

                // 
                m_stageDescription.OnUpdate();

                // ステージへ
                if (m_selectUIController.IsPress)
                {
                    if (await EnterActionStage()) return;
                    cancelToken.ThrowIfCancellationRequested();
                }

                // 選択UIの後処理
                m_selectUIController.OnLateUpdate();

                // 閉じる or 戦闘ステージへ
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


    private async UniTask SelectButton()
    {

        var cancelToken = this.destroyCancellationToken;

        try
        {
            // 選択したボタン
            var id = m_selectUIController.IsPressButton();

            // 選択した難易度を反映させる
            switch (id)
            {
                case ButtonID.StageEasy:
                    {
                        m_selectDifficulty = StageEnemyDifficultyLevel.Difficulty.easy;
                        break;
                    }
                case ButtonID.StageNormal:
                    {
                        m_selectDifficulty = StageEnemyDifficultyLevel.Difficulty.normal;
                        break;
                    }
                case ButtonID.StageHard:
                    {
                        m_selectDifficulty = StageEnemyDifficultyLevel.Difficulty.hard;
                        break;
                    }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    private async UniTask<bool> EnterActionStage()
    {
        if (m_selectDifficulty == StageEnemyDifficultyLevel.Difficulty.none) return false;
        StageEnemyDifficultyLevel stageEnemyDifficultyLevel = BaseManager<StageEnemyDifficultyLevel>.instance;
        if (!stageEnemyDifficultyLevel)
        {
            Debug.LogError("StageEnemyDifficultyLevelが見つかりませんでした");
        }
        stageEnemyDifficultyLevel.SetDifficulty(m_selectDifficulty);

        m_sceneTransitionManager.m_sceneName = stageEnemyDifficultyLevel.StageData.SceneName;
        await m_sceneTransitionManager.SceneChange();
        return true;
    }
}

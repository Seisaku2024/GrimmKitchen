using StageInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChangeSceneSignalController : MonoBehaviour
{
    [SerializeField]
    private SceneTransitionManager m_transitionManager;

    [SerializeField]
    private StageID m_nextStageID = StageID.Stage01;

    [SerializeField]
    private StageEnemyDifficultyLevel.Difficulty m_difficulty = StageEnemyDifficultyLevel.Difficulty.normal;


    // Start is called before the first frame update
    void Start()
    {

    }

    public async void ChangeScene()
    {
        // 難易度変更
        StageEnemyDifficultyLevel stageEnemyDifficultyLevel = BaseManager<StageEnemyDifficultyLevel>.instance;
        if (stageEnemyDifficultyLevel == null) return;

        // ステージデータを取得
        var data = StageDataBaseManager.instance.GetStageData(m_nextStageID);
        if (data == null) return;

        stageEnemyDifficultyLevel.SetStageData(data);
        stageEnemyDifficultyLevel.SetDifficulty(m_difficulty);

        await m_transitionManager.SceneChange();
    }

}

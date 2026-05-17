using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 進捗度タイプ
public enum StoryProgressType
{
    None,
    FirstVisitLobbyScene = 1,// 初めてロビーシーンを訪れた
    FirstVisitHanselGretelScene = 2,// 初めてヘンゼルとグレーテルステージを訪れた
    FirstVisitAkazukinScene = 3,// 初めて赤ずきんステージを訪れた

    CompleteHanselGretelTutrial = 100,//ヘンゼルとグレーテルステージでチュートリアルを達成した
    CompleteManagementTutrial = 101,//経営パートのチュートリアルを終えた

    CompleteAkazukinStory=110,// 赤ずきんステージのストーリーをクリアした

    PlayManagement = 150,//経営パートを一度プレイした

    GetAkazukinBook = 200,//赤ずきんの絵本手に入れた（赤ずきんステージ解放した）

    FinishLastStoryCharenge = 500,//最後のチャレンジ終了（体験版で遊べる範囲終了）

    FinishTrialGame = 600,//体験版終了

}

public class StoryProgressManager : BaseManager<StoryProgressManager>
{
    // ストーリー進捗度を管理するクラス（山本）
    [Header("ストーリー進捗度のデータベース")]
    [SerializeField]
    private StoryProgressDataBase m_storyProgressDataBase = null;
    public StoryProgressDataBase StoryProgressDataBase => m_storyProgressDataBase;

    protected override void Load()
    {
        foreach (var data in m_storyProgressDataBase.StoryDataBaseProgressList)
        {
            if (data == null) continue;
            var saveLoadData = StoryProgressDataSaveLoader.Load(data.StoryProgressType);
            data.Load(saveLoadData);
        }
    }

    public StoryProgressData GetStoryProgressData(StoryProgressType _storyProgressType)
    {
        // 返すデータ
        StoryProgressData data = null;

        foreach (var list in m_storyProgressDataBase.StoryDataBaseProgressList)
        {
#if DEBUG
            if (!list)
            {

                // データベースがnullでないか確認する
                if (data != null)
                {
                    Debug.LogError("データが登録されていません");
                }

                continue;

            }


            if (list.StoryProgressType == _storyProgressType)
            {

                // デバッグ時はIDが被っていないか確認する
                if (data != null)
                {
                    Debug.LogError("ストーリー進捗度データが2つ存在します。確認してください");
                }

                data = list;

            }


#else


            if (!list)
            {

                continue;

            }

             if (list.StoryProgressType == _storyProgressType)
            {

                data = list;

            }

#endif


        }

        if (data == null)
        {
            Debug.LogError(_storyProgressType + " このストーリー進捗度データは存在しません。登録できているか確認してください");
        }


        return data;

    }






}

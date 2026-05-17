using CI.QuickSave;
using StageInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class StoryProgressDataSaveLoader : MonoBehaviour
{
    // 制作　山本
    // ストーリー進捗度ファイルをもとにしたローダー

    /// <summary>
    /// 現状のデータをファイルにセーブする
    /// </summary>
    public static async UniTask SaveCurrentData()
    {
        try
        {
            foreach (var data in StoryProgressManager.instance.StoryProgressDataBase.StoryDataBaseProgressList)
            {
                if (data == null) continue;
                AchieveStoryProgressDataSaveLoad newData = new(data.StoryProgressType, data.AchieveStoryProgressData);
                await Save(data.StoryProgressType, newData);
            }
        }
        catch
        {

        }
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// データを全て削除する
    /// </summary>
    public static async UniTask DeleteAllData()
    {
        try
        {
            foreach (var data in StoryProgressManager.instance.StoryProgressDataBase.StoryDataBaseProgressList)
            {
                if (data == null) continue;
                await Delete(data.StoryProgressType);
            }
        }
        catch
        {

        }
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 引数StoryProgressTypeのファイルを検索し、データを取得
    /// ファイルがない場合初期データを返す
    /// </summary>
    public static AchieveStoryProgressDataSaveLoad Load(StoryProgressType _type)
    {
        string path = System.IO.Path.Combine(_type.ToString());

        AchieveStoryProgressDataSaveLoad saveLoad = new(_type);

        // 読み込みを失敗した場合は新規作成
        if (QuickSaveReader.RootExists(path) == false)
        {
            Debug.Log("セーブデータが存在しませんでした。初期状態を返します。: " + _type.ToString());
            return saveLoad;
        }

        // 読み込みデータを作成
        QuickSaveReader reader = QuickSaveReader.Create(path, SaveManager.QuickSaveSettings);

        try
        {
            saveLoad.StoryProgressType = reader.Read<StoryProgressType>("StoryProgressType");
            saveLoad.IsAchieve = reader.Read<bool>("IsFinish");
        }
        catch
        {
            saveLoad = new(_type);
            Debug.Log("読み取りミスが発生しました。初期状態を返します。: " + _type.ToString());
            return saveLoad;
        }

        // 読み込みデータを返信
        return saveLoad;
    }


    /// <summary>
    /// 引数のストーリー進捗度を基にファイルにセーブ
    /// ファイルが存在しなければ新規で作成する
    /// </summary>
    public static async UniTask Save(StoryProgressType _type, AchieveStoryProgressDataSaveLoad _data)
    {
        if (_data == null) return;

        string path = System.IO.Path.Combine(_type.ToString());

        // 書き込みデータを作成
        var writer = QuickSaveWriter.Create(path, SaveManager.QuickSaveSettings);

        writer.Write<StoryProgressType>("StoryProgressType", _data.StoryProgressType);
        writer.Write<bool>("IsFinish", _data.IsAchieve);

        // 書き込み
        writer.Commit();

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// ファイルが存在すれば削除
    /// </summary>
    public static async UniTask Delete(StoryProgressType _type)
    {
        string path = System.IO.Path.Combine(_type.ToString());

        try
        {
            // ファイルを削除
            QuickSaveWriter.DeleteRoot(path);
        }
        catch
        {

        }

        await UniTask.CompletedTask;
    }


}

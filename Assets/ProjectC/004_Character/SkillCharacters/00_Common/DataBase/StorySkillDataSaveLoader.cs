using CI.QuickSave;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySkillDataSaveLoader : MonoBehaviour
{
    // 制作　山本
    // 童話スキルデータをもとにしたローダー
    public static async UniTask SaveCurrentData()
    {
        try
        {
            foreach (var data in StorySkillDataBaseManager.instance.DataBase.StorySkillDataList)
            {
                if (data == null) continue;
                MasterStorySkillDataSaveLoad newData = new(data.StorySkill_ID, data.MasterStorySkillData);
                await Save(data.StorySkill_ID, newData);
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
            foreach (var data in StorySkillDataBaseManager.instance.DataBase.StorySkillDataList)
            {
                if (data == null) continue;
                await Delete(data.StorySkill_ID);
            }
        }
        catch
        {

        }
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 引数StorySkillIDのファイルを検索し、データを取得
    /// ファイルがない場合初期データを返す
    /// </summary>
    public static MasterStorySkillDataSaveLoad Load(StorySkill_ID _id)
    {
        string path = System.IO.Path.Combine(_id.ToString());

        MasterStorySkillDataSaveLoad saveLoad = new(_id);

        // 読み込みを失敗した場合は新規作成
        if (QuickSaveReader.RootExists(path) == false)
        {
            Debug.Log("セーブデータが存在しませんでした。初期状態を返します。: " + _id.ToString());
            return saveLoad;
        }

        // 読み込みデータを作成
        QuickSaveReader reader = QuickSaveReader.Create(path, SaveManager.QuickSaveSettings);

        try
        {
            saveLoad.StorySkill_ID = reader.Read<StorySkill_ID>("StorySkill_ID");
            saveLoad.IsMaster = reader.Read<bool>("IsMaster");
        }
        catch
        {
            saveLoad = new(_id);
            Debug.Log("読み取りミスが発生しました。初期状態を返します。: " + _id.ToString());
            return saveLoad;
        }

        // 読み込みデータを返信
        return saveLoad;
    }


    /// <summary>
    /// 引数の童話スキルを基にファイルにセーブ
    /// ファイルが存在しなければ新規で作成する
    /// </summary>
    public static async UniTask Save(StorySkill_ID _id, MasterStorySkillDataSaveLoad _data)
    {
        if (_data == null) return;

        string path = System.IO.Path.Combine(_id.ToString());

        // 書き込みデータを作成
        var writer = QuickSaveWriter.Create(path, SaveManager.QuickSaveSettings);

        writer.Write<StorySkill_ID>("StorySkill_ID", _data.StorySkill_ID);
        writer.Write<bool>("IsMaster", _data.IsMaster);

        // 書き込み
        writer.Commit();

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// ファイルが存在すれば削除
    /// </summary>
    public static async UniTask Delete(StorySkill_ID _id)
    {
        string path = System.IO.Path.Combine(_id.ToString());

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




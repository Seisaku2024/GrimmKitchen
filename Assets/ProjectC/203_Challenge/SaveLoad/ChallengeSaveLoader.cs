using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using ChallengeInfo;
using Cysharp.Threading.Tasks;

public class SettingChallengeSaveLoad
{
    public SettingChallengeSaveLoad()
    {

    }

    public ChallengeID ChallengeID = ChallengeID.None;

}


public class ChallengeSaveLoader
{
    // 制作者 田内
    // チャレンジファイルを基にしたローダー

    //=======================================================
    //                      実行処理
    //=======================================================


    /// <summary>
    /// 現状のデータをファイルにセーブする
    /// </summary>
    public static async UniTask SaveCurrentData()
    {
        try
        {
            List<ChallengeData> list = ChallengeDataBaseManager.instance.GetDataList();
            foreach (var data in list)
            {
                if (data == null) continue;
                ChallengeSaveLoad newData = new(data.ChallengeID, data.ClearChallengeData);
                await ChallengeSaveLoader.Save(data.ChallengeID, newData);
            }


            SettingChallengeSaveLoad newSettingData = new();
            newSettingData.ChallengeID = ChallengeManager.instance.ChallengeID;
            await ChallengeSaveLoader.Save(newSettingData);
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
            foreach (var data in ChallengeDataBaseManager.instance.GetDataList())
            {
                if (data == null) continue;
                await ChallengeSaveLoader.Delete(data.ChallengeID);
            }

            await ChallengeSaveLoader.Delete();
        }
        catch
        {

        }
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 引数IDのファイルを検索し、データを取得
    /// ファイルがない場合初期データを返す
    /// </summary>
    public static ChallengeSaveLoad Load(ChallengeID _id)
    {
        string path = System.IO.Path.Combine(_id.ToString());

        ChallengeSaveLoad saveLoad = new(_id);

        // 読み込みを失敗した場合は新規作成
        if (QuickSaveReader.RootExists(path) == false)
        {
            Debug.Log("セーブデータが存在しませんでした。初期状態を返します。:Challenge" + _id.ToString());
            return saveLoad;
        }

        // 読み込みデータを作成
        QuickSaveReader reader = QuickSaveReader.Create(path, SaveManager.QuickSaveSettings);

        try
        {
            saveLoad.IsClear = reader.Read<bool>("IsClear");
            saveLoad.IsLock = reader.Read<bool>("IsLock");
            saveLoad.ClearNum = reader.Read<uint>("ClearNum");
        }
        catch
        {
            saveLoad = new(_id);
            Debug.Log("読み取りミスが発生しました。初期状態を返します。:Challenge" + _id.ToString());
            return saveLoad;
        }

        // 読み込みデータを返信
        return saveLoad;
    }

    /// <summary>
    /// 引数IDのファイルを検索し、データを取得
    /// ファイルがない場合初期データを返す
    /// </summary>
    public static SettingChallengeSaveLoad Load()
    {
        string path = System.IO.Path.Combine("SettingChallenge");

        SettingChallengeSaveLoad saveLoad = new();

        // 読み込みを失敗した場合は新規作成
        if (QuickSaveReader.RootExists(path) == false)
        {
            Debug.Log("セーブデータが存在しませんでした。初期状態を返します。: SettingChallenge");
            return saveLoad;
        }

        // 読み込みデータを作成
        QuickSaveReader reader = QuickSaveReader.Create(path, SaveManager.QuickSaveSettings);

        try
        {
            saveLoad.ChallengeID = reader.Read<ChallengeID>("SettingChallenge");
        }
        catch
        {
            saveLoad = new();
            Debug.Log("読み取りミスが発生しました。初期状態を返します。: SettingChallenge");
            return saveLoad;
        }

        // 読み込みデータを返信
        return saveLoad;
    }


    /// <summary>
    /// 引数のIDを基にファイルにセーブ
    /// ファイルが存在しなければ新規で作成する
    /// </summary>
    public static async UniTask Save(ChallengeID _id, ChallengeSaveLoad _data)
    {
        if (_data == null) return;

        string path = System.IO.Path.Combine(_id.ToString());

        // 書き込みデータを作成
        var writer = QuickSaveWriter.Create(path, SaveManager.QuickSaveSettings);

        writer.Write<bool>("IsClear", _data.IsClear);

        writer.Write<bool>("IsLock", _data.IsLock);

        writer.Write<uint>("ClearNum", _data.ClearNum);


        // 書き込み
        writer.Commit();


        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 引数のIDを基にファイルにセーブ
    /// ファイルが存在しなければ新規で作成する
    /// </summary>
    public static async UniTask Save(SettingChallengeSaveLoad _data)
    {
        if (_data == null) return;


        string path = System.IO.Path.Combine("SettingChallenge");

        // 書き込みデータを作成
        var writer = QuickSaveWriter.Create(path, SaveManager.QuickSaveSettings);

        writer.Write<ChallengeID>("SettingChallenge", _data.ChallengeID);

        // 書き込み
        writer.Commit();


        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 引数のIDのファイルが存在すれば削除
    /// </summary>
    public static async UniTask Delete(ChallengeID _id)
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

    /// <summary>
    /// 引数のIDのファイルが存在すれば削除
    /// </summary>
    public static async UniTask Delete()
    {
        string path = System.IO.Path.Combine("SettingChallenge");

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

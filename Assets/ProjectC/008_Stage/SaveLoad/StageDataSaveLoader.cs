using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using StageInfo;
using Cysharp.Threading.Tasks;

public class StageDataSaveLoader : MonoBehaviour
{
    // 制作者 田内
    // ステージデータファイルを基にしたローダー

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
            foreach (var data in StageDataBaseManager.instance.DataBase.StageDataBaseList)
            {
                if (data == null) continue;
                ClearStageDataSaveLoad newData = new(data.StageID, data.ClearStageData);
                await Save(data.StageID, newData);
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
            foreach (var data in StageDataBaseManager.instance.DataBase.StageDataBaseList)
            {
                if (data == null) continue;
                await Delete(data.StageID);
            }
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
    public static ClearStageDataSaveLoad Load(StageID _id)
    {
        string path = System.IO.Path.Combine(_id.ToString());

        ClearStageDataSaveLoad saveLoad = new(_id);

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
            saveLoad.StageID = reader.Read<StageID>("StageID");
            saveLoad.IsClear = reader.Read<bool>("IsClear");
            saveLoad.IsLock = reader.Read<bool>("IsLock");
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
    /// 引数のIDを基にファイルにセーブ
    /// ファイルが存在しなければ新規で作成する
    /// </summary>
    public static async UniTask Save(StageID _id, ClearStageDataSaveLoad _data)
    {
        if (_data == null) return;

        string path = System.IO.Path.Combine(_id.ToString());

        // 書き込みデータを作成
        var writer = QuickSaveWriter.Create(path, SaveManager.QuickSaveSettings);

        writer.Write<StageID>("StageID", _data.StageID);
        writer.Write<bool>("IsClear", _data.IsClear);
        writer.Write<bool>("IsLock", _data.IsLock);

        // 書き込み
        writer.Commit();

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// ファイルが存在すれば削除
    /// </summary>
    public static async UniTask Delete(StageID _id)
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

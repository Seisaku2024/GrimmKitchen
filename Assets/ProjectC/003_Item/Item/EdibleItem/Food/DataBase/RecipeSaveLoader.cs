using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodInfo;
using CI.QuickSave;
using Cysharp.Threading.Tasks;

public class RecipeSaveLoader
{
    // 制作者 田内
    // レシピファイルを基にしたローダー

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
            List<FoodData> list = ItemDataBaseManager.instance.FoodDataBase.FoodDataBaseList;
            foreach (var data in list)
            {
                if (data == null) continue;
                RecipeSaveLoad newData = new((FoodID)data.ItemID, data.LockRecipeData);
                await RecipeSaveLoader.Save((FoodID)data.ItemID, newData);
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
            List<FoodData> list = ItemDataBaseManager.instance.FoodDataBase.FoodDataBaseList;
            foreach (var data in list)
            {
                if (data == null) continue;
                await RecipeSaveLoader.Delete((FoodID)data.ItemID);
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
    public static RecipeSaveLoad Load(FoodID _id)
    {
        string path = System.IO.Path.Combine(_id.ToString());

        RecipeSaveLoad saveLoad = new(_id);

        // 読み込みを失敗した場合は新規作成
        if (QuickSaveReader.RootExists(path) == false)
        {
            Debug.Log("セーブデータが存在しませんでした。初期状態を返します。:Recipe" + _id.ToString());
            return saveLoad;
        }

        // 読み込みデータを作成

        QuickSaveReader reader = QuickSaveReader.Create(path, SaveManager.QuickSaveSettings);
        try
        {
            if (reader.Exists("IsLock")) saveLoad.IsLock = reader.Read<bool>("IsLock");
            if (reader.Exists("FoodID")) saveLoad.FoodID = reader.Read<FoodID>("FoodID");
        }
        catch
        {
            saveLoad = new(_id);
            Debug.Log("読み取りミスが発生しました。初期状態を返します。:Recipe" + _id.ToString());
            return saveLoad;
        }

        // 読み込みデータを返信
        return saveLoad;
    }


    /// <summary>
    /// 引数のIDを基にファイルにセーブ
    /// ファイルが存在しなければ新規で作成する
    /// </summary>
    public static async UniTask Save(FoodID _id, RecipeSaveLoad _data)
    {
        if (_data == null) return;

        string path = System.IO.Path.Combine(_id.ToString());

        // 書き込みデータを作成
        var writer = QuickSaveWriter.Create(path, SaveManager.QuickSaveSettings);

        writer.Write<bool>("IsLock", _data.IsLock);
        writer.Write<FoodID>("FoodID", _data.FoodID);

        // 書き込み
        writer.Commit();

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 引数のIDのファイルが存在すれば削除
    /// </summary>
    public static async UniTask Delete(FoodID _id)
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

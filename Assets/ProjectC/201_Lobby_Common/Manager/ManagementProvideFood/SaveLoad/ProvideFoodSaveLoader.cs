using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using System.Linq;
using FoodInfo;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class ProvideFoodSaveLoadData
{
    public ProvideFoodSaveLoadData()
    {
    }

    public FoodID FoodID = FoodID.None;
}

[System.Serializable]
public class ProvideFoodSaveLoad
{
    public ProvideFoodSaveLoad()
    {
    }

    public List<ProvideFoodSaveLoadData> ProvideFoodSaveLoadDataList = new();
}

public class ProvideFoodSaveLoader : MonoBehaviour
{
    // 制作者 田内
    // 提供料理ファイルを基にしたローダー

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
            ProvideFoodSaveLoad saveData = new();
            foreach (var id in ProvideFoodManager.instance.ProvideFoodIDRC)
            {
                ProvideFoodSaveLoadData newData = new();
                newData.FoodID = id;
                saveData.ProvideFoodSaveLoadDataList.Add(newData);
            }
            await Save(saveData);
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
        await Delete();
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// ファイルがない場合初期データを返す
    /// </summary>
    public static ProvideFoodSaveLoad Load()
    {
        string path = System.IO.Path.Combine("ProvideFood");

        ProvideFoodSaveLoad saveLoad = new();

        // 読み込みを失敗した場合は新規作成
        if (QuickSaveReader.RootExists(path) == false)
        {
            Debug.Log("セーブデータが存在しませんでした。初期状態を返します。: ProvideFood");
            return saveLoad;
        }

        // 読み込みデータを作成
        QuickSaveReader reader = QuickSaveReader.Create(path, SaveManager.QuickSaveSettings);

        try
        {
            saveLoad.ProvideFoodSaveLoadDataList = reader.Read<List<ProvideFoodSaveLoadData>>("ProvideFoodList");
        }
        catch
        {
            saveLoad = new();
            Debug.Log("読み取りミスが発生しました。初期状態を返します。: ProvideFood");
            return saveLoad;
        }

        // 読み込みデータを返信
        return saveLoad;
    }


    /// <summary>
    /// ファイルが存在しなければ新規で作成する
    /// </summary>
    public static async UniTask Save(ProvideFoodSaveLoad _data)
    {
        if (_data == null) return;

        string path = System.IO.Path.Combine("ProvideFood");

        // 書き込みデータを作成
        var writer = QuickSaveWriter.Create(path, SaveManager.QuickSaveSettings);

        writer.Write<List<ProvideFoodSaveLoadData>>("ProvideFoodList", _data.ProvideFoodSaveLoadDataList);

        // 書き込み
        writer.Commit();

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 引数のIDのファイルが存在すれば削除
    /// </summary>
    public static async UniTask Delete()
    {
        string path = System.IO.Path.Combine("ProvideFood");

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using CI.QuickSave;
using System.Linq;
using Cysharp.Threading.Tasks;

/// <summary>
/// 読み込み/書き込み用チャレンジデータ
/// </summary>
[System.Serializable]
public class PocketItemSaveLoad
{
    public PocketItemSaveLoad()
    {
    }


    // ポケットアイテムデータをまとめたリスト
    public List<PocketItemData> PocketItemDataList = new();
}


public static class PocketItemSaveLoader
{
    // 制作者 田内
    // ポケットアイテムファイルを基にしたローダー

    //=======================================================
    //                      実行処理
    //=======================================================


    /// <summary>
    /// 現状のデータをファイルにセーブする
    /// </summary>
    public static async UniTask SaveCurrentData(PocketType _type)
    {
        try
        {
            var pocketManager = _type.GetPocketItemDataManager();
            if (pocketManager == null) return;

            PocketItemSaveLoad pocketItemSaveLoad = new();

            foreach (var data in pocketManager.ItemDataRC)
            {
                if (data == null || data.IsSave == false) continue;
                pocketItemSaveLoad.PocketItemDataList.Add(data);
            }
            // セーブ
            await Save(_type, pocketItemSaveLoad);
        }
        catch
        {

        }
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// データを削除する
    /// </summary>
    public static async UniTask DeleteAllData(PocketType _type)
    {
        await Delete(_type);
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 引数IDのファイルを検索し、データを取得
    /// ファイルがない場合初期データを返す
    /// </summary>
    public static PocketItemSaveLoad Load(PocketType _type)
    {
        string path = System.IO.Path.Combine(_type.ToString());

        PocketItemSaveLoad saveLoad = new();

        // 読み込みを失敗した場合は新規作成
        if (QuickSaveReader.RootExists(path) == false)
        {
            Debug.Log("セーブデータが存在しませんでした。初期状態を返します。:PocketItem" + _type.ToString());
            return saveLoad;
        }

        // 読み込みデータを作成
        QuickSaveReader reader = QuickSaveReader.Create(path, SaveManager.QuickSaveSettings);

        try
        {
            saveLoad.PocketItemDataList = reader.Read<List<PocketItemData>>("PocketItemDataList");
        }
        catch
        {
            saveLoad = new();
            Debug.Log("セーブデータが存在しませんでした。初期状態を返します。:PocketItem" + _type.ToString());
            return saveLoad;
        }

        // 読み込みデータを返信
        return saveLoad;
    }


    /// <summary>
    /// 引数のIDを基にファイルにセーブ
    /// ファイルが存在しなければ新規で作成する
    /// </summary>
    public static async UniTask Save(PocketType _type, PocketItemSaveLoad _data)
    {
        if (_data == null) return;


        string path = System.IO.Path.Combine(_type.ToString());

        // 書き込みデータを作成
        var writer = QuickSaveWriter.Create(path, SaveManager.QuickSaveSettings);

        writer.Write<List<PocketItemData>>("PocketItemDataList", _data.PocketItemDataList);

        // 書き込み
        writer.Commit();

        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 引数のIDのファイルが存在すれば削除
    /// </summary>
    public static async UniTask Delete(PocketType _type)
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

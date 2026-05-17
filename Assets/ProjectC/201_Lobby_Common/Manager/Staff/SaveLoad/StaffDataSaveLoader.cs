using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using Cysharp.Threading.Tasks;

/// <summary>
/// 読み込み/書き込み用経営データ
/// </summary>
[System.Serializable]
public class StaffDataSaveLoad
{
    public StaffDataSaveLoad()
    {
    }

    // スタッフストレージリスト
    public List<StaffStatusSaveLoadData> StaffStatusSaveLoadDataList = new();

    // スタッフポイントリスト
    [HideInInspector]
    public List<StaffPointSaveLoadData> StaffPointSaveLoadDataList = new();

}

public class StaffDataSaveLoader : MonoBehaviour
{
    // 制作者 田内
    // 経営データファイルを基にしたローダー

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
            StaffDataSaveLoad saveLoad = new();

            foreach (var data in StaffManager.instance.StaffStorageRC)
            {
                if (data == null) continue;
                if (data.IsSave == false) continue;

                StaffStatusSaveLoadData newData = new(data);
                saveLoad.StaffStatusSaveLoadDataList.Add(newData);
            }

            foreach (var data in StaffManager.instance.StaffPointDataList)
            {
                if (data == null) continue;
                StaffPointSaveLoadData saveLoadData = new(data.SetStaffPointData);
                saveLoad.StaffPointSaveLoadDataList.Add(saveLoadData);
            }

            await Save(saveLoad);
        }
        catch
        {

        }
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// データを削除する
    /// </summary>
    public static async UniTask DeleteAllData()
    {
        await Delete();
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 引数IDのファイルを検索し、データを取得
    /// ファイルがない場合初期データを返す
    /// </summary>
    public static StaffDataSaveLoad Load()
    {
        string path = System.IO.Path.Combine("StaffData");

        StaffDataSaveLoad saveLoad = new();

        // 読み込みを失敗した場合は新規作成
        if (QuickSaveReader.RootExists(path) == false)
        {
            Debug.Log("セーブデータが存在しませんでした。初期状態を返します。: StaffDataSaveLoad");
            return saveLoad;
        }

        // 読み込みデータを作成
        QuickSaveReader reader = QuickSaveReader.Create(path, SaveManager.QuickSaveSettings);
        try
        {
            saveLoad.StaffStatusSaveLoadDataList = reader.Read<List<StaffStatusSaveLoadData>>("StaffStorageList");
            saveLoad.StaffPointSaveLoadDataList = reader.Read<List<StaffPointSaveLoadData>>("StaffPointSaveLoadDataList");
        }
        catch
        {
            saveLoad = new();
            Debug.Log("読み取りミスが発生しました。初期状態を返します。: StaffDataSaveLoad");
            return saveLoad;
        }

        // 読み込みデータを返信
        return saveLoad;
    }


    /// <summary>
    /// 引数のIDを基にファイルにセーブ
    /// ファイルが存在しなければ新規で作成する
    /// </summary>
    public static async UniTask Save(StaffDataSaveLoad _saveLoad)
    {
        if (_saveLoad == null) return;

        string path = System.IO.Path.Combine("StaffData");

        // 書き込みデータを作成
        var writer = QuickSaveWriter.Create(path, SaveManager.QuickSaveSettings);

        writer.Write<List<StaffStatusSaveLoadData>>("StaffStorageList", _saveLoad.StaffStatusSaveLoadDataList);
        writer.Write<List<StaffPointSaveLoadData>>("StaffPointSaveLoadDataList", _saveLoad.StaffPointSaveLoadDataList);

        // 書き込み
        writer.Commit();

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// ファイルが存在すれば削除
    /// </summary>
    public static async UniTask Delete()
    {
        string path = System.IO.Path.Combine("StaffData");

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using UpgradeManagementHouseInfo;
using Cysharp.Threading.Tasks;

/// <summary>
/// 読み込み/書き込み用経営データ
/// </summary>
[System.Serializable]
public class ManagementDataSaveLoad
{
    // 総額
    public int TotalEarnedMoney = 0;

    // 総評価
    public int TotalEvaluation = 0;

    // お店状態
    public UpgradeManagementHouseID UpgradeManagementHouseID = UpgradeManagementHouseID.Upgrade1;

}


public class ManagementDataSaveLoader
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
            ManagementDataSaveLoad saveLoad = new();

            saveLoad.TotalEarnedMoney = ManagementDataManager.instance.TotalEarnedMoney;
            saveLoad.TotalEvaluation = ManagementDataManager.instance.TotalEvaluation;
            saveLoad.UpgradeManagementHouseID = ManagementDataManager.instance.UpgradeManagementHouseID;

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
    public static ManagementDataSaveLoad Load()
    {
        string path = System.IO.Path.Combine("ManagementData");

        ManagementDataSaveLoad saveLoad = new();

        // 読み込みを失敗した場合は新規作成
        if (QuickSaveReader.RootExists(path) == false)
        {
            Debug.Log("セーブデータが存在しませんでした。初期状態を返します。: ManagementDataSaveLoad");
            return saveLoad;
        }

        // 読み込みデータを作成
        QuickSaveReader reader = QuickSaveReader.Create(path, SaveManager.QuickSaveSettings);
        try
        {
            saveLoad.TotalEarnedMoney = reader.Read<int>("TotalEarnedMoney");
            saveLoad.TotalEvaluation = reader.Read<int>("TotalEvaluation");
            saveLoad.UpgradeManagementHouseID = reader.Read<UpgradeManagementHouseID>("UpgradeManagementHouseID");
        }
        // 読み取りミスが発生した場合は初期値
        catch
        {
            saveLoad = new();
            Debug.Log("読み取りミスが発生しました。初期状態を返します。: ManagementDataSaveLoad");
        }

        // 読み込みデータを返信
        return saveLoad;
    }


    /// <summary>
    /// 引数のIDを基にファイルにセーブ
    /// ファイルが存在しなければ新規で作成する
    /// </summary>
    public static async UniTask Save(ManagementDataSaveLoad _saveLoad)
    {
        if (_saveLoad == null) return;

        string path = System.IO.Path.Combine("ManagementData");

        // 書き込みデータを作成
        var writer = QuickSaveWriter.Create(path, SaveManager.QuickSaveSettings);

        writer.Write<int>("TotalEarnedMoney", _saveLoad.TotalEarnedMoney);
        writer.Write<int>("TotalEvaluation", _saveLoad.TotalEvaluation);
        writer.Write<UpgradeManagementHouseID>("UpgradeManagementHouseID", _saveLoad.UpgradeManagementHouseID);

        // 書き込み
        writer.Commit();

        await UniTask.CompletedTask;
    }

    /// <summary>
    /// ファイルが存在すれば削除
    /// </summary>
    public static async UniTask Delete()
    {
        try
        {
            string path = System.IO.Path.Combine("ManagementData");
            // ファイルを削除
            QuickSaveWriter.DeleteRoot(path);
        }
        catch
        {

        }

        await UniTask.CompletedTask;

        try
        {
            // 歴代売り上げグラフのデータ削除
            string path = System.IO.Path.Combine("ManagementResult");
            QuickSaveWriter.DeleteRoot(path);
        }
        catch
        {

        }

        await UniTask.CompletedTask;
    }
}

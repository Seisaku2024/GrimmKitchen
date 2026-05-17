using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.QuickSave;
using CI.QuickSave.Core.Storage;
using ChallengeInfo;
using Cysharp.Threading.Tasks;

public class PlayerStatusSaveLoader
{
    // 制作者 伊波


    //=======================================================
    //                      実行処理
    //=======================================================

    // セーブ処理はPlayerParameters内で随時行う
    public static async UniTask SaveCurrentData()
    {
        try
        {
            var writer = QuickSaveWriter.Create("PlayerStatus", SaveManager.QuickSaveSettings);
            writer.Write<int>("StrengtheningCap", PlayerStatusManager.instance.StrengtheningCap);

            PlayerStatusManager.instance.NowStatus().Save(writer);
            writer.Commit();
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

            string path = System.IO.Path.Combine("PlayerStatus");
            QuickSaveWriter.DeleteRoot(path);
        }
        catch
        {

        }
        await UniTask.CompletedTask;

        try
        {
            string path = System.IO.Path.Combine("PlayerStorySkillStatus");
            QuickSaveWriter.DeleteRoot(path);
        }
        catch
        {

        }
        await UniTask.CompletedTask;


        BaseManager<PlayerStatusManager>.instance.DataLoad();
    }

    public static void Load(ref CharacterStatus status, ref StrengtheningStatus data, ref int strengthCap)
    {
        if (QuickSaveReader.RootExists("PlayerStatus"))
        {
            QuickSaveReader reader = QuickSaveReader.Create("PlayerStatus", SaveManager.QuickSaveSettings);

            try
            {
                strengthCap = reader.Read<int>("StrengtheningCap");
            }
            catch
            {

            }

            data.Load(reader);
        }
        else
        {
            // 無かった場合
            Debug.Log("セーブデータがないので新規作成します:PlayerStatus");
            data.Reset(PlayerDataBaseManager.instance.DataBase);
        }

        status = new CharacterStatus(
            PlayerDataBaseManager.instance.DataBase.CharacterStatus,
            data);
    }
}
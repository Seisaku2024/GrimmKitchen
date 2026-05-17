using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using UniRx;
using Cysharp.Threading.Tasks;
using CI.QuickSave;

public class SaveManager : BaseManager<SaveManager>
{
    // セーブ処理をまとめたクラス


    /// <summary>
    /// セーブ時に発信されるイベント
    /// </summary>
    public class GlobalSaveEvent
    {
        public enum Type
        {
            Start = 0,      // セーブ開始
            End = 1,        // セーブ終了    
        }

        public Type SaveType = Type.Start;

        // イベント送信
        public static void Publish(Type _type)
        {
            GlobalSaveEvent eve = new();
            eve.SaveType = _type;
            MessageBroker.Default.Publish<GlobalSaveEvent>(eve);
        }
    }

    /// <summary>
    /// 削除時に発信されるイベント
    /// </summary>
    public class GlobalDeleteSaveEvent
    {
        public static void Publish()
        {
            GlobalDeleteSaveEvent eve = new();
            MessageBroker.Default.Publish<GlobalDeleteSaveEvent>(eve);
        }
    }

    [Header("オートセーブ")]
    [SerializeField]
    private bool m_autoSave = true;

    /////////////////////////////セーブデータ用///////////////////////////////////
    ///////////※値を変更した場合、既存のファイルは機能しなくなります※///////////
    public const string PasswordSaveLoad = "Mi_8ITa_8JoYo_8Ne_8KaHa";
    public const SecurityMode SecurityModeSaveLoad = SecurityMode.Aes;
    public const CompressionMode CompressionModeSaveLoad = CompressionMode.Gzip;
    //////////////////////////////////////////////////////////////////////////////

    public static readonly QuickSaveSettings QuickSaveSettings = new QuickSaveSettings
    {
        SecurityMode = SaveManager.SecurityModeSaveLoad,
        Password = SaveManager.PasswordSaveLoad,
        CompressionMode = SaveManager.CompressionModeSaveLoad
    };

    private bool m_isSave = false;

    //======================================
    //              実行処理
    //======================================

    /// <summary>
    /// セーブ処理
    /// </summary>
    public async UniTask AllSave()
    {
#if UNITY_EDITOR
        if (m_autoSave == false) return;
#endif

        // セーブ中であれば通さない
        if (m_isSave == true) return;
        m_isSave = true;


        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // セーブ開始イベントを発信
            await UniTask.SwitchToMainThread();
            GlobalSaveEvent.Publish(GlobalSaveEvent.Type.Start);

            // マルチスレッドでセーブを開始
            await UniTask.RunOnThreadPool(async () =>
            {
                await Save();
                cancelToken.ThrowIfCancellationRequested();
            });

            cancelToken.ThrowIfCancellationRequested();

            // セーブ終了イベントを発信
            await UniTask.SwitchToMainThread();
            GlobalSaveEvent.Publish(GlobalSaveEvent.Type.End);
        }
        catch
        {

        }


        // セーブデータがあることを記憶
        var writer = QuickSaveWriter.Create("NewGame");
        writer.Commit();

        m_isSave = false;
        await UniTask.CompletedTask;
    }

    /// <summary>
    /// 削除
    /// </summary>
    public async UniTask AllDelete()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // マルチスレッドでデリートを開始
            await UniTask.RunOnThreadPool(async () =>
            {
                await Delete();
                cancelToken.ThrowIfCancellationRequested();
            });

            cancelToken.ThrowIfCancellationRequested();

            // セーブ削除イベントを発信
            await UniTask.SwitchToMainThread();
            GlobalDeleteSaveEvent.Publish();
        }
        catch
        {

        }

        // セーブデータ削除
        QuickSaveWriter.DeleteRoot("NewGame");

        await UniTask.CompletedTask;
    }



    /// <summary>
    /// ゲームデータがセーブされているかどうか
    /// </summary>
    public bool IsSave()
    {
        return QuickSaveReader.RootExists("NewGame");
    }



    // セーブ処理をまとめた関数
    private async UniTask Save()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await StaffDataSaveLoader.SaveCurrentData();
            cancelToken.ThrowIfCancellationRequested();

            await RecipeSaveLoader.SaveCurrentData();
            cancelToken.ThrowIfCancellationRequested();

            await StaffDataSaveLoader.SaveCurrentData();
            cancelToken.ThrowIfCancellationRequested();

            await StageDataSaveLoader.SaveCurrentData();
            cancelToken.ThrowIfCancellationRequested();

            await ManagementDataSaveLoader.SaveCurrentData();
            cancelToken.ThrowIfCancellationRequested();

            await ChallengeSaveLoader.SaveCurrentData();
            cancelToken.ThrowIfCancellationRequested();

            await PocketItemSaveLoader.SaveCurrentData(PocketType.Inventory);
            cancelToken.ThrowIfCancellationRequested();
            await PocketItemSaveLoader.SaveCurrentData(PocketType.ManagementStorage);
            cancelToken.ThrowIfCancellationRequested();

            await ProvideFoodSaveLoader.SaveCurrentData();
            cancelToken.ThrowIfCancellationRequested();

            await StoryProgressDataSaveLoader.SaveCurrentData();
            cancelToken.ThrowIfCancellationRequested();

            await PlayerStatusSaveLoader.SaveCurrentData();
            cancelToken.ThrowIfCancellationRequested();

            await StorySkillDataSaveLoader.SaveCurrentData();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch
        {

        }
    }

    // セーブ削除処理をまとめた関数
    private async UniTask Delete()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await RecipeSaveLoader.DeleteAllData();
            cancelToken.ThrowIfCancellationRequested();

            await StaffDataSaveLoader.DeleteAllData();
            cancelToken.ThrowIfCancellationRequested();

            await StageDataSaveLoader.DeleteAllData();
            cancelToken.ThrowIfCancellationRequested();

            await ManagementDataSaveLoader.DeleteAllData();
            cancelToken.ThrowIfCancellationRequested();

            await ChallengeSaveLoader.DeleteAllData();
            cancelToken.ThrowIfCancellationRequested();

            await PocketItemSaveLoader.DeleteAllData(PocketType.Inventory);
            cancelToken.ThrowIfCancellationRequested();
            await PocketItemSaveLoader.DeleteAllData(PocketType.ManagementStorage);
            cancelToken.ThrowIfCancellationRequested();

            await ProvideFoodSaveLoader.DeleteAllData();
            cancelToken.ThrowIfCancellationRequested();

            await PlayerStatusSaveLoader.DeleteAllData();
            cancelToken.ThrowIfCancellationRequested();

            await StoryProgressDataSaveLoader.DeleteAllData();
            cancelToken.ThrowIfCancellationRequested();

            await StorySkillDataSaveLoader.DeleteAllData();
            cancelToken.ThrowIfCancellationRequested();


        }
        catch
        {

        }

    }
}

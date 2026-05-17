using UnityEngine;
using Steamworks;
using ExternalPropertyAttributes;

public class SteamAPIManager : MonoBehaviour
{

    [Label("アプリケーションID デフォルトの480はサンプル用")]
    public AppId_t appId_T = new AppId_t(480);

    private static SteamAPIManager instance;

    public bool IsInitialized => SteamAPI.IsSteamRunning();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (!SteamAPI.RestartAppIfNecessary(appId_T))
        {
            Debug.LogError("Steamから起動していません。");
            Application.Quit();
        }

        if (!SteamAPI.Init())
        {
            Debug.LogError("SteamAPIの初期化に失敗しました。");
            Application.Quit();
        }
        Debug.Log("SteamInitialized"+SteamUser.GetSteamID());

    }

    void Update()
    {
        SteamAPI.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        SteamAPI.Shutdown();
    }
    void AchivementTest()
    {
        // 自分の状態を取得する
        bool success = SteamUserStats.RequestCurrentStats();
        if (!success) return;

        /// 実績解除(bool)
        SteamUserStats.SetAchievement("ACH_WIN_ONE_GAME");
        SteamUserStats.SetAchievement("ACH_WIN_100_GAMES");
        SteamUserStats.SetAchievement("ACH_TRAVEL_FAR_ACCUM");
        SteamUserStats.SetAchievement("ACH_TRAVEL_FAR_SINGLE");
        SteamUserStats.StoreStats();    // ポコンって表示される

        // 実績のリセット
       // SteamUserStats.ResetAllStats(true); // 全実績リセット

        SteamUserStats.RequestCurrentStats();


    }

}

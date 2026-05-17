using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using LobbyStateInfo;

namespace LobbyStateInfo
{
    public enum LobbyState
    {
        Normal,　       // 通常の状態
        ReturnAction,   // アクションパートから帰ってきた状態
        TrialSession,   // 体験会用、タイトルに戻る


        GoToActionTutorial = 100,         // アクションに行くのを促すチュートリアル
        GoToManagementTutorial = 101,     // 経営に行くのを促すチュートリアル
        GoToManagementNormal = 102,
        GoToActionNormal = 103,

        GpToUnLockAkazukinTutorial=104,   // 赤ずきんステージの解放を知らせる

        StoryConversation01 = 200,        // ストーリー会話01
        GoToSelectStageTutrial = 201,      // ステージ選択チュートリアルへ移行
        SelectStageTutrial = 202,         // ステージ選択のチュートリアル
        StoryConversation02 = 203,        //ストーリー会話02

        CheckStoryProgress=500,           //ストーリー進捗度から次の遷移先ステートを選ぶ
        GoEndTrialGame=600,               //体験版終了ウィンドウ表示
    }
}

public class LobbyStateUpdateManager : BaseGameStateUpdateController<LobbyStateUpdateManager>
{
    // 経営パートの進行を管理するマネージャークラス
    // 制作者　田内


    [Header("デバッグ用")]
    [SerializeField]
    private LobbyState m_lobbyState = LobbyState.Normal;


    [System.Serializable]
    private class InitilizeState
    {
        [Header("シーン名")]
        [SerializeField]
        private string m_sceneName = "";

        public string SceneName
        {
            get { return m_sceneName; }
        }

        [Header("変更ステート")]
        [SerializeField]
        private LobbyState m_lobbyState = LobbyState.Normal;

        public LobbyState LobbyState
        {
            get { return m_lobbyState; }
        }

    }

    [Header("初期ステートリスト")]
    [SerializeField]
    private List<InitilizeState> m_initilizeStateList = new();

    [Header("当てはまらなかった時の初期ステート")]
    [SerializeField]
    private LobbyState m_initilizeState = LobbyState.Normal;

    [Header("ストーリー進行用コライダー")]
    [SerializeField]
    private SerializableDictionary<LobbyState,Transform>m_storyProgressColider = new SerializableDictionary<LobbyState,Transform>();
    public Transform GetProgressColliderTransform(LobbyState _lobbyState)
    {
        var transform = m_storyProgressColider[_lobbyState];

        if (transform == null)
        {
            Debug.LogError("指定されたStateのコライダーが登録されていません");
            return null;
        }

        return transform;
    }


    //=================================================================
    //                      実行処理
    //=================================================================


    // 初期ステートをセット
    override protected void SetInitializeState()
    {
        // どのシーンからきたか
        var beforeSceneName = SceneNameManager.instance.BeforeSceneName;
        if (beforeSceneName == "None")
        {
            // 初期の値をセット
            m_currentStateDevice.Value = (int)m_lobbyState;
        }
        else
        {
            // 一致するシーン名を取得
            foreach (var state in m_initilizeStateList)
            {
                if (beforeSceneName == state.SceneName)
                {
                    // 更新
                    m_currentStateDevice.Value = (int)state.LobbyState;
                    return;
                }
            }

            // 何もなければ
            m_currentStateDevice.Value = (int)m_initilizeState;
        }

    }

}

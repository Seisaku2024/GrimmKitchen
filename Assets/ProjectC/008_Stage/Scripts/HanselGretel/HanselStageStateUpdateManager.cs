using HanselStageInfo;
using LobbyStateInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanselStageInfo
{
    public enum GameStageState
    {
        Normal,                           //一般状態
        Tutrial,                          //チュートリアル状態
        TutrialMove,                      //NPC移動状態（チュートリアル）
        TutrialBattle,                    //チュートリアル戦闘

        NextPositionMove01,               //次の地点へと移動
        NextPositionMove02,
        NextPositionMove03,

        BossBattle,                       //ボス戦

        GoToMoveTutrial         = 101,　  //移動を促すチュートリアルへ
        GoToBattleTutrial       = 102,    //バトルを促すチュートリアルへ
        GoToUseItemTutrial      = 103,    //移動を促すチュートリアルへ
        GoToStorySkillTutrial   = 104,    //童話スキル使用を促すチュートリアルへ
        GoToCookingTutrial      = 105,    //料理チュートリアルへ
        GoToHelpConvaesation    = 106,    //助けて会話へ移動
        
        GoToNormal              = 200,    //ノーマル状態へ

        MoveTutrial             = 300,    //移動チュートリアル
        BattleTutrial           = 301,    //バトルチュートリアル
        CollectingTutrial       = 302,    //採取チュートリアル
        CookingTutrial          = 303,    //料理チュートリアル
        StorySkillTutrial       = 304,    //童話スキルのチュートリアル
        PortalTutrial           = 305,    //ポータルのチュートリアル

        HelpConversation        = 400,    // 助けて会話

        CheckStoryProgress      = 500,    // ストーリー進捗度チェック
    }
}


public class HanselStageStateUpdateManager : BaseGameStateUpdateController<HanselStageStateUpdateManager>
{
    // ヘンゼルとグレーテルステージの状態を管理するクラス（山本）

    [Header("デバッグ用")]
    [SerializeField]
    private GameStageState m_hanselStageState = GameStageState.Normal;

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
        private GameStageState m_hanselStageState = GameStageState.Normal;

        public GameStageState HanselStageState
        {
            get { return m_hanselStageState; }
        }

    }

    [Header("初期ステートリスト")]
    [SerializeField]
    private List<InitilizeState> m_initilizeStateList = new();

    [Header("当てはまらなかった時の初期ステート")]
    [SerializeField]
    private GameStageState m_initilizeState = GameStageState.Normal;

    [Header("チュートリアルの当たり判定用リスト")]
    [SerializeField]
    private SerializableDictionary<GameStageState, 
        ChangeStageStateUpdateOnColliderEnter> m_tutrialColliderList;

    [Header("チュートリアルの障害コライダーリスト")]
    [SerializeField]
    private SerializableDictionary<GameStageState,
        List<Transform>> m_tutrialBarrierList;

    [Header("チュートリアル上で倒すべき敵リスト")]
    [SerializeField]
    private SerializableDictionary<GameStageState,
        List<CharacterCore>> m_tutrialEnemyList;


    // 現在のStateから上記の当たり判定リストにアクセスし、
    // 対応したコライダーとの当たり判定がされたかを確認する
    public bool CheckOnColliderEnterCollisionList(GameStageState nowState)
    {
        if(m_tutrialColliderList.Count==0)
        {
            Debug.LogError("リストに登録されていません");
            return false;
        }

        return m_tutrialColliderList[nowState].IsColliderEnter;
    }

    // 現在のStateから上記の障害物判リストにアクセスし、
    // 対応した障害物コライダーのリストを取得
    public List<Transform> GetBarrierColliderList(GameStageState nowState)
    {
        if(m_tutrialBarrierList.Count==0)
        {
            Debug.LogError("リストが登録されていません");
            return null;
        }

        return m_tutrialBarrierList[nowState];
    }

    // 現在のStateから上記のエネミーリストにアクセスし、
    // 対応したエネミーのリストを取得
    public List<CharacterCore>GetEnemyCharacterCoreList(GameStageState nowState)
    {
        if(m_tutrialEnemyList.Count==0)
        {
            Debug.LogError("リストが登録されていません");
            return null;
        }
        return m_tutrialEnemyList[nowState];
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
            m_currentStateDevice.Value = (int)m_hanselStageState;
        }
        else
        {
            // 一致するシーン名を取得
            foreach (var state in m_initilizeStateList)
            {
                if (beforeSceneName == state.SceneName)
                {
                    // 更新
                    m_currentStateDevice.Value = (int)state.HanselStageState;
                    return;
                }
            }

            // 何もなければ
            m_currentStateDevice.Value = (int)m_initilizeState;
        }

    }

}

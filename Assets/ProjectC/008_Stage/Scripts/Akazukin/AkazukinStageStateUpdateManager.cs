using AkazukinStageInfo;
using HanselStageInfo;
using NUnit.Framework;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AkazukinStageInfo
{
    public enum AkazukinStageState
    {
        Normal,

        GoToFirstMeeting,
        GoToUseFoodTutorial,
        GoToCookingTutorial,
        GoToFirstLookHunter,
        GoToUseFoodTutorialByBattle,
        GoToPreBossBattleTalk,
        GoToBossBattle,

        FirstMeeting,
        UseFoodTutrial,
        CookingTutorial,
        FirstLookHunter,
        UseFoodTutorialByBattle,
        PreBossBattleTalk,
        BossBattle,
        PostBossBattleTalk,

        CheckStoryProgress,
    }
}


public class AkazukinStageUpdateManager: BaseGameStateUpdateController<AkazukinStageUpdateManager>
{
   
    [Header("デバッグ用")]
    [SerializeField]
    private AkazukinStageState m_akazukinStageState = AkazukinStageState.Normal;

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
        private AkazukinStageState m_akazukinStageState = AkazukinStageState.Normal;

        public AkazukinStageState AkazukinStageState
        {
            get { return m_akazukinStageState; }
        }

    }

    [Header("初期ステートリスト")]
    [SerializeField]
    private List<InitilizeState> m_initilizeStateList = new();

    [Header("当てはまらなかった時の初期ステート")]
    [SerializeField]
    private AkazukinStageState m_initilizeState = AkazukinStageState.Normal;

    [Header("チュートリアルの当たり判定用リスト")]
    [SerializeField]
    private SerializableDictionary<AkazukinStageState,
        ChangeStageStateUpdateOnColliderEnter> m_tutrialColliderList;

    [Header("会話によるキャラクターの立ち位置のリスト(StageStateごとにリスト化)")]
    [SerializeField]
    private List<NPCChangePositionInformation> m_changeCharacterTransformList;

    [Header("Bossキャラクター")]
    [SerializeField]
    private Transform m_BossCharacter;
    public Transform BossChara { get { return m_BossCharacter; } }

    [Header("Boss後のNPC会話Positionの親")]
    [SerializeField]
    private Transform m_NPCPositisonsParentTrans;
    public Transform NPCPositionsParentTrans => m_NPCPositisonsParentTrans;


    // 現在のStateから上記の当たり判定リストにアクセスし、
    // 対応したコライダーとの当たり判定がされたかを確認する
    public bool CheckOnColliderEnterCollisionList(AkazukinStageState nowState)
    {
        if (m_tutrialColliderList.Count == 0)
        {
            Debug.LogError("リストに登録されていません");
            return false;
        }

        return m_tutrialColliderList[nowState].IsColliderEnter;
    }

    public NPCChangePositionInformation GetChangeCharacterInformation(AkazukinStageState akazukinStageState,SpeakerType speakerType)
    {
        if (m_tutrialColliderList.Count == 0)
        {
            Debug.LogError("リストに登録されていません");
            return null;
        }

        NPCChangePositionInformation nPCChangePositionInformation = new NPCChangePositionInformation();

        foreach(var information in m_changeCharacterTransformList)
        {
            if(information.akazukinStageState == akazukinStageState && information.speakerType == speakerType)
            {
                nPCChangePositionInformation = information;
                break;
            }
        }
        return nPCChangePositionInformation;
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
            m_currentStateDevice.Value = (int)m_akazukinStageState;
        }
        else
        {
            // 一致するシーン名を取得
            foreach (var state in m_initilizeStateList)
            {
                if (beforeSceneName == state.SceneName)
                {
                    // 更新
                    m_currentStateDevice.Value = (int)m_akazukinStageState;
                    return;
                }
            }

            // 何もなければ
            m_currentStateDevice.Value = (int)m_initilizeState;
        }

    }


}

[System.Serializable]
public class NPCChangePositionInformation
{
    [SerializeField]
    private AkazukinStageState m_akazukinStageState;
    public AkazukinStageState akazukinStageState => m_akazukinStageState;

    [SerializeField]
    private SpeakerType m_speakerType;
    public SpeakerType speakerType => m_speakerType;

    [SerializeField]
    private Transform m_changeTransform;
    public Transform changeTransform => m_changeTransform;


}
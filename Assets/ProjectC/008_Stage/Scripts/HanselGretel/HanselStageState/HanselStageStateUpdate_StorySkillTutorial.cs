using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselStageStateUpdate_StorySkillTutorial : BaseHanselStageStateUpdate
{
    [Header("ウィンドウコントローラー")]
    [SerializeField]
    private ConversationWindowController m_conversationWindowController;

    [Header("ウィンドウコントローラー(チュートリアル)")]
    [SerializeField]
    private WindowController m_tutorialWindowController;

    [Header("話者タイプ")]
    [SerializeField]
    private SpeakerType m_speakerType = SpeakerType.None;

    [Header("ストーリータイプ")]
    [SerializeField]
    private StoryType m_storyType = StoryType.None;

    [Header("会話State変更")]
    [SerializeField]
    private TalkingFase talkingFase = TalkingFase.Fase1;


    private CharacterCore m_playerCore = null;
    private CharacterCore m_npcCore = null;
    private ConversationData m_conversationData = null;


    public override async UniTask OnInitialize()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            //TalkingFaseの変更
            m_conversationData = ConversationDataBaseManager.instance.GetConvaersationData(m_storyType);
            m_conversationData.TalkingFase = talkingFase;

            // ウィンドウを作成/処理
            await CreateWindow();
            cancelToken.ThrowIfCancellationRequested();

            await UniTask.CompletedTask;
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }



    private async UniTask CreateWindow()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_conversationWindowController == null)
            {
                Debug.LogError("WindowControllerがシリアライズされていません");
                return;
            }

            if (m_tutorialWindowController == null)
            {
                Debug.LogError("WindowControllerがシリアライズされていません");
                return;
            }


            // カメラターゲットを取得し渡す
            Transform camTargetTrans = null;
            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (core.GroupNo == CharacterGroupNumber.player)
                {
                    m_playerCore = core;
                   // m_playerCore.m_animator.SetTrigger("ShowCutScene");
                    continue;
                }


                if ((core.GroupNo == CharacterGroupNumber.NPC)
                && (core.NPCParameters.SpeakerType == m_speakerType))
                {
                    m_npcCore = core;
                    camTargetTrans = core.NPCParameters.CameraTrans;

                }
            }

            if (camTargetTrans == null)
            {
                Debug.LogError("フィールドに該当のNPCが存在しません");
                return;
            }

            //アイコン消す
            m_npcCore?.NPCParameters.LandMarkCanvasTrans.gameObject.SetActive(false);

            m_conversationWindowController = Instantiate(m_conversationWindowController);

            // 処理を行う
            await m_conversationWindowController.CreateConversationWindow<BaseWindow>(m_speakerType,m_storyType, camTargetTrans);
            cancelToken.ThrowIfCancellationRequested();

            // プレイヤーのスキルをヘンゼルスキルに変更(ヘンゼルとグレーテルスキル取得)
            m_playerCore.PlayerParameters.StorySkill1_ID.Value = StorySkill_ID.HanselGretel;

            // スキルが直ぐに発動できるように
            m_playerCore.PlayerParameters.PlayerStatus.m_bpSkill_1.Value = m_playerCore.PlayerParameters.PlayerStatus.MaxBPSkill_1;

            // ヘンゼルとグレーテル解放
            if(StorySkillDataBaseManager.instance)
            {
                StorySkillDataBaseManager.instance.GetStorySkillData(StorySkill_ID.HanselGretel).MasterSkill();
            }

            // 童話スキルセーブ
            m_playerCore.PlayerParameters.SaveStorySkill();

            // チュートリアルウィンドウ表示
            m_tutorialWindowController = Instantiate(m_tutorialWindowController);
            await m_tutorialWindowController.CreateWindow<BaseWindow>();
            cancelToken.ThrowIfCancellationRequested();


            //会話ウィンドウ終わったらプレイヤー戻す
            m_playerCore.m_animator.SetTrigger("ReturnIdle");

            DestoryControllerWindow();
            // 終了
            SetEnd(m_nextHanselStageState);

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    // ウィンドウを削除する
    void DestoryControllerWindow()
    {
        // 削除
        Destroy(m_conversationWindowController.gameObject);
        Destroy(m_tutorialWindowController.gameObject);
    }
}

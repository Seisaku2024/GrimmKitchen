using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyStateUpdate_StoryConversation02 : BaseLobbyStateUpdate
{
    [Header("ウィンドウコントローラー")]
    [SerializeField]
    private ConversationWindowController m_conversationWindowController;

    [Header("話者タイプ")]
    [SerializeField]
    private SpeakerType m_speakerType = SpeakerType.None;

    [Header("ストーリータイプ")]
    [SerializeField]
    private StoryType m_storyType=StoryType.None;


    [Header("会話State変更")]
    [SerializeField]
    private TalkingFase talkingFase = TalkingFase.Fase1;

    private CharacterCore m_playerCore = null;
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

    override public async UniTask OnExit()
    {
        DestoryControllerWindow();


        //もとに戻す
        if (m_playerCore)
        {
            //m_playerCore.m_animator.SetTriggerOneShot("ReturnIdle");
        }


        await UniTask.CompletedTask;
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

            // カメラターゲットを取得し渡す
            Transform camTargetTrans = null;
            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (core.GroupNo == CharacterGroupNumber.player)
                {
                    m_playerCore = core;
                    //m_playerCore.m_animator.SetTrigger("ShowCutScene");
                    continue;
                }
            }

            if (m_playerCore)
            {
                camTargetTrans = m_playerCore.transform;
            }

            if (camTargetTrans == null)
            {
                Debug.LogError("フィールドに該当のNPCが存在しません");
                return;
            }

            m_conversationWindowController = Instantiate(m_conversationWindowController);

            // カメラ変更しない
            m_conversationWindowController.ChangeCamera = false;

            // 処理を行う
            await m_conversationWindowController.CreateConversationWindow<BaseWindow>(m_speakerType,m_storyType, camTargetTrans);
            cancelToken.ThrowIfCancellationRequested();


            // 削除
            DestoryControllerWindow();

            CutSceneManager.instance.PlayCutScene(CutSceneNumber.InTheBook);

            // 終了
            //SetEnd(m_nextLobbyState);
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
    }

}

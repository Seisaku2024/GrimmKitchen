using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Cinemachine;

public class AkazukinStageStateUpdate_TalkingEvent : BaseAkazukinStageStateUpdate
{
    [Header("ウィンドウコントローラー")]
    [SerializeField]
    protected ConversationWindowController m_conversationWindowController;

    [Header("話者タイプ")]
    [SerializeField]
    protected SpeakerType m_speakerType = SpeakerType.None;

    [Header("ストーリータイプ")]
    [SerializeField]
    protected StoryType m_storyType=StoryType.None;

    [Header("会話State変更")]
    [SerializeField]
    protected TalkingFase m_talkingFase = TalkingFase.Fase1;

    protected CharacterCore m_playerCore = null;
    protected CharacterCore m_npcCore = null;
    protected ConversationData m_conversationData = null;


    public override async UniTask OnInitialize()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {

            // TalkingFaseの変更
            m_conversationData = ConversationDataBaseManager.instance.GetConvaersationData(m_storyType);
            m_conversationData.TalkingFase = m_talkingFase;


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

        await WaitUntilBlendComplete();


        Transform camLookTrans = null;
        Transform camFollowTrans = null;

        //カットシーンカメラのターゲットを主人公に戻す
        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (core.GroupNo == CharacterGroupNumber.player)
            {
                camLookTrans = core.PlayerParameters.ConversationCameraTransform;
                camFollowTrans = core.transform;
                break;
            }
        }
        CutSceneManager.instance.SetConversationCammeraLookAt(camLookTrans);
        CutSceneManager.instance.SetConversationCammeraFollow(camFollowTrans);


        //もとに戻す
        if (m_playerCore)
        {
            m_playerCore.m_animator.SetTrigger("ReturnIdle");
        }


        await UniTask.CompletedTask;
    }


    protected virtual async UniTask CreateWindow()
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
            //m_npcCore?.NPCParameters.LandMarkCanvasTrans.gameObject.SetActive(false);

            m_conversationWindowController = Instantiate(m_conversationWindowController);

            // 処理を行う
            await m_conversationWindowController.CreateConversationWindow<BaseWindow>(m_speakerType,m_storyType, camTargetTrans);
            cancelToken.ThrowIfCancellationRequested();

            // 削除
            DestoryControllerWindow();

            // 終了
            SetEnd(m_nextAkazukinStageState);
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    // ウィンドウを削除する
    protected virtual void DestoryControllerWindow()
    {
        // 削除
        Destroy(m_conversationWindowController.gameObject);
    }

    
    async Task WaitUntilBlendComplete()
    {
        if (Camera.main.TryGetComponent(out CinemachineBrain brain))
        {
            // ブレンドが始まるまで待機
            while (!brain.IsBlending)
            {
                await Task.Yield();
            }

            // ブレンド中
            while (brain.IsBlending)
            {
                await Task.Yield();
            }
        }
    }


}

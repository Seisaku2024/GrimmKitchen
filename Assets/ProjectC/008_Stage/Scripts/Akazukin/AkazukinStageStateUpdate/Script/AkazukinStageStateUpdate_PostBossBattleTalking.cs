using Arbor.Examples;
using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkazukinStageStateUpdate_PostBossBattleTalking : AkazukinStageStateUpdate_TalkingEvent
{
    [Header("アクティブを操作するNPCリスト（Key:SpeakType,Value:アクティブにするかどうか）")]
    [SerializeField]
    private SerializableDictionary<SpeakerType, bool> m_changeNPCActiveList;

    private List<CharacterCore> m_npcCoreList = new List<CharacterCore>();

    private CharacterCore m_player;

    public override UniTask OnInitialize()
    {
        foreach (var character in m_changeNPCActiveList)
        {
            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (core.GroupNo == CharacterGroupNumber.NPC
                    && core.NPCParameters.SpeakerType == character.Key)
                {
                    // 会話時にどこにいてもプレイヤーの方を見てほしいから感知距離を100に設定
                    core.NPCParameters.NPCNoticePlayerRange = 100f;
                    m_npcCoreList.Add(core);
                    break;
                }
            }
        }

        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if(core.GroupNo == CharacterGroupNumber.player)
            {
                m_player = core;
            }
        }

        // Parentを回転させる
        Transform parentNPCTrans = AkazukinStageUpdateManager.instance.NPCPositionsParentTrans;
        Vector3 direction = m_player.transform.position - parentNPCTrans.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            parentNPCTrans.rotation = targetRotation;
        }


        return base.OnInitialize();
    }

    private void SetActiveNPCCharacter()
    {
        if (m_changeNPCActiveList.Count != 0)
        {
            foreach (var character in m_changeNPCActiveList)
            {
                foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
                {
                    if (core.GroupNo == CharacterGroupNumber.NPC
                        && core.NPCParameters.SpeakerType == character.Key)
                    {
                        core.gameObject.SetActive(character.Value);
                        // 会話時にどこにいてもプレイヤーの方を見てほしいから感知距離を100に設定
                        core.NPCParameters.NPCNoticePlayerRange = 100f;
                        // 一度アニメーション初期化
                        ResetAllBools(core.m_animator);

                        m_npcCoreList.Add(core);    
                        break;
                    }
                }
            }
        }
    }


    private void SetPositionNPC()
    {
        if (m_changeNPCActiveList.Count != 0)
        {
            foreach (var character in m_changeNPCActiveList)
            {
                var changeCharacterInfomation = AkazukinStageUpdateManager.instance.
                    GetChangeCharacterInformation(m_akazukinStageState, character.Key);

                foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
                {
                    if (core.GroupNo == CharacterGroupNumber.NPC
                        && core.NPCParameters.SpeakerType == changeCharacterInfomation.speakerType)
                    {
                        core.CharaCtrl.SetPositionMotor(changeCharacterInfomation.changeTransform.position);

                        // 一度アニメーション初期化
                        ResetAllBools(core.m_animator);

                        break;
                    }
                }
            }
        }
    }

    void ResetAllBools(Animator animator)
    {
        animator.SetTriggerOneShot("IsFinish");

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(param.name, false);
            }
        }
    }

    private void TransitionEvent()
    {
        SetActiveNPCCharacter();
        SetPositionNPC();
    }


    protected override async UniTask CreateWindow()
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
                    m_playerCore.m_animator.SetTrigger("ShowCutScene");
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

            m_conversationWindowController = Instantiate(m_conversationWindowController);

            // 処理を行う
            await m_conversationWindowController.CreateConversationWindow<BaseWindow>
                (m_speakerType, m_storyType, camTargetTrans, false, TransitionEvent);
            cancelToken.ThrowIfCancellationRequested();

            // 削除
            DestoryControllerWindow();

            // 赤ずきんステージのストーリー終了を保存
            StoryProgressManager.instance.GetStoryProgressData(StoryProgressType.CompleteAkazukinStory).SetFinish();

            // 感知距離を元に戻す
            foreach (var core in m_npcCoreList)
            {
                core.NPCParameters.NPCNoticePlayerRange = 10f;
            }

            // 終了
            SetEnd(m_nextAkazukinStageState);
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    

}

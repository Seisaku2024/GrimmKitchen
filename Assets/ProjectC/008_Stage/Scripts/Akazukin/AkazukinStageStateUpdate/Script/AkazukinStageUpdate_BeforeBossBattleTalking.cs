using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkazukinStageUpdate_BeforeBossBattleTalking : AkazukinStageStateUpdate_TalkingEvent
{
    [Header("アクティブを操作するNPCリスト（Key:SpeakType,Value:アクティブにするかどうか）")]
    [SerializeField]
    private SerializableDictionary<SpeakerType, bool> m_changeNpcActiveList;

    [Header("アクティブを操作する敵のリスト（Key:GropeNo,Value:アクティブにするかどうか）")]
    [SerializeField]
    private SerializableDictionary<EnemyID, bool> m_changeNonNpcActiveList;

    private void SetActiveCharacter()
    {
        if (m_changeNpcActiveList.Count != 0)
        {
            foreach (var character in m_changeNpcActiveList)
            {
                foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
                {
                    if (core.GroupNo == CharacterGroupNumber.NPC
                        && core.NPCParameters.SpeakerType == character.Key)
                    {
                        core.gameObject.SetActive(character.Value);
                        break;
                    }
                }
            }
        }



        if (m_changeNonNpcActiveList.Count != 0)
        {
            foreach (var character in m_changeNonNpcActiveList)
            {
                foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
                {
                    if (core.GroupNo == CharacterGroupNumber.enemy
                        && core.EnemyParameters.GetEnemyData().EnemyID == character.Key)
                    {
                        core.gameObject.SetActive(character.Value);
                        break;
                    }
                }
            }
        }
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

            m_conversationWindowController = Instantiate(m_conversationWindowController);

            // 処理を行う
            await m_conversationWindowController.CreateConversationWindow<BaseWindow>
                (m_speakerType, m_storyType, camTargetTrans,false,null,SetActiveCharacter);
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
}

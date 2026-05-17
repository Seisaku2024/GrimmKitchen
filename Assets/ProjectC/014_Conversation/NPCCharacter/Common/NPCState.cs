using Cysharp.Threading.Tasks;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UniRx;

// NPCキャラクターの基盤クラス
public partial class CharacterCore : MonoBehaviour, IDamageable
{
    [System.Serializable]
    [AddTypeMenu("NPC/Common/Idle")]
    public class ActionState_NPC_Idle : ActionState_Base
    {
        [Header("NPCが感知する範囲")]
        [SerializeField]
        private float m_noticeDist = 1.5f;

        private CharacterCore m_player;

        public override void OnEnter()
        {
            base.OnEnter();

            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (core.GroupNo == CharacterGroupNumber.player)
                {
                    m_player = core;
                    break;
                }
            }

            if (Core.NPCParameters)
                m_noticeDist = Core.NPCParameters.NPCNoticePlayerRange;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var dist = (m_player.transform.position - Core.transform.position).magnitude;

            if (m_noticeDist != Core.NPCParameters.NPCNoticePlayerRange)
                m_noticeDist = Core.NPCParameters.NPCNoticePlayerRange;

            if (dist <= m_noticeDist)
            {
                var targetVec = m_player.transform.position - Core.transform.position;

                Core.SetRotateToTarget(targetVec, false);
            }

        }

    }

    [System.Serializable]
    [AddTypeMenu("NPC/Common/IdleFaceNPC")]
    public class ActionState_NPC_IdleFaceNPC : ActionState_Base
    {
        [Header("NPCが感知する範囲")]
        [SerializeField]
        private float m_noticeDist = 1.5f;

        private CharacterCore m_faceCharacter;

        public override void OnEnter()
        {
            base.OnEnter();

            if (Core.NPCParameters == null)
            {
                return;
            }

            var targetCharacter = Core.NPCParameters.NPCFaceSpeakerType;


            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (core.GroupNo == CharacterGroupNumber.NPC &&
                    core.NPCParameters.SpeakerType == targetCharacter)
                {
                    m_faceCharacter = core;
                    break;
                }
            }

            // 会話中の変更に備える処理
            Core.NPCParameters.m_bChangetFaceSpeaker.Subscribe(flg =>
           {
               if (flg == false)
               {
                   return;
               }

               targetCharacter = Core.NPCParameters.NPCFaceSpeakerType;

               // 向くキャラクターを変更
               foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
               {
                   if(targetCharacter == Speaker.SpeakerType.Player 
                   && core.GroupNo==CharacterGroupNumber.player)
                   {
                       m_faceCharacter = core;
                       break;
                   }

                   if (core.GroupNo == CharacterGroupNumber.NPC &&
                       core.NPCParameters.SpeakerType == targetCharacter)
                   {
                       m_faceCharacter = core;
                       break;
                   }
               }

               flg = false;

           }).AddTo(Core.gameObject);

        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            var dist = (m_faceCharacter.transform.position - Core.transform.position).magnitude;

            if (dist <= m_noticeDist)
            {
                var targetVec = m_faceCharacter.transform.position - Core.transform.position;

                Core.SetRotateToTarget(targetVec, false);
            }

        }

    }


}

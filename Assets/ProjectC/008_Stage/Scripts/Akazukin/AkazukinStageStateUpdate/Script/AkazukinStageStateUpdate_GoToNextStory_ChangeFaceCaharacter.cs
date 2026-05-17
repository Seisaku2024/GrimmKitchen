using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MagicaCloth2;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class AkazukinStageStateUpdate_GoToNextStory_ChangeFaceCaharacter : AkazukinStageStateUpdate_GoToNextStory
{
    [SerializeField]
    private SpeakerType m_FacespeakerType;

    [Header("キャラクターの向き変更追加用(Key：向かせるキャラクター、Value：向く対象)")]
    [SerializeField]
    private SerializableDictionary<SpeakerType, SpeakerType> m_additionalChangeFaceCharacterList;

    [Header("キャラクターのアニメーション処理(Key：キャラクター、Value：アニメーション名)")]
    [SerializeField]
    private SerializableDictionary<SpeakerType, string> m_changeAnimationCharacterList;

    private Dictionary<CharacterCore, SpeakerType> m_changeList = new();

    public override async UniTask OnInitialize()
    {
        await UniTask.DelayFrame(1);

        await base.OnInitialize();

        //ToDo:本来は別の場所に（CheckStateとか）

        var manager = AkazukinStageUpdateManager.instance;
        if (manager != null && manager.BossChara.gameObject.activeSelf == true)
        {
            manager.BossChara.gameObject.SetActive(false);
        }


        if (m_npcCore)
        {
            m_npcCore.NPCParameters.NPCFaceSpeakerType = m_FacespeakerType;
            m_npcCore.m_animator.SetBool("FaceTargetIdle", true);
        }

        // 追加のキャラクター達の変更
        if (m_additionalChangeFaceCharacterList.Count == 0)
        {
            return;
        }

        foreach (var character in m_additionalChangeFaceCharacterList)
        {
            CharacterCore keyCore = null;

            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if ((core.GroupNo == CharacterGroupNumber.NPC)
                && (core.NPCParameters.SpeakerType == character.Key))
                {
                    keyCore = core;
                    break;
                }

            }

            m_changeList.Add(keyCore, character.Value);

        }

        foreach(var core in m_changeList)
        {
            core.Key.NPCParameters.NPCFaceSpeakerType = core.Value;
            core.Key.m_animator.SetBool("FaceTargetIdle", true);
        }

        if(m_changeAnimationCharacterList.Count==0)
        {
            return;
        }

        await UniTask.DelayFrame(1);

        // リストのアニメーション変更
        foreach (var character in m_changeAnimationCharacterList)
        {
            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if(core.GroupNo==CharacterGroupNumber.NPC
                    && core.NPCParameters.SpeakerType == character.Key)
                {
                    core.m_animator.SetBool(character.Value, true);
                    break;
                }
            }
        }

    }

}

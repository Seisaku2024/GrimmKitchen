using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselStageStateUpdate_GoToMoveTutrial : BaseHanselStageStateUpdate
{
    // 移動チュートリアルへ移行するかどうか判断するState（山本）
    [Header("話者タイプ")]
    [SerializeField]
    private SpeakerType m_speakerType = SpeakerType.None;
    private CharacterCore m_npcCore = null;

    public override UniTask OnInitialize()
    {

        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if ((core.GroupNo == CharacterGroupNumber.NPC)
            && (core.NPCParameters.SpeakerType == m_speakerType))
            {
                m_npcCore = core;

            }
        }

        //NPCの目印用マークを表示する
        if (m_npcCore)
        {
            m_npcCore.NPCParameters?.LandMarkCanvasTrans.gameObject?.SetActive(true);
        }

        return base.OnInitialize();

    }

    public override async UniTask OnUpdate()
    {
        await base.OnUpdate();

        if (HanselStageStateUpdateManager.instance == null)
        {
            Debug.LogError("マネージャーが存在しません");
            return;
        }

        if (HanselStageStateUpdateManager.instance.
            CheckOnColliderEnterCollisionList(m_hanselStageState) == true)
        {
            if (m_npcCore)
            {
                m_npcCore.NPCParameters?.LandMarkCanvasTrans.gameObject?.SetActive(false);
            }

            SetEnd(m_nextHanselStageState);
        }

    }
}

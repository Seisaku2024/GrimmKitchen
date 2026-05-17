using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselStageStateUpdate_NextPositionMove02 : BaseHanselStageStateUpdate
{
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

        if (m_npcCore)
        {
            m_npcCore.NPCParameters.MoveTargetPositionFlg = true;
            m_npcCore.NPCParameters.NoWaitPlayerFlg = true;
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

        // NPCが目的地に到達したなら次のStateに移行
        if (m_npcCore.NPCParameters.MoveTargetPositionFlg == false)
        {
            m_npcCore.NPCParameters.NoWaitPlayerFlg = false;
            SetEnd(m_nextHanselStageState);
        }

    }

}

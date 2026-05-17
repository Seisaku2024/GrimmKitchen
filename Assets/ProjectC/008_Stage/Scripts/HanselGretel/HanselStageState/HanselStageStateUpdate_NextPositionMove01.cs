using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselStageStateUpdate_NextPositionMove01 : BaseHanselStageStateUpdate
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
            //NPCの目印用マークを表示する
            m_npcCore.NPCParameters?.LandMarkCanvasTrans.gameObject?.SetActive(true);
        }

        return base.OnInitialize();

    }

    override public async UniTask OnUpdate()
    {
        await UniTask.CompletedTask;

        if (m_npcCore == null)
        {
            Debug.LogError("シリアライズでセットされたNPCが存在しません");
            return;
        }

        // NPCが目的地に到達したなら次のStateに移行
        if (m_npcCore.NPCParameters.MoveTargetPositionFlg == false)
        {
            if (m_npcCore)
            {
                //NPCの目印用マークを非表示する
                m_npcCore.NPCParameters?.LandMarkCanvasTrans.gameObject?.SetActive(false);
            }

            SetEnd(m_nextHanselStageState);
        }

    }


}

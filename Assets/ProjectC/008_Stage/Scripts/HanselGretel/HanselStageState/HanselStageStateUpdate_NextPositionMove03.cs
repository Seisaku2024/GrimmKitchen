using Arbor;
using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselStageStateUpdate_NextPositionMove03 : BaseHanselStageStateUpdate
{
    [Header("話者タイプ")]
    [SerializeField]
    private SpeakerType m_speakerType = SpeakerType.None;
    private CharacterCore m_npcCore = null;

    [Header("NPCが停止する距離")]
    [SerializeField]
    private float m_changeDist = 20.0f;

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
            //ストップする範囲を拡大する
            m_npcCore.NPCParameters.StopMoveDistance = m_changeDist;
            m_npcCore.NPCParameters.MoveTargetPositionFlg = true;
            m_npcCore.NPCParameters.NoChangeTurnAroundFlg = true;
            //NPCの目印用マークを表示する
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

        // NPCが目的地に到達したなら次のStateに移行
        if (m_npcCore.NPCParameters.MoveTargetPositionFlg == false)
        {
            if (m_npcCore)
            {
                //ストップする範囲を初期化
                m_npcCore.NPCParameters.StopMoveDistance = m_npcCore.NPCParameters.InitStopMoveDist;
                m_npcCore.NPCParameters.NoChangeTurnAroundFlg = false;
                //NPCの目印用マークを表示する
                m_npcCore.NPCParameters?.LandMarkCanvasTrans.gameObject?.SetActive(false);
            }

            SetEnd(m_nextHanselStageState);

        }
    }
}

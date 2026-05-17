using Cysharp.Threading.Tasks;
using Speaker;
using UnityEngine;

public class AkazukinStageStateUpdate_GoToNextStory : BaseAkazukinStageStateUpdate
{
    [Header("話者タイプ")]
    [SerializeField]
    protected SpeakerType m_speakerType = SpeakerType.None;
    protected CharacterCore m_npcCore = null;

    public override async UniTask OnInitialize()
    {

        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if ((core.GroupNo == CharacterGroupNumber.NPC)
            && (core.NPCParameters.SpeakerType == m_speakerType))
            {
                m_npcCore = core;
                break;

            }
        }

        if (m_npcCore == null)
        {
            await UniTask.Yield();

            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if ((core.GroupNo == CharacterGroupNumber.NPC)
                && (core.NPCParameters.SpeakerType == m_speakerType))
                {
                    m_npcCore = core;
                    break;

                }
            }

        }

        //NPCの目印用マークを表示する
        if (m_npcCore && m_npcCore.NPCParameters?.LandMarkCanvasTrans != null)
        {
            m_npcCore.NPCParameters?.LandMarkCanvasTrans?.gameObject?.SetActive(true);
        }

        await base.OnInitialize();

    }

    public override async UniTask OnUpdate()
    {
        await base.OnUpdate();

        if (AkazukinStageUpdateManager.instance == null)
        {
            Debug.LogError("マネージャーが存在しません");
            return;
        }

        if (AkazukinStageUpdateManager.instance.
            CheckOnColliderEnterCollisionList(m_akazukinStageState) == true)
        {
            if (m_npcCore && m_npcCore.NPCParameters?.LandMarkCanvasTrans != null)
            {
                m_npcCore.NPCParameters?.LandMarkCanvasTrans?.gameObject?.SetActive(false);
            }

            SetEnd(m_nextAkazukinStageState);
        }

    }



}

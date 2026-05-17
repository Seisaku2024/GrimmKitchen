using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterStorySkillReward : BaseRewardChallengeData
{
    [Header("取得する童話スキルのID")]
    [SerializeField]
    private StorySkill_ID m_skillID = StorySkill_ID.None;

    [Header("スキル取得時に表示されるウィンドウ")]
    [SerializeField]
    private MasterSkillWindowController m_windowController = null;

    public override async void UpdateRewardChallenge()
    {
        if(StorySkillDataBaseManager.instance==null)
        {
            return;
        }

        var data = StorySkillDataBaseManager.instance.GetStorySkillData(m_skillID);

        if(data==null)
        {
            return;
        }

        if(m_windowController)
        {
            m_windowController = Instantiate(m_windowController);
            await m_windowController.CreateMasterSkillWindow<MasterStorySkillWindow>(m_skillID);
        }

        if(data.MasterFlg==false)
        data.MasterSkill();

        MasterStorySkillDataSaveLoad newData = new(data.StorySkill_ID, data.MasterStorySkillData);
        _= StorySkillDataSaveLoader.Save(m_skillID, newData);

    }
}

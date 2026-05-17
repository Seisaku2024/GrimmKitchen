using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//早めに生成する（山本）
[DefaultExecutionOrder(-100)]
public class StorySkillDataBaseManager : BaseManager<StorySkillDataBaseManager>
{
    //童話スキルデータベースのマネージャー（山本）

    [Header("童話スキルのデータベース")]
    [SerializeField] private StorySkillDataBase m_storySkillDataBase;

    public StorySkillDataBase DataBase { get { return m_storySkillDataBase; } }

    [Header("最初に装備する童話スキルのID")]
    [Header("*基本的にはNoneでデバックとして触れるように")]
    [SerializeField] private StorySkill_ID m_skill_ID1 = StorySkill_ID.None;
    [SerializeField] private StorySkill_ID m_skill_ID2 = StorySkill_ID.None;
    public StorySkill_ID StorySkill_ID1 { get { return m_skill_ID1; } }
    public StorySkill_ID StorySkill_ID2 { get { return m_skill_ID2; } }

    
    //-------------------------------------------------------------------------

    //童話スキルIDの対応データを返す

    public StorySkillData GetStorySkillData(StorySkill_ID _id)
    {
        if (m_storySkillDataBase == null)
        {
            Debug.LogError("童話スキルのデータベースが空です！");
            return null;
        }

        //返すデータ
        StorySkillData storySkillData = null;


        foreach (var data in m_storySkillDataBase.StorySkillDataList)
        {

            //データのIDと引数のIDが一致すればそれを返す
            if (data.StorySkill_ID == _id)
            {
                storySkillData = data;
                break;
            }


        }

        //ストーリースキル空だったら（探して見つからなかったら）
        if (storySkillData == null)
        {
            Debug.LogError("指定したIDの童話スキルは登録されてません。");
            return null;
        }


        return storySkillData;

    }

    protected override void Load()
    {
        foreach (var data in m_storySkillDataBase.StorySkillDataList)
        {
            if (data == null) continue;
            var saveLoadData = StorySkillDataSaveLoader.Load(data.StorySkill_ID);
            data.Load(saveLoadData);
        }
    }

}

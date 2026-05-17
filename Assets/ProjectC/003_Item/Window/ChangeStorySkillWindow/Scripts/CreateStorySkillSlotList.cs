using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStorySkillSlotList : BaseCreateSlotList
{
    [SerializeField]
    private ChangeStorySkillWindow m_changeStorySkillWindow = null;

    // 童話スキルのスロット作成する
    protected override async UniTask CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットがシリアライズされていません");
            return;
        }

        foreach (var data in StorySkillDataBaseManager.instance.DataBase.StorySkillDataList)
        {
            if (data == null) continue;

            if (data.StorySkill_ID == StorySkill_ID.None) continue;

            if (m_changeStorySkillWindow == null) return;

            if (data.MasterFlg == false) continue;

            // 作成
            var createSlot = Instantiate(m_slot, transform);

            if (createSlot.TryGetComponent<StorySkillSlotData>(out var slotData))
            {
                if (m_changeStorySkillWindow.SetStorySkillNo == StorySkillType.StorySkillNo.Skill1)
                {
                    slotData.SetStorySkillData(data, true);
                }
                else
                {
                    slotData.SetStorySkillData(data, false);
                }
            }
            else
            {
                Debug.LogError("作成しようとしているStaffStatusSlotDataがアタッチされていません");
            }


            if (slotData.StorySkillData.MasterFlg)
            {
                // リストに追加
                m_slotList.Add(createSlot);
                // UIcontrollerに追加
                AddSelectUIControler(createSlot);
            }

            

        }

        await UniTask.CompletedTask;

    }

}

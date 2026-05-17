using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;

public class CreateStaffStatusUpSlotList : BaseCreateSlotList
{

    protected override async UniTask CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        // 既存スロットを削除
        DestroySlotList();


        foreach (var data in StaffStatusUpDataBaseManager.instance.StaffStatusDataBase.StaffStatusUpDataList)
        {
            // Noneの場合は表示しない
            if (data.StaffStatusUpID == StaffStatusUpID.None) continue;


            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

             if (slot.TryGetComponent<StaffStatusUpSlotData>(out var slotData))
             {
                 slotData.SetData(data);
             }
             else
             {
                 Debug.LogError("StaffStatusUpSlotDataコンポーネントがアタッチされていません");
             }

            // リストに追加
            m_slotList.Add(slot);

            // UIcontrollerに追加
            AddSelectUIControler(slot);

        }

        await UniTask.CompletedTask;


    }
}

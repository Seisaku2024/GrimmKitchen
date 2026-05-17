using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using StaffInfo;

public class CreateStaffPointSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // スタッフマネージャーに登録されているスタッフデータのスロット作成

    [Header("スタッフタイプ")]
    [SerializeField]
    private StaffType m_staffType = StaffType.Hall;

    //======================================================
    //                  実行処理
    //======================================================


    protected override async UniTask CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("スロットがシリアライズされていません");
            return;
        }

        foreach (var data in StaffManager.instance.StaffPointDataList)
        {
            if (data == null) continue;
            if (data.SetStaffType != m_staffType || data.IsUnlock() == false) continue;

            // 子オブジェクトにスロット作成
            var slot = Instantiate(m_slot, gameObject.transform);

            if (slot.TryGetComponent<StaffPointSlotData>(out var slotData))
            {
                slotData.SetStaffPointData(data);
                slotData.SetStaffStatusData(data.StaffStatusData);
            }
            else
            {
                Debug.LogError("StaffPointSlotDataがシリアライズされていません");
            }

            m_slotList.Add(slot);

            // コントローラーに追加
            AddSelectUIControler(slot);
        }

        await UniTask.CompletedTask;
    }


}

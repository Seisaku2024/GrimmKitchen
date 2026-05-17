using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class CreateSelectStaffStorageSlotList : CreateStaffStorageSlotList
{

    [SerializeField]
    private StaffStatusSlotData m_noneSlot = null;



    protected override async UniTask CreateSlotInstance()
    {

        if (m_noneSlot == null)
        {
            Debug.LogError("NoneSlotがシリアライズされていません");
            return;
        }

        try
        {
            // Noneスロット作成
            var createSlot = Instantiate(m_noneSlot, transform);
            createSlot.SetStaffStatusData(null);
            m_slotList.Add(createSlot.gameObject); 
            AddSelectUIControler(createSlot.gameObject); 

            await base.CreateSlotInstance();

        }
        catch(System.ObjectDisposedException ex)
        {
            Debug.Log(ex);
        }
        await UniTask.CompletedTask;

    }
}

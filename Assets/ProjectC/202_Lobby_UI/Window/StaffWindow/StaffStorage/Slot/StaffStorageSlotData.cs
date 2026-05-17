using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class StaffStorageSlotData : StaffStatusSlotData
{
    // 制作者 田内
    // スタッフストレージ用スロット


    //================================================
    //              実行処理
    //================================================


    protected void Start()
    {
        StaffManager.instance.StaffStorageRC.ObserveCountChanged().Subscribe(_ =>
        {
            if (StaffManager.instance.IsAddedStaffStorage(m_staffStatusData) == false)
            {
                // 初期化
                m_staffStatusData = null;
                InitializeSlotData();
            }
        }
        ).AddTo(this);
    }
}

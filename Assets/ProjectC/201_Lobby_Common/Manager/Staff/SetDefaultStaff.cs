using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDefaultStaff : MonoBehaviour
{
    // 制作者 田内
    // デフォで所持中のスタッフをセットする

    [SerializeField]
    private bool m_isDebug = true;

    //===========================================
    //              実行処理
    //===========================================

    private void Start()
    {
#if UNITY_EDITOR

        if (m_isDebug == false) return;

        foreach (var storageStaffData in StaffManager.instance.StaffStorageRC)
        {
            if (storageStaffData == null) continue;

            // 空きポイントにセットする
            foreach (var pointData in StaffManager.instance.StaffPointDataList)
            {
                if (pointData == null || pointData.StaffStatusData != null) continue;


                StaffManager.instance.SetStaffPointStaffStatus(pointData, storageStaffData);
                break;
            }
        }
 
#endif
    }
}
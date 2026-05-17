using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectKeepRandomStaffControllerInputActionButton :InputActionButton
{
    // 制作者 田内
    // 総雇用金額が所持金額を超えている場合押せないようにする

    [Header("SelectKeepRandomStaffController")]
    [SerializeField]
    private SelectKeepRandomStaffController m_selectKeepRandomStaffController = null;

    //=================================================================
    //                      実行処理
    //=================================================================


    protected override bool IsPress()
    {
        if(m_selectKeepRandomStaffController==null)
        {
            Debug.LogError("SelectKeepRandomStaffControllerがシリアライズされていません");
            return false;
        }

        // 選択中の総雇用金額が所持金額を超えていれば
        if (ManagementDataManager.instance.TotalEarnedMoney < m_selectKeepRandomStaffController.TotalEmploymentPrice)
        {
            return false;
        }

        return true;
    }

}

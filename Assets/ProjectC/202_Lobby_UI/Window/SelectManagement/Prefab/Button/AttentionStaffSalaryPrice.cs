using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionStaffSalaryPrice : WindowUpdateBase
{
    // 制作者 田内
    // 給料

    public override void OnInitialize()
    {
        if (StaffManager.instance.IsSettingStaffPoint() == false)
        {
            gameObject.SetActive(false);
            return;
        }

        if (StaffManager.instance.IsSalaryPriceStaffPoint() == false)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public override void OnUpdate()
    {
        if (StaffManager.instance.IsSettingStaffPoint() == false)
        {
            gameObject.SetActive(false);
            return;
        }

        if (StaffManager.instance.IsSalaryPriceStaffPoint() == false)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

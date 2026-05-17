using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 制作者　吉田
/// シェフ・ホールスタッフ がいない時に表示する
/// </summary>
public class AttentionStaff : WindowUpdateBase
{

    public override void OnInitialize()
    {
        if (!StaffManager.instance.IsSettingStaffPoint())
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
        if(!StaffManager.instance.IsSettingStaffPoint())
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

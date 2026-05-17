using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class AddSatisfactionValueCustomer : BaseCustomerStateBehaviour
{
    // 制作者 田内
    // 満足値を追加

    //=========================================
    //              実行処理
    //=========================================

    // 満足値追加
    private void AddSatisfactionValue()
    {
        var data = GetCustomerData();
        if (data == null) return;

        ManagementGameDataManager.instance.AddSatisfactionValue(data.SatisfactionValue);

        data.SatisfactionValue = 0;
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        AddSatisfactionValue();
    }

}

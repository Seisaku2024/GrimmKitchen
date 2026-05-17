using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class DisposeSatisfactionValueCustomer : BaseCustomerStateBehaviour
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

        int dispose = -data.SatisfactionValue * 2;
        ManagementGameDataManager.instance.AddSatisfactionValue(dispose);

        data.SatisfactionValue = 0;
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        AddSatisfactionValue();
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/CalcEndPosCustomer")]
[AddComponentMenu("")]
public class CalcEndPosCustomer : BaseCustomerCalculator
{
    // 制作者 田内
    // 帰還座標を取得

    // Use this for calculate
    //=====================================
    //          実行処理
    //=====================================

    // Use this for calculate
    public override void OnCalculate()
    {
        var data = GetCustomerData();
        if (data == null) return;

        var pos = CustomerPasserbyRootManager.instance.RandomAppearPos;

        m_outputPos.SetValue(pos);
    }
}

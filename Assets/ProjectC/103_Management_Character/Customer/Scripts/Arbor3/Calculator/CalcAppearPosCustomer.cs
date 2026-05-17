using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/CalcAppearPosCustomer")]
[AddComponentMenu("")]
public class CalcAppearPosCustomer : BaseCustomerCalculator
{

    // 出現位置の座標を取得する
    // 制作者　田内

    //=====================================
    //          実行処理
    //=====================================

    // Use this for calculate
    public override void OnCalculate()
    {
        var data = GetCustomerData();
        if (data == null) return;

        var pos = data.EndPos;

        m_outputPos.SetValue(pos);
    }

}

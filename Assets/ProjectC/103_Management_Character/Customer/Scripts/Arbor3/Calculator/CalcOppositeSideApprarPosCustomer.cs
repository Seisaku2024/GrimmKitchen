// 今いる側の反対側に出現/退出する位置を計算する
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcOppositeSideApprarPosCustomer : BaseCustomerCalculator
{
    public override void OnCalculate()
    {
        var data = GetCustomerData();
        if (data == null) return;

        var isRight = data.IsRightSide;

        Vector3 pos;

        if (isRight)
        {
            pos = CustomerPasserbyRootManager.instance.LeftSideAppearPos;
        }
        else
        {
            pos = CustomerPasserbyRootManager.instance.RightSideAppearPos;
        }

        data.IsRightSide = !isRight;

        m_outputPos.SetValue(pos);

    }

}

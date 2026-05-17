using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/CalcCustomerData")]
[AddComponentMenu("")]
public class CalcCustomerData : Calculator
{
    // 制作者 田内

    //======================================
    // 客情報

    [Header("客情報")]
    [SerializeField]
    protected FlexibleCustomerDataVariable m_flexibleCustomerDataVariable = null;


    //=========================================
    // 出力

    [Header("出力:スタッフデータ")]
    [SerializeField]
    protected OutputSlotCustomerDataVariable m_customerData = new();


    public override void OnCalculate()
    {
        // 客情報が存在するか確認

        if (m_flexibleCustomerDataVariable == null)
        {
            Debug.LogError("客情報がシリアライズされていません");
            return;
        }

        if (m_flexibleCustomerDataVariable.value.CustomerData == null)
        {
            Debug.LogError("客情報が存在しません");
            return;
        }

        m_customerData.SetValue(m_flexibleCustomerDataVariable.value);
    }
}

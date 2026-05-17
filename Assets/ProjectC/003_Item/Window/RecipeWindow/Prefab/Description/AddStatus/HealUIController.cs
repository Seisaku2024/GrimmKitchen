using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealUIController : FoodWindowUpdateBase
{

    [SerializeField]
    private TextMeshProUGUI m_value = null;

    public override void OnInitialize()
    {

    }

    public override void OnUpdate(FoodData _foodData)
    {
        gameObject.SetActive(false);
        if (_foodData == null) return;

        //FoodTypeがHeal以外は表示しない
        if (!_foodData.IsFoodType(FoodData.FoodType.Heal)) return;

        // 表示
        gameObject.SetActive(true);
        uint healValue = _foodData.HealingValue();
        SetHealValue((float)healValue);
    }

    public void SetHealValue(float _value)
    {
        if (m_value != null)
        {
            m_value.text = _value.ToString();
        }
    }
}

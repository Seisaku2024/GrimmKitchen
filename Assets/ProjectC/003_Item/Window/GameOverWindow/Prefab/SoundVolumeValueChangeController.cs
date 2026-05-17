using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(10)]
public class SoundVolumeValueChangeController : ValueController
{

    public override UniTask OnUpdate()
    {
        return base.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
    }


    public override bool IsDecision()
    {
        return true;
    }

    override protected void SetData()
    {
        m_maxValue = 10;
        m_minValue = 0;
        m_currentValue = (int)(Math.Round(m_valueSlider.value,2) * 10.0f);

        SetSliderValue();

    }

    override protected void SetSliderValue()
    {
        if (m_valueSlider == null) return;

        m_valueSlider.minValue = m_minValue;
        m_valueSlider.maxValue = m_maxValue;
        m_valueSlider.value = m_currentValue;

        if (m_text != null)
        {
            int textValue = (int)(m_currentValue * 10.0f);
            m_text.text = textValue.ToString();
        }
    }


}

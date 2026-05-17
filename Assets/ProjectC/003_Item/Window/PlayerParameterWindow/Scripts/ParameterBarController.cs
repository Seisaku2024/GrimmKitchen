using Arbor;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParameterBarController : MonoBehaviour
{
    [Tooltip("色や長さ変更するImageを設定")]
    [SerializeField] private Image m_image;
    [Tooltip("強化可能の限界値を表すImage")]
    [SerializeField] private Image m_backImage;

    [Tooltip("値変えると長さ変わる")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_value = new();

    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_backValue = new();

    [SerializeField] private TextMeshProUGUI m_numTex;

    private float m_maxParameter = 200.0f;
    public float MaxParameter { get { return m_maxParameter; } set { m_maxParameter = value; } }

    // 現在のHP、最大値、最小値もセット
    public void SetParameter(float _current, float _limit ,float _max)
    {
        m_maxParameter = _max;
        SetParameterValue(_current,_limit);
    }

    // 現在のHPをセット
    public void SetParameterValue(float _currentValue,float _currentLimit)
    {

        m_value = _currentValue / m_maxParameter;
        m_backValue=_currentLimit / m_maxParameter;

        var statusNum = m_value * 100.0f;
        statusNum -= 1.0f;
        if(statusNum<=0.0f)
        {
            statusNum = 0.0f;
        }

        if (m_numTex)
        {
            m_numTex.text = statusNum.ToString("00");

            // 限界値まで来たら赤色にする
            if (m_value == m_backValue)
            {
                m_numTex.color = Color.red;
            }
            else
            {
                m_numTex.color = Color.black;
            }
        }

        // バーの更新
        UpdateFillAmount();

    }

    private void UpdateFillAmount()
    {
        if (m_image == null || m_backImage == null) return;

        // 値→長さ
        m_image.fillAmount = m_value;
        m_backImage.fillAmount = m_backValue;
    }

}

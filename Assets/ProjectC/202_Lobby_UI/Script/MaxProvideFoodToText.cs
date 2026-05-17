using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 提供可能数をTMPで表示 制作者（吉田）
/// </summary>
public class MaxProvideFoodToText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_text = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_text == null)
        {
            m_text = GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_text.text = ProvideFoodManager.instance.MaxFoodListCount.ToString();
    }
}

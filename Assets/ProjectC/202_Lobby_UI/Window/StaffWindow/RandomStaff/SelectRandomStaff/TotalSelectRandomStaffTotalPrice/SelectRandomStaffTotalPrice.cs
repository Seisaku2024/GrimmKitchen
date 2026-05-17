using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// 制作者 吉田
/// SelectKeepRandomStaffControllerから、総雇用金額を取得し、表示する
/// 
/// </summary>
public class SelectRandomStaffTotalPrice : WindowUpdateBase
{
    [SerializeField]
    private SelectKeepRandomStaffController m_selectKeepRandomStaffController = null;

    private TextMeshProUGUI m_text = null;

    // Start is called before the first frame update
    public override void OnInitialize()
    {
        if(m_selectKeepRandomStaffController == null)
        {
            Debug.LogError("m_selectKeepRandomStaffController is null");
            return;
        }

        if(!TryGetComponent(out m_text))
        {
            Debug.LogError("TextMeshProUGUIがシリアライズされていません. オブジェクト名：" + gameObject.name +
                               "　スクリプト名：" + name);
            return;
        }
    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        if(m_text == null)return;
        if(m_selectKeepRandomStaffController == null)return;

        int price = m_selectKeepRandomStaffController.TotalEmploymentPrice;
        m_text.text = price.ToString("0");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AfterStaffStatusUpUI : MonoBehaviour
{
    // 制作者 田内
    // 前後の変化を表示するUI

    [SerializeField]
    private TextMeshProUGUI m_salaryText = null;

    [SerializeField]
    private TextMeshProUGUI m_provideText = null;

    [SerializeField]
    private TextMeshProUGUI m_cookingText = null;

    [SerializeField]
    private TextMeshProUGUI m_serviceText = null;


    private StaffStatusData m_beforeStaffStatusData = null;
    private StaffStatusData m_afterStaffStatusData = null;

    //=======================================
    //              実行処理
    //=======================================

    public void SetStaffStatusData(StaffStatusData _before, StaffStatusData _after)
    {
        m_beforeStaffStatusData = _before;
        m_afterStaffStatusData = _after;
        if (m_beforeStaffStatusData == null) return;
        if (m_afterStaffStatusData == null) return;

        if (m_salaryText != null)
        {
            m_salaryText.text = "+ " + (m_afterStaffStatusData.SalaryPrice() - m_beforeStaffStatusData.SalaryPrice()).ToString();
        }

        if (m_provideText != null)
        {
            m_provideText.text = "+ " + (m_afterStaffStatusData.ProvideValue - m_beforeStaffStatusData.ProvideValue).ToString();
        }

        if (m_cookingText != null)
        {
            m_cookingText.text = "+ " + (m_afterStaffStatusData.CookingValue - m_beforeStaffStatusData.CookingValue).ToString();
        }

        if (m_serviceText != null)
        {
            m_serviceText.text = "+ " + (m_afterStaffStatusData.ServiceValue - m_beforeStaffStatusData.ServiceValue).ToString();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeStaffPointDataDescription : ChangeStaffStatusDataDescription
{
    // 制作者 田内
    // スタッフ情報に加えてポイント情報の説明文

    [Header("=================================")]
    [Header("給料合計")]
    [SerializeField]
    private TextMeshProUGUI m_totalSalaryPriceText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_totalSalaryPriceTextList = new();

    //==========================================================
    // 選択中のスタッフポイント

    private StaffPointData m_currentSelectStaffPointData = null;

    //==================================================
    //                  実行処理
    //==================================================

    public override void SetDescription()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectTutorialControllerがシリアライズされていません");
            return;
        }

        var data = m_selectUIController.CurrentSelectUI;
        if (data == null) return;

        var slotData = data.GetComponent<StaffPointSlotData>();
        if (slotData == null)
        {
            m_currentSelectStaffPointData = null;
            m_currentSelectStaffStatusData = null;
        }
        else
        {
            m_currentSelectStaffPointData = slotData.StaffPointData;
            m_currentSelectStaffStatusData = slotData.StaffStatusData;
        }

        // 説明文を更新
        SetPointDescription();

        if (m_staffStatusDataDescription == null) m_staffStatusDataDescription = gameObject.GetComponent<StaffStatusDataDescription>();
        m_staffStatusDataDescription.UpdateDescription(m_currentSelectStaffStatusData);
    }

    private void SetPointDescription()
    {

        SetStaffPointDescription();

        SetActiveGameObjectList();
    }


    private void SetActiveGameObjectList()
    {
        UIExtensions.CheckToSetActiveGameObjectList(m_totalSalaryPriceText, m_totalSalaryPriceTextList);
    }



    private void SetStaffPointDescription()
    {
        SetTotalSalaryPriceText();
    }


    // 合計給料額をセット
    private void SetTotalSalaryPriceText(bool _active = true)
    {
        if (m_totalSalaryPriceText == null) return;

        m_totalSalaryPriceText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;

        m_totalSalaryPriceText.text = StaffManager.instance.GetTotalSalaryPrice().ToString();
        m_totalSalaryPriceText.gameObject.SetActive(true);
    }



}

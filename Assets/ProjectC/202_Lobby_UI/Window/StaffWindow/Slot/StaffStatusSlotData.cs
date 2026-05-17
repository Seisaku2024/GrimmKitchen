using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(StaffStatusDataDescription))]
public class StaffStatusSlotData : MonoBehaviour
{
    // 制作者 田内

    //=====================================================================
    // 説明文

    private StaffStatusDataDescription m_staffStatusDataDescription = null;

    //=====================================================================
    // ステータスデータ
    protected StaffStatusData m_staffStatusData = null;

    public StaffStatusData StaffStatusData
    {
        get { return m_staffStatusData; }
    }

    //========================================
    //              実行処理
    //========================================


    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void SetStaffStatusData(StaffStatusData _data)
    {
        // ステータスをセット
        m_staffStatusData = _data;

        // 説明文を更新
        if (m_staffStatusDataDescription == null) m_staffStatusDataDescription = gameObject.GetComponent<StaffStatusDataDescription>();
        m_staffStatusDataDescription.UpdateDescription(m_staffStatusData);
    }

    public void InitializeSlotData()
    {
        m_staffStatusData = null;

        // 説明文を更新
        if (m_staffStatusDataDescription == null) m_staffStatusDataDescription = gameObject.GetComponent<StaffStatusDataDescription>();
        m_staffStatusDataDescription.UpdateDescription(null);
    }
}


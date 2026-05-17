using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StaffStatusUpDescription))]
public class StaffStatusUpSlotData : MonoBehaviour
{
    // 制作者 田内
    // チャレンジスロットデータ


    //=========================================================
    // 説明文

    private StaffStatusUpDescription m_staffStatusUpDescription = null;

    //============================================
    // ステータスアップデータ
    protected StaffStatusUpData m_staffStatusUpData = null;

    public StaffStatusUpData StaffStatusUpData
    {
        get { return m_staffStatusUpData; }
    }

    //========================================
    //              実行処理
    //========================================

    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void SetData(StaffStatusUpData _data)
    {
        // ステータスをセット
        m_staffStatusUpData = _data;

        // 説明文を更新
        if (m_staffStatusUpDescription == null) m_staffStatusUpDescription = gameObject.GetComponent<StaffStatusUpDescription>();
        m_staffStatusUpDescription.UpdateDescription(_data);
    }
}

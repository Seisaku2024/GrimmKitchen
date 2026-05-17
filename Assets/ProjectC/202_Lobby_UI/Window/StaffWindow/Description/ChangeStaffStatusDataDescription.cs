using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StaffStatusDataDescription))]
public class ChangeStaffStatusDataDescription : MonoBehaviour
{
    // 制作者 田内
    // スタッフのステータス説明文


    [Header("UIを選択するコントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;

    //=====================================================================
    // 説明文

    protected StaffStatusDataDescription m_staffStatusDataDescription = null;

    //=====================================================================
    // 選択中のステータスデータ

    protected StaffStatusData m_currentSelectStaffStatusData = null;


    //==============================================
    //              実行処理
    //==============================================


    virtual public void OnInitialize()
    {
        // 初期化
        SetDescription();
    }


    virtual public void OnUpdate()
    {
        // 変更が加えられれば
        if (IsChangeDescription())
        {
            // 説明文を更新
            SetDescription();
        }
    }



    // 説明文を変更できるか確認
    protected bool IsChangeDescription()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return false;
        }
        #endregion

        return m_selectUIController.IsSelectChangeFlg;
    }


    /// <summary>
    /// 選択中のItemSlotDataを基に説明文を更新
    /// </summary>
    virtual public void SetDescription()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectTutorialControllerがシリアライズされていません");
            return;
        }

        m_currentSelectStaffStatusData = null;

        var data = m_selectUIController.CurrentSelectUI;
        if (data != null)
        {
            if (data.TryGetComponent<StaffStatusSlotData>(out var staffStatusSlotData) == true)
            {
                m_currentSelectStaffStatusData = staffStatusSlotData.StaffStatusData;
            }
        }

        // 説明文を更新
        if (m_staffStatusDataDescription == null) m_staffStatusDataDescription = gameObject.GetComponent<StaffStatusDataDescription>();
        m_staffStatusDataDescription.UpdateDescription(m_currentSelectStaffStatusData);
    }
}

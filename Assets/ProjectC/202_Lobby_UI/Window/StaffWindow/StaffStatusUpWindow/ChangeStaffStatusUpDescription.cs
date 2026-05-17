using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StaffStatusUpDescription))]
public class ChangeStaffStatusUpDescription : MonoBehaviour
{
    // 制作者 田内
    // スタッフステータスアップの説明文を表示

    [Header("コントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;

    //========================================================
    // 説明文
    protected StaffStatusUpDescription m_staffStatusUpDescription = null;

    //============================================
    // 選択中のステータスアップデータ
    protected StaffStatusUpData m_staffStatusUpData = null;


    //==============================================
    //              実行処理
    //==============================================

    /// <summary>
    /// 初期化処理
    /// </summary>
    virtual public void OnInitialize()
    {
        // 初期化
        SetDescription();
    }


    /// <summary>
    /// 実行処理
    /// </summary>
    virtual public void OnUpdate()
    {
        if (IsChangeDescription())
        {
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



    virtual protected void SetDescription()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectTutorialControllerがシリアライズされていません");
            return;
        }

        m_staffStatusUpData = null;

        var data = m_selectUIController.CurrentSelectUI;
        if (data != null)
        {
            if (data.TryGetComponent<StaffStatusUpSlotData>(out var slotData))
            {
                // ターゲット更新
                m_staffStatusUpData = slotData.StaffStatusUpData;
            }
        }

        // 説明文を更新
        if (m_staffStatusUpDescription == null) m_staffStatusUpDescription = gameObject.GetComponent<StaffStatusUpDescription>();
        m_staffStatusUpDescription.UpdateDescription(m_staffStatusUpData);

    }
}

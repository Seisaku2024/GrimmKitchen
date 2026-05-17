using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeStartManagementDescription : MonoBehaviour
{
    // 制作者 田内
    // 経営開始コントローラーの情報によって表示/非表示を変更する

    [Header("経営開始コントローラー")]
    [SerializeField]
    private StartManagementController m_managementController = null;

    [Header("合計給料金額テキスト")]
    [SerializeField]
    private TextMeshProUGUI m_totalSalaryPriceText = null;

    [Header("スタッフスロット作成")]
    [SerializeField]
    private CreateStaffPointSlotList m_createStaffPointSlotList = null;

    [Header("提供料理スロット作成")]
    [SerializeField]
    private CreateProvideFoodSlotList m_createManagementProvideFoodSlotList = null;

    [Header("スタッフ")]
    [SerializeField]
    private List<CanvasGroup> m_staffUIList = new();

    [Header("スタッフ給料")]
    [SerializeField]
    private List<CanvasGroup> m_salaryPriceUIList = new();

    [Header("提供料理")]
    [SerializeField]
    private List<CanvasGroup> m_provideFoodUIList = new();

    [Header("開始")]
    [SerializeField]
    private List<CanvasGroup> m_startUIList = new();

    //============================================================
    //                     実行処理
    //============================================================


    virtual public void OnInitialize()
    {
        #region nullチェック
        if (m_createStaffPointSlotList == null)
        {
            Debug.LogError("CreateStaffPointSlotListがシリアライズされていません");
            return;
        }
        if (m_createManagementProvideFoodSlotList == null)
        {
            Debug.LogError("reateManagementProvideFoodSlotListがシリアライズされていません");
            return;
        }
        #endregion

        // 経営料理スロット
        _ = m_createManagementProvideFoodSlotList.OnInitialize();

        // スタッフスロット
        _ = m_createStaffPointSlotList.OnInitialize();

        // 合計給料金額
        SetTotalSalaryPriceText();

        // スタッフUI
        SetStaffUIList();

        // 給料UI
        SetSalaryPriceUIList();

        // 提供料理UI
        SetProvideFoodUIList();

        // 開始UI
        SetStartUIList();
    }


    // 合計給料金額
    private void SetTotalSalaryPriceText()
    {
        if (m_totalSalaryPriceText == null) return;

        m_totalSalaryPriceText.gameObject.SetActive(false);

        m_totalSalaryPriceText.text = StaffManager.instance.GetTotalSalaryPrice().ToString();

        m_totalSalaryPriceText.gameObject.SetActive(true);
    }




    /// <summary>
    /// スタッフUI表示/非表示
    /// </summary>
    private void SetStaffUIList()
    {
        #region nullチェック
        if (m_managementController == null)
        {
            Debug.LogError("ManagementController がシリアライズされていません");
            return;
        }
        #endregion

        bool flg = m_managementController.IsStaff();

        foreach (var ui in m_staffUIList)
        {
            if (ui == null) continue;
            if (flg == false) ui.alpha = 1.0f;
            else ui.alpha = 0.0f;
        }
    }


    /// <summary>
    /// 給料UI表示/非表示
    /// </summary>
    private void SetSalaryPriceUIList()
    {
        #region nullチェック
        if (m_managementController == null)
        {
            Debug.LogError("ManagementController がシリアライズされていません");
            return;
        }
        #endregion

        bool flg = m_managementController.IsSalaryPrice();

        foreach (var ui in m_salaryPriceUIList)
        {
            if (ui == null) continue;
            if (flg == false) ui.alpha = 1.0f;
            else ui.alpha = 0.0f;
        }
    }


    /// <summary>
    /// 経営料理UI表示/非表示
    /// </summary>
    private void SetProvideFoodUIList()
    {
        #region nullチェック
        if (m_managementController == null)
        {
            Debug.LogError("ManagementController がシリアライズされていません");
            return;
        }
        #endregion   

        bool flg = m_managementController.IsSettingProvideFood();

        foreach (var ui in m_provideFoodUIList)
        {
            if (ui == null) continue;
            if (flg == false) ui.alpha = 1.0f;
            else ui.alpha = 0.0f;
        }
    }



    /// <summary>
    /// 開始UI表示/非表示
    /// </summary>
    private void SetStartUIList()
    {
        #region nullチェック
        if (m_managementController == null)
        {
            Debug.LogError("ManagementController がシリアライズされていません");
            return;
        }
        #endregion

        bool flg = m_managementController.IsStart();

        foreach (var ui in m_startUIList)
        {
            if (ui == null) continue;
            if (flg == false) ui.alpha = 1.0f;
            else ui.alpha = 0.0f;
        }
    }

}

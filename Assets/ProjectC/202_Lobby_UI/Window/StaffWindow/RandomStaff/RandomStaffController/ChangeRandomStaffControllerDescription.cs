using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ChangeRandomStaffControllerDescription : ChangeValueControllerDescription
{
    // 制作者 田内
    // RandomStaffControllerによる説明文


    [Header("=======================================")]
    [Header("使用予定金額テキスト")]
    [SerializeField]
    private TextMeshProUGUI m_totalPriceText = null;

    [Header("表示/非表示")]
    [SerializeField]
    private List<GameObject> m_totalPriceTextList = new();

    //================================================
    //                  実行処理
    //================================================


    protected override void SetDescription()
    {
        base.SetDescription();

        SetTotalPriceText();
    }

    protected override void SetActiveList()
    {
        base.SetActiveList();

        UIExtensions.CheckToSetActiveGameObjectList(m_totalPriceText, m_totalPriceTextList);
    }


    // 使用予定金額テキスト
    private void SetTotalPriceText()
    {
        if (m_totalPriceText == null) return;

        m_totalPriceText.gameObject.SetActive(false);

        if (m_valueController is RandomStaffController)
        {
            var controller = m_valueController as RandomStaffController;

            m_totalPriceText.text = controller.GetTotalPrice().ToString();

            m_totalPriceText.gameObject.SetActive(true);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTotalEvaluationText : BaseChangeText
{
    // 制作者 田内
    // 現在の総額を表示する

    // 保存用
    private int m_keepValue = 0;

    //====================================
    //          実行処理
    //====================================

    override protected void ChangeText()
    {
        if (m_text == null)
        {
            Debug.LogError("テキストがシリアライズされていません");
            return;
        }

        int total = ManagementDataManager.instance.TotalEvaluation;
        if (m_keepValue != total)
        {
            m_keepValue = total;

            m_text.text = total.ToString();

            PlayEffect();
            PlayDoTween();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSatisfactionValueText : BaseChangeText
{
    // 制作者　田内
    // 満足値をテキストに表示

    // 保存用
    private int m_keepValue = 0;

    //=========================================================
    //                  実行処理
    //=========================================================

    override protected void ChangeText()
    {
        if (m_text == null)
        {
            Debug.LogError("更新するするテキストが存在しません");
            return;
        }

        int satisfactionValue = ManagementGameDataManager.instance.CurrentSatisfactionValue;
        if (m_keepValue != satisfactionValue)
        {
            // 保存しておく
            m_keepValue = satisfactionValue;

            // 現在の満足値に更新する
            m_text.text = satisfactionValue.ToString();

            PlayEffect();
            PlayDoTween();
        }
    }


}

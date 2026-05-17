using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEarnedMoneyText : BaseChangeText
{
    // 現在稼いでいる金額をテキストに表示
    // 制作者　田内

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

        int earnedMoney = ManagementGameDataManager.instance.EarnedMoney;
        if (m_keepValue != earnedMoney)
        {
            // 保存しておく
            m_keepValue = earnedMoney;

            // 現在の金額に更新する
            m_text.text = earnedMoney.ToString();

            PlayEffect();
            PlayDoTween();
        }
    }


}

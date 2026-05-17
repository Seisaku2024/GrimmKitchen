using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EarnedMoneyClearConditionChallenge : BaseClearConditionChallengeData
{
    // 制作者 田内
    // 稼ぎ条件


    [Header("稼が無いといけない金額")]
    [SerializeField]
    [Min(1)]
    private int m_earnedMoney = 10000;

    //===========================================
    //              実行処理
    //===========================================

    private void Start()
    {

        ManagementGameDataManager.instance.EarnedMoneyRP.Subscribe(_ =>
        {
            // イベントを発信
            PublishChangeClearConditionChallenge();
        }).AddTo(this);
    }


    public override bool IsClear()
    {
        if (m_earnedMoney <= ManagementGameDataManager.instance.EarnedMoney) return true;
        return false;
    }

    public override string GetConditionText()
    {
        int num = 0;
        if (ManagementGameDataManager.instance != null)
        {
            num = ManagementGameDataManager.instance.EarnedMoney;
        }

        string text = num.ToString() + " / " + m_earnedMoney.ToString();
        return text;
    }

    public override string GetConditionNumText()
    {
        string text = "×" + m_earnedMoney.ToString() + "G";
        return text;
    }

    public override float GetClearRatio()
    {
        float ratio = (float)ManagementGameDataManager.instance.EarnedMoney / (float)m_earnedMoney;

        ratio = Mathf.Clamp(value: ratio, max: 1.0f, min: 0.0f);

        return ratio;
    }

}

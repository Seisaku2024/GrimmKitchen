using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SatisfactionValueClearConditionChallenge : BaseClearConditionChallengeData
{
    // 制作者 田内
    // 満足度条件


    [Header("満足度値")]
    [SerializeField]
    [Min(1)]
    private int m_satisfactionValue = 300;

    //===========================================
    //              実行処理
    //===========================================

    private void Start()
    {

        ManagementGameDataManager.instance.CurrentSatisfactionValueRP.Subscribe(_ =>
        {
            // イベントを発信
            PublishChangeClearConditionChallenge();
        }).AddTo(this);
    }


    public override bool IsClear()
    {
        if (m_satisfactionValue <= ManagementGameDataManager.instance.CurrentSatisfactionValue) return true;
        return false;
    }

    public override string GetConditionText()
    {
        int num = 0;
        if (ManagementGameDataManager.instance != null)
        {
            num = ManagementGameDataManager.instance.CurrentSatisfactionValue;
        }

        string text = num.ToString() + " / " + m_satisfactionValue.ToString();
        return text;
    }

    public override string GetConditionNumText()
    {
        string text = "×" + m_satisfactionValue.ToString();
        return text;
    }

    public override float GetClearRatio()
    {
        float ratio = (float)ManagementGameDataManager.instance.CurrentSatisfactionValue / (float)m_satisfactionValue;

        ratio = Mathf.Clamp(value: ratio, max: 1.0f, min: 0.0f);

        return ratio;
    }
}

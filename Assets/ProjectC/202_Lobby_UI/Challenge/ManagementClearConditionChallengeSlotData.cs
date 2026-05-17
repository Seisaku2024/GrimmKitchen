using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
public class ManagementClearConditionChallengeSlotData : MonoBehaviour
{
    // 制作者　田内
    // 経営ゲーム時のクリア条件スロット

    //=======================
    // 説明文
    private ClearConditionChallengeDescription m_clearConditionChallengeDescription = null;

    //=======================
    // クリア条件データ

    private BaseClearConditionChallengeData m_clearConditionChallengeData = null;


    //=======================================
    //              実行処理
    //=======================================

    private void Start()
    {
        MessageBroker.Default.Receive<BaseClearConditionChallengeData.GlobalChangeClearConditionChallenge>().Subscribe(data =>
        {
            // 一致すれば
            if (data.ClearConditionChallengeData == m_clearConditionChallengeData)
            {
                // 説明文を更新
                if (m_clearConditionChallengeDescription == null) m_clearConditionChallengeDescription = gameObject.GetComponent<ClearConditionChallengeDescription>();
                m_clearConditionChallengeDescription.UpdateDescription(m_clearConditionChallengeData);
            }
        }).AddTo(this);
    }

    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void SetData(BaseClearConditionChallengeData _data)
    {
        // データをセット
        m_clearConditionChallengeData = _data;

        // 説明文を更新
        if (m_clearConditionChallengeDescription == null) m_clearConditionChallengeDescription = gameObject.GetComponent<ClearConditionChallengeDescription>();
        m_clearConditionChallengeDescription.UpdateDescription(m_clearConditionChallengeData);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(ClearConditionChallengeDescription))]
public class ClearConditionChallengeSlotData : MonoBehaviour
{
    // 制作者 田内
    // クリア条件スロット

    //=======================
    // 説明文
    private ClearConditionChallengeDescription m_clearConditionChallengeDescription = null;

    //=======================
    // クリア条件データ

    private BaseClearConditionChallengeData m_clearConditionChallengeData = null;

    //==========================================================
    //                       実行処理
    //==========================================================


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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UniRx;

[RequireComponent(typeof(ChallengeDescription))]
public class ChallengeSlotData : MonoBehaviour
{
    // 制作者 田内
    // チャレンジスロットデータ


    //=========================================================
    // 説明文

    private ChallengeDescription m_challengeDescription = null;

    //============================================
    // チャレンジデータ
    protected ChallengeData m_challengeData = null;

    public ChallengeData ChallengeData
    {
        get { return m_challengeData; }
    }

    //========================================
    //              実行処理
    //========================================

    private void Start()
    {
        // 選択中のチャレンジが変更されれば再度更新する
        ChallengeManager.instance.ChallengeIDRP.Subscribe(_ =>
        {
            if (m_challengeDescription == null) m_challengeDescription = gameObject.GetComponent<ChallengeDescription>();
            m_challengeDescription.UpdateDescription(m_challengeData);
        }).AddTo(this);
    }


    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void SetData(ChallengeData _data)
    {
        // ステータスをセット
        m_challengeData = _data;

        // 説明文を更新
        if (m_challengeDescription == null) m_challengeDescription = gameObject.GetComponent<ChallengeDescription>();
        m_challengeDescription.UpdateDescription(_data);
    }

}

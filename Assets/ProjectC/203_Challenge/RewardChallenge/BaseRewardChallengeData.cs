using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public abstract class BaseRewardChallengeData : MonoBehaviour
{
    // 制作者 田内
    // 報酬処理をまとめたもの


    [Header("報酬名")]
    [SerializeField]
    private LocalizeValueController m_rewardName = null;

    public LocalizeValueController RewardName
    {
        get { return m_rewardName; }
    }


    [Header("報酬画像")]
    [SerializeField]
    private Sprite m_rewardSprite = null;

    public Sprite RewardSprite
    {
        get { return m_rewardSprite; }
    }


    [Header("表示")]
    [SerializeField]
    private bool m_isDisplay = true;

    public bool IsDisplay
    {
        get { return m_isDisplay; }
    }

    //======================================================
    //                  実行処理
    //======================================================

    /// <summary>
    /// 報酬時に実行される処理
    /// </summary>
    abstract public void UpdateRewardChallenge();


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardChallengeDescription : MonoBehaviour
{
    // 制作者 田内
    // 報酬説明文
  

    [Header("=================================")]
    [Header("報酬名")]
    [SerializeField]
    protected TextMeshProUGUI m_rewardNameText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_rewardNameTextShowHideUIData = new();

    [Header("=================================")]
    [Header("報酬画像")]
    [SerializeField]
    protected Image m_rewardImage = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_rewardImageShowHideUIData = new();

    //=======================
    // チャレンジデータ

    private BaseRewardChallengeData m_rewardChallengeData= null;

    //==========================================================
    //                       実行処理
    //==========================================================

    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void UpdateDescription(BaseRewardChallengeData _data)
    {
        // ステータスをセット
       m_rewardChallengeData= _data;

        SetSlotData();
    }


    virtual protected void SetSlotData()
    {
        SetRewardNameText();
        SetRewardImage();
    }



    virtual protected void SetActiveGameObjectList()
    {
        if (m_rewardNameTextShowHideUIData != null) m_rewardNameTextShowHideUIData.SetActiveGameObjectList(m_rewardNameText);

        if (m_rewardImageShowHideUIData != null) m_rewardImageShowHideUIData.SetActiveGameObjectList(m_rewardImage);
    }



    // 報酬名
    private void SetRewardNameText(bool _active = true)
    {
        if (m_rewardNameText == null) return;

        m_rewardNameText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_rewardChallengeData == null) return;

        m_rewardNameText.text = m_rewardChallengeData.RewardName.GetLocalizedString();

        // アクティブ状態に変更
        m_rewardNameText.gameObject.SetActive(true);
    }

    // 報酬画像
    private void SetRewardImage(bool _active = true)
    {
        if (m_rewardImage == null) return;

        m_rewardImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_rewardChallengeData == null) return;

        m_rewardImage.sprite = m_rewardChallengeData.RewardSprite;

        // アクティブ状態に変更
        m_rewardImage.gameObject.SetActive(true);
    }
}

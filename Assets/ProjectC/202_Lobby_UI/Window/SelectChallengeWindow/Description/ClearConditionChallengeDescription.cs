using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClearConditionChallengeDescription : MonoBehaviour
{
    // 制作者 田内
    // クリア条件説明文


    [Header("=================================")]
    [Header("条件名")]
    [SerializeField]
    protected TextMeshProUGUI m_conditionNameText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_conditionNameTextShowHideUIData = new();

    [Header("=================================")]
    [Header("条件画像")]
    [SerializeField]
    protected Image m_conditionImage = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_conditionImageShowHideUIData = new();

    [Header("=================================")]
    [Header("クリア条件テキスト")]
    [SerializeField]
    protected TextMeshProUGUI m_conditionText = null;

    [Header("表示方法")]
    [SerializeField]
    protected bool m_isConditionNum = false;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_conditionTextShowHideUIData = new();

    [Header("=================================")]
    [Header("クリア表示画像")]
    [SerializeField]
    protected GameObject m_clearGameObjectList = null;

    //=======================
    // 条件データ

    private BaseClearConditionChallengeData m_clearConditionChallengeData = null;

    //==========================================================
    //                       実行処理
    //==========================================================

    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void UpdateDescription(BaseClearConditionChallengeData _data)
    {
        // ステータスをセット
        m_clearConditionChallengeData = _data;

        SetDescription();
    }


    virtual protected void SetDescription()
    {
        SetClearConditionNameText();
        SetClearConditionImage();
        SetClearConditionText();
        SetClearGameObject();

        SetActiveGameObjectList();
    }



    virtual protected void SetActiveGameObjectList()
    {
        if (m_conditionNameTextShowHideUIData != null) m_conditionNameTextShowHideUIData.SetActiveGameObjectList(m_conditionNameText);

        if (m_conditionImageShowHideUIData != null) m_conditionImageShowHideUIData.SetActiveGameObjectList(m_conditionImage);

        if (m_conditionTextShowHideUIData != null) m_conditionTextShowHideUIData.SetActiveGameObjectList(m_conditionText);
    }



    // 報酬名
    private void SetClearConditionNameText(bool _active = true)
    {
        if (m_conditionNameText == null) return;

        m_conditionNameText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_clearConditionChallengeData == null) return;

        m_conditionNameText.text = m_clearConditionChallengeData.ConditionName.GetLocalizedString();

        // アクティブ状態に変更
        m_conditionNameText.gameObject.SetActive(true);
    }

    // 報酬画像
    private void SetClearConditionImage(bool _active = true)
    {
        if (m_conditionImage == null) return;

        m_conditionImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_clearConditionChallengeData == null) return;

        m_conditionImage.sprite = m_clearConditionChallengeData.ConditionSprite;

        // アクティブ状態に変更
        m_conditionImage.gameObject.SetActive(true);
    }

    // 条件テキスト
    private void SetClearConditionText(bool _active = true)
    {
        if (m_conditionText == null) return;

        m_conditionText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_clearConditionChallengeData == null) return;

        if (!m_isConditionNum)
        {
            m_conditionText.text = m_clearConditionChallengeData.GetConditionText();
        }
        else
        {
            m_conditionText.text = m_clearConditionChallengeData.GetConditionNumText();
        }

        // アクティブ状態に変更
        m_conditionText.gameObject.SetActive(true);
    }

    // クリアオブジェクト
    private void SetClearGameObject(bool _active = true)
    {
        if (m_clearGameObjectList == null) return;

        m_clearGameObjectList.SetActive(false);

        if (_active == false) return;
        if (m_clearConditionChallengeData == null || m_clearConditionChallengeData.IsClear() == false) return;

        m_clearGameObjectList.SetActive(true);
    }
}

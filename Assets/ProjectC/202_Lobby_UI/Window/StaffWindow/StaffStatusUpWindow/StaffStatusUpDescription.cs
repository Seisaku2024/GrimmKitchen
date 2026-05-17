using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StaffStatusUpDescription : MonoBehaviour
{
    // スタッフステータスの説明文表示


    [Header("=================================")]
    [Header("画像")]
    [SerializeField]
    private Image m_image = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_imageShowHideUIData = new();

    [Header("=================================")]
    [Header("名")]
    [SerializeField]
    private TextMeshProUGUI m_nameText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_nameTextShowHideUIData = new();

    [Header("=================================")]
    [Header("説明文")]
    [SerializeField]
    private TextMeshProUGUI m_descriptionText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_descriptionTextShowHideUIData = new();


    // 選択中のステータスデータ
    protected StaffStatusUpData m_currentSelectStaffStatusUpData = null;

    //==============================================
    //              実行処理
    //==============================================


    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void UpdateDescription(StaffStatusUpData _data)
    {
        // ステータスをセット
        m_currentSelectStaffStatusUpData = _data;

        // 説明文を更新
        SetDescription();

        SetActiveGameObjectList();
    }


    // 説明文のテキストをセットする
    virtual protected void SetDescription()
    {
        // 名をセット
        SetNameText();

        // 説明文
        SetDescriptionText();

        // 画像をセット
        SetImage();
    }


    virtual protected void SetActiveGameObjectList()
    {
        if (m_imageShowHideUIData != null) m_imageShowHideUIData.SetActiveGameObjectList(m_image);
        if (m_nameTextShowHideUIData != null) m_nameTextShowHideUIData.SetActiveGameObjectList(m_nameText);
        if (m_descriptionTextShowHideUIData != null) m_descriptionTextShowHideUIData.SetActiveGameObjectList(m_descriptionText);
    }


    // 名
    private void SetNameText(bool _active = true)
    {
        if (m_nameText == null) return;

        m_nameText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusUpData == null) return;


        // スタッフ名をセット
        m_nameText.text = m_currentSelectStaffStatusUpData.StatusUpName.GetLocalizedString();
        m_nameText.gameObject.SetActive(true);
    }

    // 説明文
    private void SetDescriptionText(bool _active = true)
    {
        if (m_descriptionText == null) return;

        m_descriptionText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusUpData == null) return;


        // スタッフ名をセット
        m_descriptionText.text = m_currentSelectStaffStatusUpData.StatusUpDescription.GetLocalizedString();
        m_descriptionText.gameObject.SetActive(true);
    }


    // 画像
    private void SetImage(bool _active = true)
    {
        if (m_image == null) return;

        m_image.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_currentSelectStaffStatusUpData == null) return;


        // スタッフ名をセット
        m_image.sprite = m_currentSelectStaffStatusUpData.StatusUpSprite;
        m_image.gameObject.SetActive(true);
    }


}

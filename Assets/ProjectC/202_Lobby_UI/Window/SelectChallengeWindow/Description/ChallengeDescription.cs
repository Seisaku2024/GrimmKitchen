using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IngredientInfo;
using UnityEngine.UI;
using TMPro;
using ChallengeInfo;

public class ChallengeDescription : MonoBehaviour
{
    // 制作者 田内
    // チャレンジの説明文

    [Header("=================================")]
    [Header("チャレンジ名")]
    [SerializeField]
    protected TextMeshProUGUI m_nameText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_nameTextShowHideUIData = new();

    [Header("=================================")]
    [Header("チャレンジ説明文")]
    [SerializeField]
    protected TextMeshProUGUI m_descriptionText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_descriptionTextShowHideUIData = new();

    [Header("=================================")]
    [Header("チャレンジ画像")]
    [SerializeField]
    protected Image m_image = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_imageShowHideUIData = new();

    [Header("=================================")]
    [Header("クリア画像")]
    [SerializeField]
    protected Image m_clearImage = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_clearImageShowHideUIData = new();

    [Header("=================================")]
    [Header("クリア回数")]
    [SerializeField]
    protected TextMeshProUGUI m_clearNumText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_clearNumTextShowHideUIData = new();

    [Header("=================================")]
    [Header("プレイタイプ名")]
    [SerializeField]
    protected TextMeshProUGUI m_playTypeNameText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_playTypeNameTextShowHideUIData = new();

    [Header("=================================")]
    [Header("タイプ")]
    [SerializeField]
    protected Image m_typeImage = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_typeImageShowHideUIData = new();

    [Header("=================================")]
    [Header("条件スロット")]
    [SerializeField]

    CreateClearConditionChallengeSlotList m_createClearConditionChallengeSlotList = null;

    [Header("=================================")]
    [Header("報酬スロット")]
    [SerializeField]

    CreateRewardChallengeSlotList m_createRewardChallengeSlotList = null;

    [Header("=================================")]
    [Header("選択画像")]
    [SerializeField]
    protected Image m_settingImage = null;

    [Header("表示/非表示用")]
    [SerializeField]
    protected ShowHideUIData m_settingImageShowHideUIData = new();

    [Header("=================================")]
    [Header("高値食材種類スロット作成")]
    [SerializeField]
    protected CreateIngredientTypeSlotList m_createIngredientTypeSlotList = null;

    [Header("=================================")]
    [Header("メインチャレンジ時 表示オブジェクト")]
    [SerializeField]
    protected GameObject m_showMainObject = null;

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


    /// <summary>
    /// データをセット/更新する
    /// </summary>
    virtual public void UpdateDescription(ChallengeData _data)
    {
        // ステータスをセット
        m_challengeData = _data;

        SetDescription();

        SetActiveGameObjectList();
    }


    virtual protected void SetDescription()
    {
        SetName();
        SetDescriptionText();
        SetImage();
        SetSettingImage();
        SetClearImage();
        SetTypeImage();
        SetPlayTypeNameText();
        CreateRewardChallengeSlot();
        CreateIngredientTypeSlotList();
        SetClearNumText();
        CreateClearConditionChallengeSlot();
        SetActiveMainObject();
    }



    virtual protected void SetActiveGameObjectList()
    {
        if (m_nameTextShowHideUIData != null) m_nameTextShowHideUIData.SetActiveGameObjectList(m_nameText);

        if (m_descriptionTextShowHideUIData != null) m_descriptionTextShowHideUIData.SetActiveGameObjectList(m_descriptionText);

        if (m_imageShowHideUIData != null) m_imageShowHideUIData.SetActiveGameObjectList(m_image);

        if (m_settingImageShowHideUIData != null) m_settingImageShowHideUIData.SetActiveGameObjectList(m_settingImage);

        if (m_clearNumTextShowHideUIData != null) m_clearNumTextShowHideUIData.SetActiveGameObjectList(m_clearNumText);

        if (m_playTypeNameTextShowHideUIData != null) m_playTypeNameTextShowHideUIData.SetActiveGameObjectList(m_playTypeNameText);

        if (m_typeImageShowHideUIData != null) m_typeImageShowHideUIData.SetActiveGameObjectList(m_typeImage);

        if (m_clearImageShowHideUIData != null) m_clearImageShowHideUIData.SetActiveGameObjectList(m_clearImage);
    }


    // チャレンジ名
    private void SetName(bool _active = true)
    {
        if (m_nameText == null) return;

        m_nameText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_challengeData == null) return;

        // チャレンジ名をセット
        m_nameText.text = m_challengeData.ChallengeName.GetLocalizedString();
        m_nameText.gameObject.SetActive(true);
    }


    // チャレンジ説明文
    private void SetDescriptionText(bool _active = true)
    {
        if (m_descriptionText == null) return;

        m_descriptionText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_challengeData == null) return;

        // チャレンジ説明文をセット
        m_descriptionText.text = m_challengeData.ChallengeDescription.GetLocalizedString();
        m_descriptionText.gameObject.SetActive(true);
    }

    // チャレンジ画像
    private void SetImage(bool _active = true)
    {
        if (m_image == null) return;

        m_image.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_challengeData == null) return;
        if(m_challengeData.ChallengeSprite == null) return;

        // チャレンジ画像をセット
        m_image.sprite = m_challengeData.ChallengeSprite;
        m_image.gameObject.SetActive(true);
    }

    // クリア画像
    private void SetClearImage(bool _active = true)
    {
        if (m_clearImage == null) return;

        m_clearImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_challengeData == null || m_challengeData.ClearChallengeData == null) return;


        // 透明度をセット
        if (m_clearImageShowHideUIData != null)
        {
            if (m_challengeData.ClearChallengeData.IsClear)
            {
                m_clearImageShowHideUIData.SetCanvasGroupAlpha(true);
            }
            else
            {
                m_clearImageShowHideUIData.SetCanvasGroupAlpha(false);
            }
        }


        // アクティブ状態に変更
        m_clearImage.gameObject.SetActive(true);
    }

    // クリア回数
    private void SetClearNumText(bool _active = true)
    {
        if (m_clearNumText == null) return;

        m_clearNumText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_challengeData == null || m_challengeData.ClearChallengeData == null) return;

        m_clearNumText.text = m_challengeData.ClearChallengeData.ClearNum.ToString();

        // アクティブ状態に変更
        m_clearNumText.gameObject.SetActive(true);
    }

    // 現在セットされているチャレンジか
    private void SetSettingImage(bool _active = true)
    {
        if (m_settingImage == null) return;

        m_settingImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_challengeData == null) return;

        // 一致していなければ
        if (ChallengeManager.instance.ChallengeID != m_challengeData.ChallengeID) return;

        // チャレンジ画像をセット
        m_settingImage.gameObject.SetActive(true);
    }

    // 種類
    private void SetTypeImage(bool _active = true)
    {
        if (m_typeImage == null) return;

        m_typeImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_challengeData == null) return;

        ChallengeType type = m_challengeData.GetChallengeType();
        var data = ChallengeDataBaseManager.instance.GetTypeData(type);
        if (data == null) return;

        // 色をセット
        m_typeImage.color = data.ChallengeTypeColor;

        // 種類画像をセット
        m_typeImage.gameObject.SetActive(true);
    }

    // 条件スロット
    private void CreateClearConditionChallengeSlot()
    {
        if (m_createClearConditionChallengeSlotList == null) return;

        m_createClearConditionChallengeSlotList.SetData(m_challengeData);
        _ = m_createClearConditionChallengeSlotList.CreateSlot();
    }

    // 報酬スロット
    private void CreateRewardChallengeSlot()
    {
        if (m_createRewardChallengeSlotList == null) return;

        m_createRewardChallengeSlotList.SetData(m_challengeData);
        _ = m_createRewardChallengeSlotList.CreateSlot();
    }


    // プレイタイプ画像
    private void SetPlayTypeNameText(bool _active = true)
    {
        if (m_playTypeNameText == null) return;

        m_playTypeNameText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_challengeData == null) return;

        var data = ChallengeDataBaseManager.instance.GetPlayTypeData(m_challengeData.PlayType);
        if (data == null) return;

        m_playTypeNameText.text = data.PlayTypeName.GetLocalizedString();

        // 表示
        m_playTypeNameText.gameObject.SetActive(true);
    }

    // 高額料理スロット
    private void CreateIngredientTypeSlotList()
    {
        if (m_createIngredientTypeSlotList == null) return;
        if (m_challengeData == null) return;

        IngredientTypeID id = m_challengeData.BusinessConditionsIngredientTypeID;

        m_createIngredientTypeSlotList.SetData(id);
        _ = m_createIngredientTypeSlotList.CreateSlot();
    }

    // メインチャレンジ時 表示オブジェクト
    private void SetActiveMainObject(bool _active = true)
    {
        if (m_showMainObject == null) return;

        m_showMainObject.SetActive(false);

        if (_active == false) return;
        if (m_challengeData == null) return;

        ChallengeType challengeType = m_challengeData.GetChallengeType();

        if (challengeType == ChallengeType.Main)
        {
            m_showMainObject.SetActive(true);
        }
        else
        { }

    }
}

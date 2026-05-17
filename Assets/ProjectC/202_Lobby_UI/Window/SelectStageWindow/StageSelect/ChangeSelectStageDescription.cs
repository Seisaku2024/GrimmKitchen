using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StageInfo;

public class ChangeSelectStageDescription : WindowUpdateBase
{
    // ステージの情報を更新する
    // 制作者　田内
    // 修正 吉田

    //[Header("ステージ選択コントローラー")]
    //[SerializeField]
    //protected SelectStageController m_selectStageController = null;

    private StageData m_currentStage = null;
    public void SetStageData(StageID _stageID)
    {
        m_currentStage = StageDataBaseManager.instance.GetStageData(_stageID);
    }
    public void SetStageData(StageData _data)
    {
        m_currentStage = _data;
    }

    [Header("ステージImage")]
    [SerializeField]
    protected Image m_stageImage = null;

    [Header("ステージ名前")]
    [SerializeField]
    protected TextMeshProUGUI m_stageNameTextMeshProUGUI = null;

    [Header("ステージ紹介文")]
    [SerializeField]
    protected TextMeshProUGUI m_descriptionTextMeshProUGUI = null;

    //========================================================
    //                      実行処理
    //========================================================

    override public void OnInitialize()
    {
        if (m_currentStage == null)
        {
            StageEnemyDifficultyLevel stageEnemyDifficultyLevel = BaseManager<StageEnemyDifficultyLevel>.instance;
            if (stageEnemyDifficultyLevel)
            {
                m_currentStage = stageEnemyDifficultyLevel.StageData;
            }
            else return;
        }

        // 初期化
        ChangeDescription();
    }



    override public void OnUpdate()
    {
        //ChangeDescription();
    }



    private void ChangeDescription()
    {
        if (m_currentStage == null)
        {
            Debug.LogError("ステージデータが存在しません");
            SetDescription(null);
            return;
        }

        SetDescription(m_currentStage);
    }




    private void SetDescription(StageData _data)
    {
        if (_data == null)
        {
            Debug.LogError("引数データが存在しません");
            return;
        }

        // ステージの画像をセット
        SetImage(_data);

        // ステージ名テキストをセット
        SetStageNameText(_data);

        // ステージ説明テキストをセット
        SetDescriptionText(_data);

    }

    private void SetStageNameText(StageData _data)
    {
        if (m_stageNameTextMeshProUGUI == null)
        {
            Debug.Log("ステージ名を表示するTextMeshProUGUIがシリアライズされていません");
            return;
        }

        m_stageNameTextMeshProUGUI.gameObject.SetActive(false);

        if (_data == null)
        {
            Debug.LogError("引数データが存在しません");
            return;
        }

        m_stageNameTextMeshProUGUI.text = _data.StageNameText.GetLocalizedString();

        m_stageNameTextMeshProUGUI.gameObject.SetActive(true);
    }


    private void SetImage(StageData _data)
    {
        if (m_stageImage == null)
        {
            Debug.Log("ステージ画像を表示するImageがシリアライズされていません");
            return;
        }

        m_stageImage.gameObject.SetActive(false);

        if (_data == null)
        {
            Debug.LogError("引数データが存在しません");
            return;
        }

        m_stageImage.sprite = _data.StageSprite;

        m_stageImage.gameObject.SetActive(true);
    }

    private void SetDescriptionText(StageData _data)
    {
        if (m_descriptionTextMeshProUGUI == null)
        {
            Debug.Log("説明文を表示するTextMeshProUGUIがシリアライズされていません");
            return;
        }

        m_descriptionTextMeshProUGUI.gameObject.SetActive(false);

        if (_data == null)
        {
            Debug.LogError("引数データが存在しません");
            return;
        }

        m_descriptionTextMeshProUGUI.text = _data.StageDescriptionText.GetLocalizedString();

        m_descriptionTextMeshProUGUI.gameObject.SetActive(true);
    }

}

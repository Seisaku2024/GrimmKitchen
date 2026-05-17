using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ChangeSelectTutorialControllerDescription : MonoBehaviour
{
    // 制作者 田内
    // チュートリアルの説明文を更新する

    [Header("コントローラー")]
    [SerializeField]
    private SelectTutorialController m_selectTutorialController = null;

    [Header("画像")]
    [SerializeField]
    private Image m_image = null;

    [Header("説明文")]
    [SerializeField]
    private TextMeshProUGUI m_text = null;

    [Header("ページ数")]
    [SerializeField]
    private TextMeshProUGUI m_pageNumText = null;

    //InputAction関係（山本）--------------------------------------------
    [Header("InputActionButton")]
    [SerializeField]
    private InputActionButton m_inputActionButton = null;

    [Header("InputActionButtonのテキスト")]
    [SerializeField]
    private TextMeshProUGUI m_inputButtonText = null;

    //------------------------------------------------------------------

    //==============================================
    //              実行処理
    //==============================================

    virtual public void OnInitialize()
    {
        // 初期化
        SetDescription();
    }

    private void Start()
    {
        // 初期化
        SetDescription();
    }


    virtual public void OnUpdate()
    {
        if (IsChangeDescription())
        {
            SetDescription();
        }
    }



    private void SetDescription()
    {
        if (m_selectTutorialController == null)
        {
            Debug.LogError("SelectTutorialControllerがシリアライズされていません");
            return;
        }

        var data = m_selectTutorialController.GetCurrentTutorialData();
        if (data == null) return;

        // 画像をセット
        SetImage(data);

        // テキストをセット
        SetText(data);

        // ページ数
        SetPageText();

        // InputActionButton関係
        SetInputAction(data);

    }




    private void SetImage(TutorialData _data)
    {
        if (m_image == null)
        {
            // シリアライズされていません
            return;
        }

        m_image.gameObject.SetActive(false);

        if (_data?.Sprite == null) return;

        m_image.gameObject.SetActive(true);

        m_image.sprite = _data.Sprite;

    }


    private void SetText(TutorialData _data)
    {
        if (m_text == null)
        {
            // シリアライズされていません
            return;
        }

        m_text.gameObject.SetActive(false);

        if (_data == null) return;

        m_text.gameObject.SetActive(true);

        m_text.text = _data.Text;

    }


    private void SetPageText()
    {
        #region nullチェック
        if (m_selectTutorialController == null)
        {
            return;
        }
        if (m_pageNumText == null)
        {
            // シリアライズされていません
            return;
        }
        #endregion

        m_pageNumText.gameObject.SetActive(false);
        m_pageNumText.gameObject.SetActive(true);

        int currentPage = m_selectTutorialController.CurrentTutorial + 1;
        int maxPage = m_selectTutorialController.TutorialDataList.Count;

        m_pageNumText.text = currentPage.ToString() + "/" + maxPage.ToString();

    }

    //InputAction関係(山本)
    private void SetInputAction(TutorialData _data)
    {
        #region nullチェック
        if (m_selectTutorialController == null)
        {
            return;
        }
        if (m_inputActionButton == null)
        {
            // シリアライズされていません
            return;
        }
        if(m_inputButtonText==null)
        {
            return;
        }
        #endregion

        m_inputActionButton.gameObject.SetActive(false);
        if (_data.InputActionReference == null) return;
        m_inputActionButton.InputActionReference = _data.InputActionReference;
        m_inputActionButton.UpdateButtonImage();
        m_inputActionButton.gameObject.SetActive(true);


        m_inputButtonText.gameObject.SetActive(false);
        if (_data.InputActionText == null) return;
        m_inputButtonText.text = _data.InputActionText;
        m_inputButtonText.gameObject.SetActive(true);

    }

    // 説明文を変更できるか確認
    public bool IsChangeDescription()
    {
        // コントローラーがなければ
        if (m_selectTutorialController == null)
        {
            Debug.LogError("コントローラーが登録されていません");
            return false;
        }

        if (m_selectTutorialController.IsSelectChangeFlg)
        {
            // 説明文を変更する
            return true;
        }

        return false;
    }



}

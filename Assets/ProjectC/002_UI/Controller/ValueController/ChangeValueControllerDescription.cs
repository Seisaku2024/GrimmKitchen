using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeValueControllerDescription : MonoBehaviour
{
    // 制作者 田内
    // 料理作成コントローラーの説明文

    [Header("コントローラー")]
    [SerializeField]
    protected ValueController m_valueController = null;


    [Header("=======================================")]
    [Header("現在値")]
    [SerializeField]
    private TextMeshProUGUI m_currentCreateNumText = null;

    [Header("表示/非表示")]
    [SerializeField]
    private List<GameObject> m_currentCreateNumTextList = new();


    [Header("=======================================")]
    [Header("最大値")]
    [SerializeField]
    private TextMeshProUGUI m_maxCreateNumText = null;

    [Header("表示/非表示")]
    [SerializeField]
    private List<GameObject> m_maxCreateNumTextList = new();

    [Header("=======================================")]
    [Header("最小値")]
    [SerializeField]
    private TextMeshProUGUI m_minCreateNumText = null;

    [Header("表示/非表示")]
    [SerializeField]
    private List<GameObject> m_minCreateNumTextList = new();

    //==============================================
    //              実行処理
    //==============================================

    virtual public void OnInitialize()
    {
        // 初期化
        SetDescription();
        SetActiveList();
    }



    virtual public void OnUpdate()
    {
        if (IsChangeDescription())
        {
            SetDescription();
            SetActiveList();
        }
    }


    /// <summary>
    /// 説明文を更新
    /// </summary>
    virtual protected void SetDescription()
    {
        SetCurrentCreateNumText();

        SetMaxCreateNumText();

        SetMinCreateNumText();
    }

    /// <summary>
    /// アクティブリストを更新
    /// </summary>
    virtual protected void SetActiveList()
    {
        UIExtensions.CheckToSetActiveGameObjectList(m_currentCreateNumText, m_currentCreateNumTextList);

        UIExtensions.CheckToSetActiveGameObjectList(m_maxCreateNumText, m_maxCreateNumTextList);

        UIExtensions.CheckToSetActiveGameObjectList(m_minCreateNumText,m_minCreateNumTextList);
    }


    private void SetCurrentCreateNumText()
    {
        #region nullチェック
        if (m_valueController == null)
        {
            return;
        }
        if (m_currentCreateNumText == null)
        {
            // シリアライズされていません
            return;
        }
        #endregion

        m_currentCreateNumText.gameObject.SetActive(false);

        m_currentCreateNumText.text = m_valueController.CurrentValue.ToString();

        m_currentCreateNumText.gameObject.SetActive(true);
    }


    private void SetMaxCreateNumText()
    {
        #region nullチェック
        if (m_valueController == null)
        {
            return;
        }
        if (m_maxCreateNumText == null)
        {
            // シリアライズされていません
            return;
        }
        #endregion

        m_maxCreateNumText.gameObject.SetActive(false);

        m_maxCreateNumText.text = m_valueController.MaxValue.ToString();

        m_maxCreateNumText.gameObject.SetActive(true);
    }


    private void SetMinCreateNumText()
    {
        #region nullチェック
        if (m_valueController == null)
        {
            return;
        }
        if (m_minCreateNumText == null)
        {
            // シリアライズされていません
            return;
        }
        #endregion

        m_minCreateNumText.gameObject.SetActive(false);

        m_minCreateNumText.text = m_valueController.MinValue.ToString();

        m_minCreateNumText.gameObject.SetActive(true);

    }

    // 説明文を変更できるか確認
    public bool IsChangeDescription()
    {
        // コントローラーがなければ
        if (m_valueController == null)
        {
            Debug.LogError("コントローラーが登録されていません");
            return false;
        }

        if (m_valueController.IsSelectChangeFlg)
        {
            // 説明文を変更するs
            return true;
        }

        return false;
    }



}

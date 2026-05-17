using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeResultTextUI : MonoBehaviour
{
    // 制作者 田内
    // チャレンジ結果をテキストで表示する

    [Header("テキストオブジェクト")]
    [SerializeField]
    TMPro.TextMeshProUGUI m_text = null;

    [Header("成功時テキスト")]
    [SerializeField]
    private LocalizeStringData m_successText = new();

    [Header("失敗時テキスト")]
    [SerializeField]
    private LocalizeStringData m_failureText = new();

    //============================================
    //              実行処理
    //============================================

    private void Start()
    {
        SetText();
    }


    private void SetText()
    {
        if (m_text == null)
        {
            Debug.LogError("Textがシリアライズされていません");
            return;
        }

        // クリアしていれば
        if (ManagementGameDataManager.instance.IsClearChallenge())
        {
            m_text.text = m_successText.LocalizedString().GetLocalizedString();
        }
        // していなければ
        else
        {
            m_text.text = m_failureText.LocalizedString().GetLocalizedString();
        }
    }

}

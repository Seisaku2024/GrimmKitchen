using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelectUseStaffInfo;
using TMPro;

public class SelectUseStaffButton : MonoBehaviour
{
    // 制作者 田内
    // IDを保持するボタン

    [Header("キャンバスグループ")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;


    [Header("ボタンテキスト")]
    [SerializeField]
    private TextMeshProUGUI m_buttonText = null;


    //===================
    // 選択ID

    private SelectUseStaffID m_id = SelectUseStaffID.None;

    public SelectUseStaffID ID { get { return m_id; } }

    //====================
    // 押せるかどうか


    private bool m_isCanPress = true;

    public bool IsCanPress { get { return m_isCanPress; } }


    //========================================
    //              実行処理
    //========================================

    public void SetData(SelectUseStaffID _id, bool _active)
    {
        if (m_buttonText == null)
        {
            Debug.LogError("TextMeshProが存在しません");
            return;
        }

        // IDを更新
        m_id = _id;

        // 押せるかどうかを更新
        m_isCanPress = _active;

        SetName();
        SetCanvasGroup();
    }


    private void SetName()
    {
        if (m_buttonText == null) return;

        var data = UseStaffDataBaseManager.instance.GetData(m_id);
        if (data == null) return;

        m_buttonText.text = data.UseStaffName.GetLocalizedString();
    }


    private void SetCanvasGroup()
    {
        if (m_canvasGroup == null) return;

        if (m_isCanPress)
        {
            m_canvasGroup.alpha = 1.0f;
        }
        else
        {
            m_canvasGroup.alpha = 0.5f;
        }
    }
}

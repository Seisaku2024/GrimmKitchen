using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// 制作者　吉田
/// Enumでの設定オブジェクトを制御するクラス
/// 表示する名前を切り替える
/// </summary>
public class EnumElementController : MonoBehaviour
{
    [SerializeField]
    private int m_enumIndex = 0;
    public int EnumIndex
    {
        get { return m_enumIndex; }
    }

    private List<Action> m_onChangeIndexEventList = new();
    public void AddOnChangeIndexEvent(Action _event)
    {
        m_onChangeIndexEventList.Add(_event);
    }

    [Header("表示する文字列のリスト　Enumの順と合わせる")]
    [SerializeField]
    private List<LocalizedString> m_enumList = new List<LocalizedString>();

    [SerializeField]
    private TextMeshProUGUI m_text = null;

    [ContextMenu("ChangeText")]
    // テキストを m_enumList[m_enumIndex] に変更
    private void ChangeText()
    {
        if (m_text == null) return;

        if(m_enumList.Count > m_enumIndex)
        {
            m_text.text = m_enumList[m_enumIndex].GetLocalizedString();
        }
    }

    public void Increment()
    {
        m_enumIndex++;
        if (m_enumIndex >= m_enumList.Count)
        {
            m_enumIndex = 0;
        }

        ChangeIndex();
        ChangeText();
    }

    public void Decrement()
    {
        m_enumIndex--;
        if (m_enumIndex < 0)
        {
            m_enumIndex = m_enumList.Count - 1;
        }

        ChangeIndex();
        ChangeText();
    }

    public void SetEnumIndex(int index)
    {
        m_enumIndex = index;
        ChangeIndex();
        ChangeText();
    }

    private void ChangeIndex()
    {
        foreach (var _event in m_onChangeIndexEventList)
        {
            _event.Invoke();
        }
    }

}

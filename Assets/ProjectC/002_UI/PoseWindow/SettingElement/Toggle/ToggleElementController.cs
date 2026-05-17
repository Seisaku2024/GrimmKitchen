using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 制作者　吉田
/// トグルのオブジェクトを制御するクラス
/// 表示/非表示を切り替える
/// </summary>
public class ToggleElementController : MonoBehaviour
{

    [Header("トグル値")]
    [SerializeField]
    private bool m_value = false;
    public bool Value
    {
        get { return m_value; }
    }

    [Space(10)]
    [Header("Trueの場合の表示オブジェクト")]
    [SerializeField]
    private GameObject m_trueViewObject = null;

    [Header("Falseの場合の表示オブジェクト")]
    [SerializeField]
    private GameObject m_falseViewObject = null;


    public void Switch()
    {
        if(m_value)
        {
            OnFalse();
        }
        else
        {
            OnTrue();
        }
    }

    /// <summary>
    /// 現在のm_valueの値によって表示を変更する
    /// </summary>
    [ContextMenu("SetView")]
    public void SetValue()
    {
        SetValue(m_value);
    }

    public void SetValue(bool value)
    {
        if(value)
        {
            OnTrue();
        }
        else
        {
            OnFalse();
        }
    }

    private void OnTrue()
    {
        m_value = true;

        if(m_trueViewObject != null)
        {
            m_trueViewObject.SetActive(true);
        }
        if(m_falseViewObject != null)
        {
            m_falseViewObject.SetActive(false);
        }
    }

    private void OnFalse()
    {
        m_value = false;

        if(m_trueViewObject != null)
        {
            m_trueViewObject.SetActive(false);
        }
        if(m_falseViewObject != null)
        {
            m_falseViewObject.SetActive(true);
        }
    }

}

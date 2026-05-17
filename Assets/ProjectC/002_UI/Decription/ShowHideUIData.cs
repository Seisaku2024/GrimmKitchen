using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShowHideUIData
{
    // 制作者 田内
    // 説明文に付随した処理

    [Header("透明度")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_maxAlpha = 1.0f;

    [SerializeField]
    [Range(0.0f,1.0f)]
    private float m_minAlpha = 0.5f;

    [Header("隠すオブジェクト")]
    [SerializeField]
    private List<GameObject> m_gameObjectList = new();

    /// <summary>
    /// 透明度をセット
    /// </summary>
    public void SetCanvasGroupAlpha(bool _isAlpha)
    {
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }

        if (_isAlpha) m_canvasGroup.alpha = m_maxAlpha;
        else m_canvasGroup.alpha = m_minAlpha;
    }

    /// <summary>
    /// 引数オブジェクトのアクティブを基に
    /// ゲームオブジェクトリストのアクティブをセット
    /// </summary>
    public void SetActiveGameObjectList<T>(T _component) where T : Behaviour
    {
        UIExtensions.CheckToSetActiveGameObjectList(_component, m_gameObjectList);
    }
}


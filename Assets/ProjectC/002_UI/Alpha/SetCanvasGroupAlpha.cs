using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCanvasGroupAlpha : MonoBehaviour
{
    // 制作者 田内
    // キャンバスグループの透明度をセットする

    [Header("キャンバスグループ")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;

    [Header("可能時の透明度")]
    [SerializeField]
    private float m_possibleAlpha = 1.0f;

    [Header("可能事に表示する画像")]
    [SerializeField]
    private Image m_possibleImage = null;

    [Header("不可能時の透明度")]
    [SerializeField]
    private float m_impossibleAlpha = 0.5f;

    [Header("不可能事に表示する画像")]
    [SerializeField]
    private Image m_impossibleImage = null;


    //===========================================================
    //                       実行処理
    //===========================================================

    // 作成可能か確認
    public void CheckProvide(bool _flg)
    {
        #region nullチェック
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }
        #endregion

        if (_flg)
        {
            m_canvasGroup.alpha = m_possibleAlpha;

            if (m_impossibleImage) m_impossibleImage.gameObject.SetActive(false);
            if (m_possibleImage) m_possibleImage.gameObject.SetActive(true);
        }
        // 不可能事
        else
        {
            m_canvasGroup.alpha = m_impossibleAlpha;

            if (m_impossibleImage) m_impossibleImage.gameObject.SetActive(true);
            if (m_possibleImage) m_possibleImage.gameObject.SetActive(false);

        }
    }
}

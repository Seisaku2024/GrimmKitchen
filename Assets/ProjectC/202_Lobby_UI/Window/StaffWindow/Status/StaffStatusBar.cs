using Arbor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// 制作者（吉田）
/// 
/// ＴＭＰの数値から
/// FillAmountで長さを変更するスクリプト
/// 
/// </summary>
public class StaffStatusBar : MonoBehaviour
{
    [SerializeField]
    private Image m_barImage = null;

    [SerializeField]
    private TextMeshProUGUI m_TMPText = null;

    // 値
    private int m_value = -1;

    //======================
    // Tween
    [Header("アニメーション補完")]
    [SerializeField]
    private bool m_isDoTween = false;
    private Tween m_tween = null;

    //======================================
    //              実行処理
    //======================================

    private void Start()
    {
        if (m_barImage == null)
        {
            Debug.LogError("Imageがセットされてません。");
            // → ステータスをゲージ表記する、Imageをセットしてください。
            // セットしてても上手く行かない場合は、
            // Imageをアタッチしているオブジェクトが RectTransformになってない場合があります
            return;
        }

        // Imageコンポーネントの初期設定
        SettingImage();

        // 幅変更
        InitChangeWidth();
    }


    private void Update()
    {
        // テキストの値が変更された場合更新
        if (m_TMPText == null) return;
        if (int.TryParse(m_TMPText.text, out int result))
        {
            if (m_value != result)
            {
                m_value = result;
                ChangeWidth().Forget();
            }
        }
    }

    private void OnDestroy()
    {
        if (m_tween != null)
        {
            m_tween.Kill();
            m_tween = null;
        }
    }

    private void InitChangeWidth()
    {
        if (m_barImage == null) return;
        if (m_TMPText == null) return;

        // 値を更新
        if (int.TryParse(m_TMPText.text, out int result))
        {
            m_value = result;
        }

        float value = (float)m_value / (float)StaffManager.instance.MaxStatusValue;
        m_barImage.fillAmount = value;
    }

    // 横幅変更
    private async UniTask ChangeWidth()
    {
        if (m_barImage == null) return;
        if (m_TMPText == null) return;

        if (m_tween != null)
        {
            m_tween.Kill();
            m_tween = null;
        }

        // 値を更新
        if (int.TryParse(m_TMPText.text, out int result))
        {
            m_value = result;
        }

        if (m_isDoTween)
        {
            float targetRatio = (float)m_value / (float)StaffManager.instance.MaxStatusValue;

            m_tween = m_barImage.DOFillAmount(targetRatio, 0.8f).
                      SetEase(Ease.OutCubic).
                      SetLink(m_barImage.gameObject).
                      SetUpdate(true);
        }
        else
        {
            float value = (float)m_value / (float)StaffManager.instance.MaxStatusValue;
            m_barImage.fillAmount = value;
        }

        await UniTask.CompletedTask;
    }

    // 画像セット
    private void SettingImage()
    {
        if (m_barImage == null) return;

        m_barImage.type = Image.Type.Filled;
    }

}

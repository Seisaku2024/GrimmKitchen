using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

[DefaultExecutionOrder(100)]
public class ItemEffectController : MonoBehaviour
{
    // 素材に近づいた際にエフェクトを表示させる

    [Header("キラキラエフェクト")]
    [SerializeField] private GameObject m_sparklingEffect = null;
    private Tween m_onSparklingEffectTween = null;
    private Tween m_offSparklingEffectTween = null;

    [Header("オーラエフェクト")]
    [SerializeField] private GameObject m_auraEffect = null;
    private Tween m_onAuraEffectTween = null;
    private Tween m_offAuraEffectTween = null;

    [Header("光柱エフェクト")]
    [SerializeField] private GameObject m_lightPillarEffect = null;

    [Header("キラキラエフェクト表示するか")]
    [SerializeField] private bool m_bSparklingEffect = false;

    [Header("オーラエフェクト表示するか")]
    [SerializeField] private bool m_bAuraEffect = false;

    [Header("光柱エフェクト表示するか")]
    [SerializeField] private bool m_bLightPillarEffect = false;

    [Header("エフェクトが表示される範囲")]
    [SerializeField] private float m_sensingRange = 10.0f;

    [Header("Scaleの最大値")]
    [SerializeField] private float m_maxScale = 1.0f;

    [Header("Scaleの最小値")]
    [SerializeField] private float m_minScale = 0.0f;

    [Header("遷移までの時間")]
    [SerializeField] private float m_duration = 0.1f;

    private CharacterCore m_playerCore = null;

    private bool m_bStopFlg = false;
    private bool m_bStartFlg = false;

    // 現在エフェクトを表示しているか
    private bool m_isEffectVisible = false;

    private void Start()
    {
        InitializeEffects();
        FindPlayer();
    }

    private void Update()
    {
        if (m_playerCore == null)
        {
            FindPlayer();
        }

        if (m_playerCore == null)
        {
            return;
        }

        float dist = Vector3.Distance(
            m_playerCore.transform.position,
            transform.position
        );

        bool shouldShow = dist <= m_sensingRange;

        // 状態が変化していない場合は何もしない
        if (shouldShow == m_isEffectVisible)
        {
            return;
        }

        m_isEffectVisible = shouldShow;

        if (m_isEffectVisible)
        {
            OnItemEffect();
        }
        else
        {
            OffItemEffect();
        }
    }

    private void InitializeEffects()
    {
        // 使用しないエフェクトは無効化
        if (!m_bSparklingEffect && m_sparklingEffect != null)
        {
            m_sparklingEffect.SetActive(false);
            m_sparklingEffect = null;
        }

        if (!m_bAuraEffect && m_auraEffect != null)
        {
            m_auraEffect.SetActive(false);
            m_auraEffect = null;
        }

        if (!m_bLightPillarEffect && m_lightPillarEffect != null)
        {
            m_lightPillarEffect.SetActive(false);
            m_lightPillarEffect = null;
        }

        // 初期状態は非表示
        if (m_sparklingEffect != null)
        {
            m_sparklingEffect.transform.localScale =
                Vector3.one * m_minScale;

            m_sparklingEffect.SetActive(false);
        }

        if (m_auraEffect != null)
        {
            m_auraEffect.transform.localScale =
                Vector3.one * m_minScale;

            m_auraEffect.SetActive(false);
        }

        if (m_lightPillarEffect != null)
        {
            if (m_lightPillarEffect.TryGetComponent(
                out VisualEffect visual))
            {
                visual.Stop();
            }
        }

        m_isEffectVisible = false;
        m_bStartFlg = false;
        m_bStopFlg = true;
    }

    private void FindPlayer()
    {
        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara == null)
            {
                continue;
            }

            if (chara.GroupNo != CharacterGroupNumber.player)
            {
                continue;
            }

            m_playerCore = chara;
            break;
        }
    }

    public void OnItemEffect()
    {
        // ---------------------------------------------------------
        // キラキラエフェクト
        // ---------------------------------------------------------
        if (m_sparklingEffect != null &&
            m_bSparklingEffect)
        {
            if (m_offSparklingEffectTween != null)
            {
                m_offSparklingEffectTween.Kill();
                m_offSparklingEffectTween = null;
            }

            if (m_onSparklingEffectTween != null)
            {
                m_onSparklingEffectTween.Kill();
                m_onSparklingEffectTween = null;
            }

            m_sparklingEffect.SetActive(true);

            m_onSparklingEffectTween =
                m_sparklingEffect.transform
                    .DOScale(m_maxScale, m_duration)
                    .SetLink(m_sparklingEffect)
                    .OnComplete(() =>
                    {
                        m_onSparklingEffectTween = null;
                    });
        }

        // ---------------------------------------------------------
        // オーラエフェクト
        // ---------------------------------------------------------
        if (m_auraEffect != null &&
            m_bAuraEffect)
        {
            if (m_offAuraEffectTween != null)
            {
                m_offAuraEffectTween.Kill();
                m_offAuraEffectTween = null;
            }

            if (m_onAuraEffectTween != null)
            {
                m_onAuraEffectTween.Kill();
                m_onAuraEffectTween = null;
            }

            m_auraEffect.SetActive(true);

            m_onAuraEffectTween =
                m_auraEffect.transform
                    .DOScale(m_maxScale, m_duration)
                    .SetLink(m_auraEffect)
                    .OnComplete(() =>
                    {
                        m_onAuraEffectTween = null;
                    });
        }

        // ---------------------------------------------------------
        // 光柱エフェクト
        // ---------------------------------------------------------
        if (m_lightPillarEffect != null &&
            m_bLightPillarEffect)
        {
            m_lightPillarEffect.SetActive(true);

            if (m_lightPillarEffect.TryGetComponent(
                out VisualEffect visual))
            {
                if (!m_bStartFlg)
                {
                    visual.Play();

                    m_bStopFlg = false;
                    m_bStartFlg = true;
                }
            }
        }
    }

    public void OffItemEffect()
    {
        // ---------------------------------------------------------
        // キラキラエフェクト
        // ---------------------------------------------------------
        if (m_sparklingEffect != null &&
            m_bSparklingEffect)
        {
            if (m_onSparklingEffectTween != null)
            {
                m_onSparklingEffectTween.Kill();
                m_onSparklingEffectTween = null;
            }

            if (m_offSparklingEffectTween != null)
            {
                m_offSparklingEffectTween.Kill();
                m_offSparklingEffectTween = null;
            }

            m_offSparklingEffectTween =
                m_sparklingEffect.transform
                    .DOScale(m_minScale, m_duration)
                    .SetLink(m_sparklingEffect)
                    .OnComplete(() =>
                    {
                        if (m_sparklingEffect != null)
                        {
                            m_sparklingEffect.SetActive(false);
                        }

                        m_offSparklingEffectTween = null;
                    });
        }

        // ---------------------------------------------------------
        // オーラエフェクト
        // ---------------------------------------------------------
        if (m_auraEffect != null &&
            m_bAuraEffect)
        {
            if (m_onAuraEffectTween != null)
            {
                m_onAuraEffectTween.Kill();
                m_onAuraEffectTween = null;
            }

            if (m_offAuraEffectTween != null)
            {
                m_offAuraEffectTween.Kill();
                m_offAuraEffectTween = null;
            }

            m_offAuraEffectTween =
                m_auraEffect.transform
                    .DOScale(m_minScale, m_duration)
                    .SetLink(m_auraEffect)
                    .OnComplete(() =>
                    {
                        if (m_auraEffect != null)
                        {
                            m_auraEffect.SetActive(false);
                        }

                        m_offAuraEffectTween = null;
                    });
        }

        // ---------------------------------------------------------
        // 光柱エフェクト
        // ---------------------------------------------------------
        if (m_lightPillarEffect != null &&
            m_bLightPillarEffect)
        {
            if (m_lightPillarEffect.TryGetComponent(
                out VisualEffect visual))
            {
                if (!m_bStopFlg)
                {
                    visual.Stop();

                    m_bStopFlg = true;
                    m_bStartFlg = false;
                }
            }
        }
    }

    private void OnDestroy()
    {
        m_onSparklingEffectTween?.Kill();
        m_offSparklingEffectTween?.Kill();

        m_onAuraEffectTween?.Kill();
        m_offAuraEffectTween?.Kill();

        m_onSparklingEffectTween = null;
        m_offSparklingEffectTween = null;

        m_onAuraEffectTween = null;
        m_offAuraEffectTween = null;
    }
}
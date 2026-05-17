using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

[DefaultExecutionOrder(100)]
public class ItemEffectController : MonoBehaviour
{
    //素材に近づいた際にエフェクトを表示させる（山本）

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

    void Start()
    {
        if (!m_bSparklingEffect && m_sparklingEffect)
        {
            m_sparklingEffect.SetActive(false);
            m_sparklingEffect = null;
        }

        if (!m_bAuraEffect && m_auraEffect)
        {
            m_auraEffect.SetActive(false);
            m_auraEffect = null;
        }

        if (!m_bLightPillarEffect && m_lightPillarEffect)
        {
            m_lightPillarEffect.SetActive(false);
            m_lightPillarEffect = null;
        }


        //Scale値を調整-----------------------------------------------------
        if (m_sparklingEffect)
        {
            m_sparklingEffect.transform.localScale = Vector3.zero;
        }

        if (m_auraEffect)
        {
            m_auraEffect.transform.localScale = Vector3.zero;
        }

        if (m_lightPillarEffect)
        {
            // m_lightPillarEffect.transform.localScale = Vector3.zero;
        }
        //-----------------------------------------------------------------


        //プレイヤーが指定範囲内にいないか確認する
        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara.GroupNo != CharacterGroupNumber.player) continue;

            m_playerCore = chara;

        }


        //if (m_lightPillarEffect.TryGetComponent(out VisualEffect visual))
        //{
        //    visual.Play();
        //}


    }

    void Update()
    {
        ////プレイヤーが指定範囲内にいないか確認する
        //foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        //{
        //    if (chara.GroupNo != CharacterGroupNumber.player) continue;

        //    float dist = Vector3.Distance(chara.transform.position, this.transform.position);

        //    if (dist <= m_sensingRange)
        //    {
        //        OnItemEffect();
        //    }
        //    else
        //    {
        //        OffItemEffect();
        //    }

        //}

        if (m_playerCore == null)
        {
            //プレイヤーが指定範囲内にいないか確認する
            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.GroupNo != CharacterGroupNumber.player) continue;

                m_playerCore = chara;

            }
        }

        if (m_playerCore == null) { return; }

        float dist = Vector3.Distance(m_playerCore.transform.position, this.transform.position);

        if (dist <= m_sensingRange)
        {
            OnItemEffect();
        }
        else
        {
            OffItemEffect();
        }


    }

    public void OnItemEffect()
    {

        if (m_onSparklingEffectTween == null && m_sparklingEffect && m_bSparklingEffect)
        {
            // OffTweenが再生中であれば削除
            if (m_offSparklingEffectTween != null)
            {
                m_offSparklingEffectTween.Kill();
                m_offSparklingEffectTween = null;
            }

            m_sparklingEffect.SetActive(true);

            m_onSparklingEffectTween = m_sparklingEffect.transform.DOScale(m_maxScale, m_duration).
                SetLink(m_sparklingEffect);
        }


        if (m_onAuraEffectTween == null && m_auraEffect && m_bAuraEffect)
        {
            // OffTweenが再生中であれば削除
            if (m_offAuraEffectTween != null)
            {
                m_offAuraEffectTween.Kill();
                m_offAuraEffectTween = null;
            }

            m_auraEffect.SetActive(true);

            m_onAuraEffectTween = m_auraEffect.transform.DOScale(m_maxScale, m_duration).
                SetLink(m_auraEffect);
        }


        if (m_lightPillarEffect && m_bLightPillarEffect)
        {
            m_lightPillarEffect.SetActive(true);

            if (m_lightPillarEffect.TryGetComponent(out VisualEffect visual))
            {
                //if (visual.aliveParticleCount <= 0.0f)
                if (m_bStartFlg == false)
                {
                    visual.Play();

                    m_bStopFlg = false;
                    m_bStartFlg = true;
                }
            }

            //m_lightPillarEffect.transform.DOScale(m_maxScale, m_duration);
        }

    }

    public void OffItemEffect()
    {

        if (m_offSparklingEffectTween == null && m_sparklingEffect && m_bSparklingEffect)
        {
            // OnTweenが再生中であれば削除
            if (m_onSparklingEffectTween != null)
            {
                m_onSparklingEffectTween.Kill();
                m_onSparklingEffectTween = null;
            }

            m_offSparklingEffectTween = m_sparklingEffect.transform.DOScale(m_minScale, m_duration).
                SetLink(m_sparklingEffect).
                OnComplete(() =>
            {
                m_sparklingEffect.SetActive(false);
            }
                );
        }


        if (m_offAuraEffectTween == null && m_auraEffect && m_bAuraEffect)
        {
            // OnTweenが再生中であれば削除
            if (m_onAuraEffectTween != null)
            {
                m_onAuraEffectTween.Kill();
                m_onAuraEffectTween = null;
            }

            m_offAuraEffectTween = m_auraEffect.transform.DOScale(m_minScale, m_duration).
                SetLink(m_auraEffect).
                OnComplete(() =>
              {
                  m_auraEffect.SetActive(false);
              }
                 );
        }


        if (m_lightPillarEffect && m_bLightPillarEffect)
        {
            if (m_lightPillarEffect.TryGetComponent(out VisualEffect visual))
            {
                if (m_bStopFlg == false)
                {
                    visual.Stop();
                    m_bStopFlg = true;
                    m_bStartFlg = false;

                }
            }

        }

    }

}

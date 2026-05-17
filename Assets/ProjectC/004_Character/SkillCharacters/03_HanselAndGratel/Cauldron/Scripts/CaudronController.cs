using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CaudronController : MonoBehaviour
{
    // 釜のシェーダー、パーティクルのコントローラー(山本)
    // 着地判定も取得できる

    // 釜の炎関係--------------------------------------------
    [Header("炎のマテリアル")]
    [SerializeField] private Material m_fireMaterial = null;
    [Header("炎の元の値")]
    [SerializeField] private float m_nowFireNum = 0.06f;
    [Header("炎が見えなくなる値")]
    [SerializeField] private float m_vanishFireNum = 0.5f;
    [Header("炎が点火するまでの時間")]
    [SerializeField] private float m_ignitionTime = 0.5f;
    [Header("炎用ポイントライト")]
    [SerializeField] private Light m_pointLight = new Light();
    [Header("炎のポイントライトの最終インテンシティ")]
    [SerializeField] private float m_pointLightIntensity = 10.0f;
    //炎マテリアルのトゥィーン
    private Tween m_fireMaterialTween = null;
    //炎ポイントライトのトゥィーン
    private Tween m_firePointLightTween = null;
    //-------------------------------------------------------

    // パーティクルシステム関係------------------------------
    [Header("パーティクルシステム")]
    [Header("煙")]
    [SerializeField] private ParticleSystem m_smokeParticleSystem = null;
    [Header("泡")]
    [SerializeField] private ParticleSystem m_bubbleParticleSystem = null;
    [Header("火花")]
    [SerializeField] private ParticleSystem m_sparkParticleSystem = null;

    [Header("大噴火パーティクルリスト")]
    [SerializeField] private List<ParticleSystem> m_fireThowerList = new List<ParticleSystem>();

    //-------------------------------------------------------

    // 着地しているかどうか
    private bool m_bOnGroundFlg = false;
    public bool OnGroundFlg => m_bOnGroundFlg;


    //　釜に着火する関数
    public void Ignition()
    {
        if (m_fireMaterial == null || m_pointLight == null) return;

        // 炎マテリアルのイージング処理
        m_fireMaterialTween = DOVirtual.Float
            (m_vanishFireNum,
            m_nowFireNum,
            m_ignitionTime,
            value =>
            {
                m_fireMaterial.SetFloat("Vector1_EB6C467F", value);
            });

        // 炎用ポイントライトのイージング処理
        m_firePointLightTween = DOVirtual.Float
            (0.0f,
            m_pointLightIntensity,
            m_ignitionTime,
            value =>
            {
                m_pointLight.intensity = value;
            }
             ).OnComplete
             (
            () =>
            {
                m_smokeParticleSystem.Play();
                m_bubbleParticleSystem.Play();
                m_sparkParticleSystem.Play();
            }
            );

    }

    //　釜を消化する関数
    public void Digestion()
    {
        if (m_fireMaterial == null || m_pointLight == null) return;

        // 炎マテリアルのイージング処理
        m_fireMaterialTween = DOVirtual.Float
            (m_nowFireNum,
            m_vanishFireNum,
            m_ignitionTime,
            value =>
            {
                m_fireMaterial.SetFloat("Vector1_EB6C467F", value);
            });

        // 炎用ポイントライトのイージング処理
        m_firePointLightTween = DOVirtual.Float
            (m_pointLightIntensity,
            0.0f,
            m_ignitionTime,
            value =>
            {
                m_pointLight.intensity = value;
            }
             ).OnComplete
             (
            () =>
            {
                m_smokeParticleSystem.Stop();
                m_bubbleParticleSystem.Stop();
                m_sparkParticleSystem.Stop();
            }
            );

    }


    // 大噴火発生
    public void StartLargeEruption()
    {
        foreach(var particle in m_fireThowerList)
        {
            particle.Play();
        }

    }

    // 大噴火エフェクトが停止しているかどうか
    public bool IsStopLargeEruption()
    {
        bool finishFlg = false;

        foreach (var particle in m_fireThowerList)
        {
            if(particle.IsAlive())
            {
                finishFlg = false;
            }
            else
            {
                finishFlg = true;
            }
        }

        return finishFlg;

    }



    private void Awake()
    {
        m_bOnGroundFlg = false;
    }

    void Start()
    {
        // 初期化

        if (m_pointLight)
            m_pointLight.intensity = 0.0f;

        if (m_fireMaterial)
            m_fireMaterial.SetFloat("Vector1_EB6C467F", m_vanishFireNum);

    }

    private void OnDestroy()
    {
        m_fireMaterialTween = null;
        m_fireMaterialTween.Kill();

        m_firePointLightTween = null;
        m_firePointLightTween.Kill();

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("NoHitEnemySkillCharacter"))
        {
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBarrier"))
        {
            return;
        }

        Debug.Log(collision.gameObject.name);

        if(this.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }

        m_bOnGroundFlg = true;
    }

}

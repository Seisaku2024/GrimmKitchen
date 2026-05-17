using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

[DefaultExecutionOrder(10)]
public class SetAlphaDitherObject : MonoBehaviour
{
    [SerializeField]
    private Transform m_wallTrans;

    [Header("通常→アルファディザに変化するまでの時間")]
    [SerializeField]
    private float m_changeAlphaDitherTime = 0.5f;

    [Header("アルファディザ→通常に変化するまでの時間")]
    [SerializeField]
    private float m_changeNormalTime = 1.0f;


    private Material m_material;

    private Transform m_playerTrans;
    private bool m_bInRoomFlg = false;

    private Tween m_tween;
    private float m_nowAdjustAlphaNum = 0.0f;

    public void Awake()
    {
        if (m_wallTrans == null) { return; }

        if (m_wallTrans.gameObject.TryGetComponent<Renderer>(out var mesh))
        {
            m_material = mesh.sharedMaterial;

            m_material.SetInt("InsideRoomFlg", 0);
            m_material.SetFloat("_AdjustDitherNum", 1.0f);

        }

        m_nowAdjustAlphaNum = 1.0f;

    }

    public void Start()
    {
        m_bInRoomFlg = false;

        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara.GroupNo == CharacterGroupNumber.player)
            {
                m_playerTrans = chara.transform;
                break;
            }
        }
    }

    public void Update()
    {
        if (m_material == null) return;

        if (m_bInRoomFlg)
        {
            m_material.SetVector("_TargetPosition", m_playerTrans.position);
        }
    }

    private void OnDestroy()
    {
        DOTween.Kill(m_tween);
        m_material = null;
        Destroy(m_material);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }
        if (m_material == null) return;

        m_material.SetInt("_InsideRoomFlg", 1);
        m_bInRoomFlg = true;

        float nowAdjustAlphaNum = m_nowAdjustAlphaNum;

        m_tween.Kill();

        //Dotweenで徐々にディザ抜きする
        m_tween = DOVirtual.Float(nowAdjustAlphaNum, 0.0f, m_changeAlphaDitherTime,
            value =>
            {
                m_nowAdjustAlphaNum = value;
                m_material.SetFloat("_AdjustDitherNum", value);
            }
            );

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }
        if (m_material == null) return;

        m_bInRoomFlg = false;

        float nowAdjustAlphaNum = m_nowAdjustAlphaNum;

        m_tween.Kill();

        //Dotweenで徐々にもとに戻す
        m_tween = DOVirtual.Float(nowAdjustAlphaNum, 1.0f, m_changeNormalTime,
            value =>
            {
                m_nowAdjustAlphaNum = value;
                m_material.SetFloat("_AdjustDitherNum", value);
            }
            ).OnComplete
            (
            () =>
            {
                m_material.SetInt("_InsideRoomFlg", 0);
            }
            );
    }




}

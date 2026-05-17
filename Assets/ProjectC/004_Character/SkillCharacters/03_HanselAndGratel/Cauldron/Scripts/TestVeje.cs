using Arbor;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class TestVeje : MonoBehaviour
{
    // ベジェ曲線
    [Header("終着点（この位置がベジェ曲線の終点になる）")]
    [SerializeField]
    private Transform m_endPoint;
    [Header("ベジェ曲線の制御点の高さ")]
    [SerializeField]
    private float m_height = 5.0f;
    [Header("制御点を2直線のどの割合に位置するか")]
    [SerializeField, Range(0.0f, 1.0f)]
    private float m_persentage = 0.7f;
    [Header("ベジェ曲線で指定地点まで移動する時間")]
    [SerializeField]
    private float m_duration = 2.0f;
    [Header("ベジェ曲線の終点をずらすためのランダム値")]
    [SerializeField]
    private float m_randomNum = 1.0f;

    //　釜への落下
    [Header("落下の終点")]
    [SerializeField]
    private Transform m_endPosint;
    [Header("落下するまでの時間")]
    [SerializeField]
    private float m_fallTime = 1.0f;
    [Header("落下後最終スケール値")]
    [SerializeField]
    private float m_targetScale = 0.5f;

    [SerializeField]
    private PathFinding m_pathFinding;


    private Transform m_startPoint;   // 開始点
    private Vector3 m_controlPoint = new Vector3(); // 制御点


    //ベジェ曲線の始点と終点のベクトル
    private Vector3 m_targetVec;

    //ベジェ曲線の終点
    private Vector3 m_calcEndPosition = new Vector3();

    private void Awake()
    {
        if (m_endPosint == null) { return; }

        m_startPoint = this.transform;
        m_calcEndPosition = m_endPoint.position
            + new Vector3(Random.Range(-m_randomNum, m_randomNum), Random.Range(-m_randomNum, m_randomNum), Random.Range(-m_randomNum, m_randomNum));
        Vector3 point = Vector3.Lerp(m_startPoint.position, m_calcEndPosition, m_persentage);
        m_controlPoint = point + new Vector3(0, m_height, 0);


        if (gameObject.TryGetComponent(out CharacterCore characterCore))
        {
            characterCore.SetRotateToTarget(m_controlPoint, false);
        }

        if (m_pathFinding)
        { 
            m_pathFinding.enabled = false;
        }

        

    }

    private void Start()
    {
        //鍋の上へと移動
        DOVirtual.Float(0.0f, 1.0f, m_duration,
            value =>
            {
                // ベジェ曲線の計算
                Vector3 position = CalculateBezierPoint
                (value,
                m_startPoint.position,
                m_controlPoint,
                m_calcEndPosition);

                if (m_pathFinding)
                {
                   //m_pathFinding.SetDestination(position);
                }
                else
                {
                    transform.position = position;
                }


                if (gameObject.TryGetComponent(out CharacterCore characterCore))
                {
                    characterCore.CharaCtrl.SetPositionMotor(position);
                    characterCore.CharaCtrl.Gravity.y = 0;
                }
            }
            ).OnComplete
            (
            () =>
            {
                if (m_endPosint == null) { return; }
                //下に落下とスケール値調整
                DOVirtual.Vector3(m_calcEndPosition, m_endPosint.position, m_fallTime,
                    value =>
                    {
                        transform.position = value;
                        if (gameObject.TryGetComponent(out CharacterCore characterCore))
                        {
                            characterCore.CharaCtrl.SetPositionMotor(value);
                        }




                    }).SetEase(Ease.InQuad);

                if (gameObject.TryGetComponent(out CharacterCore characterCore))
                {
                    characterCore.m_animator.transform.DOScale(m_targetScale, m_fallTime);
                }
                else
                {
                    transform.DOScale(0.5f, m_fallTime).SetEase(Ease.InBounce);
                }

            }
            ).SetEase(Ease.InQuad);

    }

    void Update()
    {
        //if (m_pathFinding)
        //    m_pathFinding.Stop();

    }

    // ベジェ曲線の計算式
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return point;
    }
}

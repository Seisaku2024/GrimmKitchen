using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectScaleDotween : MonoBehaviour
{
    [Header("サイズを調整するオブジェクトのTransform")]
    [SerializeField]
    private Transform m_adjustSizeObjTrans = null;

    [Header("最大サイズ")]
    [SerializeField]
    private float m_maxObjSize = 1.0f;

    [Header("最大サイズに到達するまでの時間")]
    [SerializeField]
    private float m_completeTime = 1.0f;

    private DOTween m_tween = null;

    public void ChangeDoTweenScalObject()
    {
        m_adjustSizeObjTrans.DOScale(m_maxObjSize, m_completeTime);
    }
   
}

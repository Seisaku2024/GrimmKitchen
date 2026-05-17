using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 3D空間でもUIの大きさが一定になるようにする（吉田）
/// 追加でClumpを使用して一定以上大きくならないように調整
/// </summary>
public class LabelTransControllerAdjustSize : MonoBehaviour
{
    // ターゲット HPバーを持つオブジェクト（キャラクター）
    [SerializeField] private GameObject m_target;

    // 大きさを一定にするためのキャンバス
    [SerializeField] private RectTransform m_canvasRect;

    // キャンバスの基本大きさ 大きすぎたらこの値を変える
    [SerializeField] private float m_baseScale = 0.0005f;

    [SerializeField] private float m_maxSize = 0.8f;
    [SerializeField] private float m_minSize = 0.5f;


    private void Update()
    {
        UpdateLabelTrans();
    }

    [ContextMenu("UpdateLabelTrans")]
    private void UpdateLabelTrans()
    {
        float distance = GetDistance();

        Vector3 scale = Vector3.one * m_baseScale * distance;
        scale = Mathf.Clamp(scale.x, m_minSize, m_maxSize) * Vector3.one;
        // スケール
        m_canvasRect.transform.localScale = scale;
    }

    private float GetDistance()
    {
        Transform cameraTrans = Camera.main.transform;
        return (m_target.gameObject.transform.position - cameraTrans.position)
            .magnitude;
    }
}

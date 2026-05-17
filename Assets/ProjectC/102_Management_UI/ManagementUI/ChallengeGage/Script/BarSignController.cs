using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 制作者 吉田 (追加 田内)
/// 
/// バーの一点を示すのコントローラー
/// 0~1の割合で位置を変更する
/// 
/// Startで処理を行う
/// </summary>

public class BarSignController : MonoBehaviour
{

    [Tooltip("0～1の割合で位置を変更できるコントローラー")]

    //===========================================
    // 2D行列
    private RectTransform m_gageTransform = null;

    //=================================================
    [Header("コンテクストメニューで座標変わります")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_rate = 0.0f;
    public float Rate { get => m_rate; set => m_rate = value; }

    //=================================================
    //                  実行処理
    //=================================================

    private void Start()
    {
        SetPos();
    }


    public void SetPos(float _rate)
    {
        // 親オブジェクトの画像を基に
        if (m_gageTransform == null)
        {
            if (transform.parent.TryGetComponent(out RectTransform gageTransform) == false)
            {
                Debug.LogError("m_gageTransform is null");
                return;
            }
            m_gageTransform = gageTransform;
        }

        // 左端
        float leftEdge = m_gageTransform.localPosition.x - (m_gageTransform.rect.width / 2.0f);

        // 右端
        float rightEdge = m_gageTransform.localPosition.x + (m_gageTransform.rect.width / 2.0f);

        Vector3 pos = transform.localPosition;
        float xPos = Mathf.Lerp(leftEdge, rightEdge, _rate);
        pos = new(xPos, pos.y, pos.z);

        transform.localPosition = pos;
    }


    public void SetPos(float _maxValue, float _currntValue)
    {
        // もし0だった場合
        if (_maxValue == 0.0f)
        {
            m_rate = 0.0f;
        }
        // 現在の割合(0～1)
        else
        {
            m_rate = _currntValue / _maxValue;
        }
        SetPos(m_rate);
    }


    [ContextMenu("SetPos")]
    private void SetPos()
    {
        SetPos(m_rate);
    }


}

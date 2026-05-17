using Cysharp.Threading.Tasks;
using SaintsField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自動でスクロールするコンポーネント 制作者（吉田）
/// 
/// </summary>
public class AutoScrollView : MonoBehaviour
{
    [SerializeField]
    private Scrollbar m_scrollbar = null;

    [SerializeField]
    private GameObject m_content = null;

    [SerializeField]
    [Header("スロットがいくつ以上ならスクロールするか")]
    private int m_slotMinNum = 10;

    [SerializeField]
    [Header("止まる時間 1000で1秒")]
    private int m_stopTime = 3000;
    [SerializeField]
    private float m_speed = 0.003f;

    [ReadOnly]
    private bool m_isStop = false;


    // Start is called before the first frame update
    void Start()
    {
        if (m_scrollbar == null)
        {
            m_scrollbar = GetComponentInChildren<Scrollbar>();
        }
        m_isStop = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_scrollbar == null) return;
        if (m_content.transform.childCount < m_slotMinNum) return;

        if (m_isStop) return;

        m_scrollbar.value -= m_speed * Time.unscaledDeltaTime;

        if (m_scrollbar.value <= 0.0f)
        {
            DelayReset();
        }
    }

    // 少し待つ→Topに戻す→少し待つ
    private async UniTask DelayReset()
    {
        m_isStop = true;

        await UniTask.Delay(m_stopTime, true);
        m_scrollbar.value = 1.0f;
        await UniTask.Delay(m_stopTime, true);

        m_isStop = false;
    }
}

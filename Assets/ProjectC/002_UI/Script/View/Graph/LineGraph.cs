using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaintsField;
using UnityEngine.UI;
using SaintsField.Playa;

public class LineGraph : MonoBehaviour
{
    [AboveButton(nameof(CreateGraphForEditer))]

    [SerializeField]
    [Range(1, 40)]
    private int m_widthSplit = 5;

    [SerializeField]
    private float m_maxValue = 5f;

    [SerializeField]
    private List<float> m_valueList = new();
    public List<float> ValueList { get { return m_valueList; } set { m_valueList = value; } }
    public void AddValue(float _value)
    {
        m_valueList.Add(_value);
        int count = m_valueList.Count;
        if (count <= m_widthSplit) return;

        int removeCount = count - m_widthSplit;
        for (int i = 0; i < removeCount; i++)
        {
            m_valueList.RemoveAt(0);
        }
    }


    [Space(15)]

    [SerializeField]
    [Header("Dot Pref")]
    private GameObject m_dotPref = null;

    [SerializeField]
    [Header("Line Pref")]
    private GameObject m_linePref = null;

    private RectTransform m_rectTransform;


    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void CreateGraph()
    {
        // リスト内の最大値を取得
        m_maxValue = GetMaxValue();

        // 高さ
        float graphHeight = m_rectTransform.sizeDelta.y;
        // 間隔幅
        float width = m_rectTransform.sizeDelta.x / (float)(m_widthSplit - 1);

        Vector2 LastPos = Vector2.zero;
        for (int i = 0; i < m_valueList.Count; i++)
        {
            // 点
            float posX = i * width;
            float value = m_valueList[i];
            if (value > m_maxValue) value = m_maxValue;
            float posY = (value / m_maxValue) * graphHeight;
            CreateDot(new Vector2(posX, posY));

            // 線
            if (i != 0)// 1回目は飛ばす。
            {
                CreateLine(LastPos, new Vector2(posX, posY));
            }

            LastPos = new Vector2(posX, posY);
        }
    }

    public void ResetGraph()
    {
        foreach (Transform child in gameObject.transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void CreateDot(Vector2 _position)
    {
        if (m_dotPref == null) return;

        GameObject objDot = Instantiate(m_dotPref, gameObject.transform);
        RectTransform rtDot = objDot.GetComponent<RectTransform>();
        if (rtDot == null) return;

        rtDot.anchoredPosition = _position;
    }

    private void CreateLine(Vector2 _position, Vector2 _toPosition)
    {
        if (m_linePref == null) return;

        GameObject objLine = Instantiate(m_linePref, gameObject.transform);
        RectTransform rtList = objLine.GetComponent<RectTransform>();
        if (rtList == null) return;

        Vector2 dir = (_toPosition - _position).normalized;
        float dist = Vector2.Distance(_toPosition, _position);

        // 幅・長さ
        Vector2 size = rtList.sizeDelta;
        rtList.sizeDelta = new Vector2(dist, rtList.sizeDelta.y);

        // 角度
        rtList.localEulerAngles = new Vector3(0f, 0f,
            Vector2.SignedAngle(new Vector2(1.0f, 0.0f), dir));

        // 位置
        rtList.anchoredPosition = _position + (dir * dist * 0.5f);
    }

    [ContextMenu("CreateGraph")]
    private void CreateGraphForEditer()
    {
        if (m_rectTransform == null)
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        foreach (Transform child in gameObject.transform)
        {
            DestroyImmediate(child.gameObject);
        }

        CreateGraph();
    }

    private float GetMaxValue()
    {
        float maxValue = 0f;
        foreach (var value in m_valueList)
        {
            if (value > maxValue) maxValue = value;
        }
        return maxValue;
    }
}

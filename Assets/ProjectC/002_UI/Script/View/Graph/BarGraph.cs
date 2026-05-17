using SaintsField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarGraph : MonoBehaviour
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
    [Header("Bar Pref")]
    private GameObject m_barPref = null;

    private RectTransform m_rectTransform;


    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }


    public void CreateGraph()
    {
        // リスト内の最大値を取得
        m_maxValue = GetMaxValue();

        // 高さ
        float graphHeight = m_rectTransform.sizeDelta.y;
        // 間隔幅
        float width = m_rectTransform.sizeDelta.x / (float)(m_widthSplit - 1);

        for (int i = 0; i < m_valueList.Count; i++)
        {
            // 点
            float posX = i * width;
            float value = m_valueList[i];
            if (value > m_maxValue) value = m_maxValue;
            CreateBar((value / m_maxValue), new Vector2(posX, 0f));
        }
    }

    private void CreateBar(float _lengthRate, Vector2 _pos)
    {
        if (m_barPref == null) return;

        GameObject objBar = Instantiate(m_barPref, transform);
        RectTransform rtBar = objBar.GetComponent<RectTransform>();
        if (rtBar == null) return;

        // 座標
        rtBar.anchoredPosition = _pos;

        // 横幅はプレハブのものを使う
        Vector2 size = rtBar.sizeDelta;
        // 縦の高さだけ親の高さと同じにする
        rtBar.sizeDelta = new Vector2(size.x, m_rectTransform.sizeDelta.y);

        Image image = rtBar.GetComponent<Image>();
        if (image == null) return;
        image.fillAmount = _lengthRate;
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

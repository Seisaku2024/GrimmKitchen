using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NowInventoryNum : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_maxInventoryNumText = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_maxInventoryNumText == null)
        {
            m_maxInventoryNumText = GetComponent<TextMeshProUGUI>();
            if (!m_maxInventoryNumText)
            {
                Debug.LogError("TextMeshProUGUIがアタッチされていません");
                return;
            }
        }

        m_maxInventoryNumText.text = InventoryManager.instance.ItemDataRC.Count.ToString();
    }
}

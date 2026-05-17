using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSelectImageColor : MonoBehaviour
{
    // 選択したスライダーなどのイメージの色を変更する処理(山本)

    [Header("選択された時の色")]
    [SerializeField]
    private Color m_selectColor = Color.white;
    [Header("選択されてない時の色")]
    [SerializeField]
    private Color m_notSelectColor = Color.white;
    [Header("色を変更するイメージのリスト")]
    [SerializeField]
    private List<Image> m_imageList = new List<Image>();

    [Header("色を変更するテキストメッシプロ（マテリアル）")]
    [SerializeField]
    private TextMeshProUGUI m_meshPro = null;


    private bool m_bSelectChangeCol = false;
    private bool m_bSelectNoChangeCol = true;

    public void Start()
    {
        m_bSelectChangeCol = false;
        m_bSelectNoChangeCol = true;
    }

    public void ChangeSelectColor()
    {
       
        foreach (Image image in m_imageList)
        {
            image.color = m_selectColor;
        }

        m_meshPro.fontSharedMaterial.SetColor("_FaceColor", m_selectColor);

    }

    public void ChangeNotSelectColor()
    {
        foreach (Image image in m_imageList)
        {
            image.color = m_notSelectColor;
        }

        m_meshPro.fontSharedMaterial.SetColor("_FaceColor", m_notSelectColor);

    }

}

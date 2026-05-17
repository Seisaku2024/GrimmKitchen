using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextureAlphaNum : MonoBehaviour
{
    // セットしたテクスチャ（Image）のアルファ値を変換する（山本）
    [SerializeField]
    private Image m_image;

    
    public void SetImageAlphaNum(float _alphaNum)
    {
        m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, _alphaNum);
    }

}

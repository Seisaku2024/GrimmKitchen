using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TwoColorGrad : MonoBehaviour
{
    [SerializeField]
    private Shader m_shader;

    private Material m_material;

    private Image m_image;


    public void Initialize()
    {
        m_image = GetComponent<Image>();
        m_material = new Material(m_shader);
        m_image.material = m_material;
    }

    public void SetColor(Color _color1, Color _color2, float _alpha)
    {
        if (m_material == null)
        {
            Initialize();
        }
        m_material.SetColor("_Color1", _color1);
        m_material.SetColor("_Color2", _color2);
        m_material.SetFloat("_Alpha", _alpha);
    }

    public void SetColor(Color _color1,float _alpha)
    {
        if (m_material == null)
        {
            Initialize();
        }
        m_material.SetColor("_Color1", _color1);
        m_material.SetColor("_Color2", _color1);
        m_material.SetFloat("_Alpha", _alpha);
    }

    void OnDestroy()
    {
        Destroy(m_material);
    }
}

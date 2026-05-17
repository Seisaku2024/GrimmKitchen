using CriWare;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-5)]
public class AudioPlayVolumeTest : MonoBehaviour
{

    [SerializeField]
    private ValueController m_valueController;

    public CriAtomSource m_bgmCriAtomSource;
    public string m_categoryName;
    private Slider m_slider;

    private void Start()
    {
        m_slider = GetComponentInChildren<Slider>();

        if (m_slider)
        {
            m_slider.value = PlayerPrefs.GetFloat(m_categoryName + "Vol");
        }
    }

    public void PlaySound()
    {
        if (m_bgmCriAtomSource == null) return;
        if (m_bgmCriAtomSource.status == CriAtomSource.Status.Playing)
        {
            m_bgmCriAtomSource.Stop();
        }
        else
        {
            m_bgmCriAtomSource.Play();
        }
    }

    public void SetVolume()
    {
        if (m_valueController)
        {
            var soundVolume = m_slider.value / m_valueController.MaxValue;
            CriAtom.SetCategoryVolume(m_categoryName, soundVolume);
        }
        else
        {
            CriAtom.SetCategoryVolume(m_categoryName, m_slider.value);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class VFXEffectRegistry : MonoBehaviour
{
    // VFX登録するコンポーネント（山本）
    [SerializeField]
    private SerializableDictionary<string, VisualEffect> m_vfxResistoryList;

    public SerializableDictionary<string, VisualEffect> VFXResistoryList { get { return m_vfxResistoryList; } }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSetLayerMask : MonoBehaviour
{
    [SerializeField]
    private LayerMask m_layerMask;

    [Header("カメラで写す対象をまとめたリスト")]
    [SerializeField]
    private SerializableDictionary<string, LayerMask> m_dictionaryCameraMasklList;

    private Camera m_mainCamera;

    void Start()
    {
        m_mainCamera = Camera.main;
        SetLayerMask("All");
    }

    public void SetLayerMask(string name)
    {
        if (m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }

        if (m_mainCamera == null)
        {
            Debug.LogWarning("MainCamera が見つかりません。");
            return;
        }

        if (m_dictionaryCameraMasklList.TryGetValue(name, out LayerMask layer))
        {
            m_mainCamera.cullingMask = layer;
        }
        else
        {
            Debug.LogWarning($"LayerMask '{name}' が Dictionary に登録されていません。");
        }
    }

}

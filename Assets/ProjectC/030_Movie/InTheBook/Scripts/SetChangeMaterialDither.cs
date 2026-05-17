using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChangeMaterialDither : MonoBehaviour
{
    [SerializeField]
    private List<Material> m_materialList = null;

    private List<float> m_maxDistList = new List<float>();
    private List<float> m_minDistList = new List<float>();


    private void Start()
    {
        float maxDist = 0.0f;
        float minDist = 0.0f;

        foreach (var material in m_materialList)
        {
            if (material.HasFloat("_MaxDist") && material.HasFloat("_MinDist"))
            {
                maxDist = material.GetFloat("_MaxDist");
                minDist = material.GetFloat("_MinDist");

                m_maxDistList.Add(maxDist);
                m_minDistList.Add(minDist);
            }
        }

    }

    public void SetNoAlphaDither()
    {
        foreach (var material in m_materialList)
        {
            material.SetFloat("_MaxDist", 0.0f);
            material.SetFloat("_MinDist", 0.0f);
        }
    }

    public void SetAlphaDither()
    {
        int i = 0;

        foreach (var material in m_materialList)
        {
            material.SetFloat("_MaxDist", m_maxDistList[i]);
            material.SetFloat("_MinDist", m_minDistList[i]);
            i++;
        }

    }

}

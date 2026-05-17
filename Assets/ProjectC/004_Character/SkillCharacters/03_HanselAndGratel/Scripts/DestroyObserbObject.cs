using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObserbObject : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject m_obserbObject = null;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_obserbObject == null) return;
        if (m_obserbObject.activeSelf == false)
        {
            Destroy(gameObject);
        }

    }
}

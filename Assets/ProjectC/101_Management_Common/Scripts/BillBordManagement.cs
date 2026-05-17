using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBordManagement : MonoBehaviour
{
    private Transform _Trasnform;

    private void Start()
    {
        // Cache Transform.
        _Trasnform = transform;
    }

    void LateUpdate()
    {
        var CamTrans = Camera.main.transform;
        CamTrans.rotation=Quaternion.Euler(0,CamTrans.eulerAngles.y, CamTrans.eulerAngles.z);
        // Adjust to camera direction.
        _Trasnform.forward = CamTrans.forward;
    }
}

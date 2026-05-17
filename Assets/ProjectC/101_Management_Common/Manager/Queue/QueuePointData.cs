using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuePointData : MonoBehaviour
{
    // 制作者 田内

    private void Start()
    {
        if (QueueManager.instance == null) return;
        QueueManager.instance.SetHeadPoint(gameObject);
    }
}

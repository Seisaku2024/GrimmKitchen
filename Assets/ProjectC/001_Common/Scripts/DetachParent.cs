using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//　自身の子オブジェクトとの関係を解除するコンポーネント（山本）
public class DetachParent : MonoBehaviour
{
    void Start()
    {
        transform.parent = null;
    }
}

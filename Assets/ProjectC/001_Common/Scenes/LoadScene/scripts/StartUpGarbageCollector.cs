using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class StartUpGarbageCollector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        // インクリメンタルガーベッジコレクション解放(山本)
        //GarbageCollector.CollectIncremental(nanoseconds: 0UL);
        Debug.Log("Collecting garbage");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

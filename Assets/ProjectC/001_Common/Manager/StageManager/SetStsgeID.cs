using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageInfo;

public class SetStsgeID : MonoBehaviour
{
    // 制作者 田内
    // ステージID

    [Header("セットするステージID")]
    [SerializeField]
    private StageID m_stageID = StageID.Stage01;

    //=================================
    //          実行処理
    //=================================

    void Start()
    {
        StageManager.instance.SetStageID(m_stageID);
    }
}

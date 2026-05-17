using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageInfo;

public class StageButton : ButtonData
{
    // 制作者 田内
    // ステージボタン

    [Header("=================ステージID=================")]
    [SerializeField]
    private StageID m_stageID = StageID.Stage01;

    public StageID StageID
    {
        get { return m_stageID; }
    }

    //===================================
    //          実行処理
    //===================================


    protected override void Start()
    {
        // ステージ選択できるかどうかをセット
        var data = StageDataBaseManager.instance.GetStageData(m_stageID);
        if (data != null)
        {
            // ロックされていなければ
            m_isUse = !data.ClearStageData.IsLock;
        }

        base.Start();
    }


}

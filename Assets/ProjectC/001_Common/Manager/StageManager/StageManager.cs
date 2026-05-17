using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageInfo;

public class StageManager : BaseManager<StageManager>
{
    // 制作者 田内
    // ステージ情報を管理する

    private StageID m_stageID = StageID.Stage01;

    public StageID StageID
    {
        get { return m_stageID; }
    }

    //=======================================
    //              実行処理
    //=======================================

    public void SetStageID(StageID _id)
    {
        m_stageID = _id;
    }

    public void StageClear()
    {
        var data = StageDataBaseManager.instance.GetStageData(m_stageID);
        if (data == null) return;

        data.SetClear();
    }

}

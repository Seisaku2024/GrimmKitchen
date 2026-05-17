using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStageDataSaveLoaderController : MonoBehaviour
{
    // 制作者 田内
    // デバッグ用処理

    [Header("デバッグ用データ")]
    [SerializeField]
    private List<ClearStageDataSaveLoad> m_clearStageDataSaveLoad = new();

    //===============================================
    //              実行処理
    //===============================================

    /// <summary>
    /// リスト上にあるデータをファイルにセーブする
    /// </summary>

    [ContextMenu("DebugSave")]
    private void DebugSave()
    {
        if (m_clearStageDataSaveLoad == null) return;
        foreach (var data in m_clearStageDataSaveLoad)
        {
            if (data == null) continue;
            StageDataSaveLoader.Save(data.StageID, data);
        }
    }

    /// <summary>
    /// 全てのデータを初期化してファイルにセーブする
    /// </summary>

    [ContextMenu("DebugDeleteAllData")]
    private void DebugDeleteData()
    {
        StageDataSaveLoader.DeleteAllData();
    }

    /// <summary>
    /// 現状のデータをファイルにセーブする
    /// ※実行中にのみ動作
    /// </summary>

    [ContextMenu("DebugSaveCurrentData")]
    private void DebugSaveCurrentData()
    {
        // 現状のデータをセーブ
        StageDataSaveLoader.SaveCurrentData();
    }
}

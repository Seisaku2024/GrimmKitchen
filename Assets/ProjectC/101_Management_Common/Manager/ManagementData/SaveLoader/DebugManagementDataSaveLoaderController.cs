using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManagementDataSaveLoaderController : MonoBehaviour
{
    // 制作者 田内
    // デバッグ用処理

    [Header("デバッグ用経営データ")]
    [SerializeField]
    private ManagementDataSaveLoad m_managementDataSaveLoad = null;

    //===============================================
    //              実行処理
    //===============================================

    /// <summary>
    /// リスト上にあるデータをファイルにセーブする
    /// </summary>

    [ContextMenu("DebugSave")]
    private void DebugSave()
    {
        if (m_managementDataSaveLoad == null) return;
        ManagementDataSaveLoader.Save(m_managementDataSaveLoad);
    }

    /// <summary>
    /// 全てのデータを初期化してファイルにセーブする
    /// </summary>

    [ContextMenu("DebugDeleteAllData")]
    private void DebugDeleteAllData()
    {
        ManagementDataSaveLoader.DeleteAllData();
    }

    /// <summary>
    /// 現状のデータをファイルにセーブする
    /// ※実行中にのみ動作
    /// </summary>

    [ContextMenu("DebugSaveCurrentData")]
    private void DebugSaveCurrentData()
    {
        // 現状のデータをセーブ
        ManagementDataSaveLoader.SaveCurrentData();
    }
}

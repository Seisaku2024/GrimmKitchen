using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStaffSaveLoaderController : MonoBehaviour
{
    // 制作者 田内
    // デバッグ用処理

    [Header("デバッグ用スタッフデータ")]
    [SerializeField]
    private StaffDataSaveLoad m_staffDataSaveLoad = new();

    //===============================================
    //              実行処理
    //===============================================

    /// <summary>
    /// リスト上にあるデータをファイルにセーブする
    /// </summary>

    [ContextMenu("DebugSave")]
    private void DebugSave()
    {
        if (m_staffDataSaveLoad == null) return;
        StaffDataSaveLoader.Save(m_staffDataSaveLoad);
    }

    /// <summary>
    /// 全てのデータを初期化してファイルにセーブする
    /// </summary>

    [ContextMenu("DebugDeleteAllData")]
    private void DebugDeleteAllData()
    {
       StaffDataSaveLoader.DeleteAllData();
    }

    /// <summary>
    /// 現状のデータをファイルにセーブする
    /// ※実行中にのみ動作
    /// </summary>

    [ContextMenu("DebugSaveCurrentData")]
    private void DebugSaveCurrentData()
    {
        // 現状のデータをセーブ
        StaffDataSaveLoader.SaveCurrentData();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FoodInfo;

public class DebugProvideFoodSaveLoaderController : MonoBehaviour
{
    // 制作者 田内
    // デバッグ用処理

    [Header("デバッグ用ポケットアイテムデータ")]
    [SerializeField]
    private List<ProvideFoodSaveLoadData> m_debugProvideFoodSaveLoadeDataList = new();

    //===============================================
    //              実行処理
    //===============================================

    /// <summary>
    /// リスト上にあるデータをファイルにセーブする
    /// </summary>

    [ContextMenu("DebugSaveList")]
    private void DebugSaveList()
    {
        // デバッグ作成
        ProvideFoodSaveLoad newData = new();
        newData.ProvideFoodSaveLoadDataList = m_debugProvideFoodSaveLoadeDataList;

        ProvideFoodSaveLoader.Save(newData);
    }


    /// <summary>
    /// 全てのデータを初期化してファイルにセーブする
    /// </summary>

    [ContextMenu("DebugDeleteAllData")]
    private void DebugDeleteAllData()
    {
        ProvideFoodSaveLoader.DeleteAllData();
    }

    /// <summary>
    /// 現状のデータをファイルにセーブする
    /// ※実行中にのみ動作
    /// </summary>

    [ContextMenu("DebugSaveCurrentData")]
    private void DebugSaveCurrentData()
    {
        // 現状のデータをセーブ
        ProvideFoodSaveLoader.SaveCurrentData();
    }
}

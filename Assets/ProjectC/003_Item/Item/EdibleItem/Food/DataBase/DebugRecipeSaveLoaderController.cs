using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodInfo;

public class DebugRecipeSaveLoaderController : MonoBehaviour
{
    // 制作者 田内
    // デバッグ用処理


    [Header("デバッグ用レシピデータリスト")]
    [SerializeField]
    private List<RecipeSaveLoad> m_debugList = new();

    //===============================================
    //              実行処理
    //===============================================

    /// <summary>
    /// リスト上にあるデータをファイルにセーブする
    /// </summary>

    [ContextMenu("DebugSaveList")]
    private void DebugSaveList()
    {
        foreach (var data in m_debugList)
        {
            if (data == null) continue;
            RecipeSaveLoader.Save(data.FoodID,data);
        }
    }

    /// <summary>
    /// 全てのデータを初期化してファイルにセーブする
    /// </summary>

    [ContextMenu("DebugDeleteAllData")]
    private void DebugDeleteAllData()
    {
        RecipeSaveLoader.DeleteAllData();
    }

    /// <summary>
    /// 現状のデータをファイルにセーブする
    /// ※実行中にのみ動作
    /// </summary>

    [ContextMenu("DebugSaveCurrentData")]
    private void DebugSaveCurrentData()
    {
        // 現状のデータをセーブ
       RecipeSaveLoader.SaveCurrentData();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;


public class DebugChallengeSaveLoaderController : MonoBehaviour
{
    // 制作者 田内
    // デバッグ用処理

    [Header("デバッグ用チャレンジデータリスト")]
    [SerializeField]
    private List<ChallengeSaveLoad> m_challengeSaveLoadList = new();

    //===============================================
    //              実行処理
    //===============================================


    /// <summary>
    /// リスト上にあるデータをファイルにセーブする
    /// </summary>

    [ContextMenu("DebugSaveList")]
    private void DebugSaveList()
    {
        foreach (var data in m_challengeSaveLoadList)
        {
            ChallengeSaveLoader.Save(data.ChallengeID, data);
        }
    }

    /// <summary>
    /// 全てのデータを削除する
    /// </summary>

    [ContextMenu("DebugDeleteAllData")]
    private void DebugDeleteAllData()
    {
        ChallengeSaveLoader.DeleteAllData();
    }


    /// <summary>
    /// 現状のデータをファイルにセーブする
    /// ※実行中にのみ動作
    /// </summary>

    [ContextMenu("DebugSaveCurrentData")]
    private void DebugSaveCurrentData()
    {
        // 現状のデータをセーブ
        ChallengeSaveLoader.SaveCurrentData();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelectUseStaffInfo;

public class UseStaffDataBaseManager : BaseManager<UseStaffDataBaseManager>
{
    // 制作者 田内
    // アイテム選択データベース

    [Header("データベース")]
    [SerializeField]
    private UseStaffDataBase m_useStaffDataBase = null;

    public UseStaffDataBase UseStaffDataBase
    {
        get
        {
            UseStaffDataBase database = new();
            if (m_useStaffDataBase != null) database = m_useStaffDataBase;
            return database;
        }
    }

    //=================================================================
    //                       実行処理
    //=================================================================



    /// <summary>
    /// 引数アイテムデータを取得する
    /// </summary>
    public UseStaffData GetData(SelectUseStaffID _id)
    {
        foreach (var data in m_useStaffDataBase.UseStaffDataList)
        {
            if (data.SelectUseStaffID != _id) continue;

            return data;
        }

        Debug.LogError("シリアライズされていません : " + _id);
        return null;

    }

}

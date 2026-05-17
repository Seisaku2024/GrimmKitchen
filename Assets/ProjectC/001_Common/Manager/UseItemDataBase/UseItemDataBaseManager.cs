using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelectUseItemInfo;

public class UseItemDataBaseManager : BaseManager<UseItemDataBaseManager>
{
    // 制作者 田内
    // アイテム選択データベース

    [Header("データベース")]
    [SerializeField]
    private UseItemDataBase m_useItemDataBase = null;

    public UseItemDataBase UseItemDataBase
    {
        get
        {
            UseItemDataBase database = new();
            if (m_useItemDataBase != null) database = m_useItemDataBase;
            return database;
        }
    }

    //=================================================================
    //                       実行処理
    //=================================================================



    /// <summary>
    /// 引数アイテムデータを取得する
    /// </summary>
    public UseItemData GetData(SelectUseItemID _id)
    {
        foreach (var data in m_useItemDataBase.UseItemDataList)
        {
            if (data.SelectUseItemID != _id) continue;

            return data;
        }

        Debug.LogError("シリアライズされていません : " + _id);
        return null;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TableSetManager : BaseManager<TableSetManager>
{
    // テーブルセットを管理・操作するコントローラー
    // 制作者　田内

    // テーブルセット
    private List<TableSetData> m_tableSetList = new();

    //=============================================
    //              実行処理
    //=============================================

    public bool AddTableSetData(TableSetData _data)
    {
        m_tableSetList.Add(_data);
        return true;
    }

    public bool RemoveTableSetData(TableSetData _data)
    {
        if (m_tableSetList.Remove(_data)) return true;
        else return false;
    }


    #region メソッド説明
    /// <summary>
    /// ランダムで空席を取得する
    /// </summary>
    /// <returns> 空席のTableSetData</returns>
    #endregion
    public TableSetData GetRandomTableSet()
    {
        var list = m_tableSetList.GetShuffleRandomList();

        foreach (var table in list)
        {
            if (table == null || table.SitObject != null) continue;
            return table;
        }

        return null;
    }

}

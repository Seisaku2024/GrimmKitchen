using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FoodData.AddStatus;

public class AddStatusTypeDataBaseManager : BaseManager<AddStatusTypeDataBaseManager>
{
    // 制作者 吉田
    // 追加ステータスデータベースを管理するマネージャークラス

    [Header("データベース")]
    [SerializeField]
    private AddStatusTypeDataBase m_addStatusTypeDataBase = null;

    //=================================================
    //                  実行処理
    //=================================================

    /// <summary>
    /// AddStatusTypeのデータを取得する
    /// </summary>
    public AddStatusTypeData GetData(AddStatusType _id)
    {
        if (m_addStatusTypeDataBase == null)
        {
            Debug.LogError("データベースがシリアライズされていません");
            return null;
        }


        foreach(var data in m_addStatusTypeDataBase.AddStatusTypeDataList)
        {
            if(data.AddStatusType==_id)
            {
                return data;
            }
        }

        Debug.LogError("シリアライズされていません : " + _id.ToString());
        return null;
    }


}

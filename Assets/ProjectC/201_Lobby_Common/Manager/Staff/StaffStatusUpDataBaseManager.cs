using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;

public class StaffStatusUpDataBaseManager : BaseManager<StaffStatusUpDataBaseManager>
{
    // 制作者 田内
    // スタッフ名データベースマネージャー


    [Header("データベース")]
    [SerializeField]
    private StaffStatusUpDataBase m_staffStatusUpDataBase = null;

    public StaffStatusUpDataBase StaffStatusDataBase
    {
        get { return m_staffStatusUpDataBase; }
    }


    //=========================================
    //              実行処理
    //=========================================

    /// <summary>
    /// スタッフネームを取得
    /// </summary>
    public StaffStatusUpData GetData(StaffStatusUpID _id)
    {
        foreach (var data in m_staffStatusUpDataBase.StaffStatusUpDataList)
        {
            if (data == null) continue;
            if (data.StaffStatusUpID == _id)
            {
                return data;
            }
        }

        return null;
    }

}

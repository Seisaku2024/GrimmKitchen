using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;

public class StaffNameDataBaseManager : BaseManager<StaffNameDataBaseManager>
{
    // 制作者 田内
    // スタッフ名データベースマネージャー


    [Header("ノーマル(ランダムでも使用)")]
    [SerializeField]
    private StaffNameDataBase m_staffNameDataBase = null;

    [Header("レア")]
    [SerializeField]
    private StaffNameDataBase m_rareStaffNameDataBase = null;

    [Header("スーパーレア")]
    [SerializeField]
    private StaffNameDataBase m_superRareStaffNameDataBase = null;


    //=========================================
    //              実行処理
    //=========================================

    /// <summary>
    /// スタッフネームを取得
    /// </summary>
    public StaffNameData GetStaffNameData(StaffNameID _id)
    {
        foreach (var data in m_superRareStaffNameDataBase.staffNameDataList)
        {
            if (data == null) continue;
            if (data.StaffNameID == _id)
            {
                return data;
            }
        }
        foreach (var data in m_rareStaffNameDataBase.staffNameDataList)
        {
            if (data == null) continue;
            if (data.StaffNameID == _id)
            {
                return data;
            }
        }
        foreach (var data in m_staffNameDataBase.staffNameDataList)
        {
            if (data == null) continue;
            if (data.StaffNameID == _id)
            {
                return data;
            }
        }

        return null;
    }

    /// <summary>
    /// 引数性別の名前をランダムで取得する
    /// </summary>
    public StaffNameID GetRandomStaffNameID(StaffGenderType _type)
    {

        var list = m_staffNameDataBase.staffNameDataList.GetShuffleRandomList();

        foreach (var data in list)
        {
            if (data == null) continue;
            if (data.StaffGenderType == _type)
            {
                return data.StaffNameID;
            }
        }

        return StaffNameID.None;
    }

}

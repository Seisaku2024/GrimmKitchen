using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;

public class StaffTypeDataBaseManager : BaseManager<StaffTypeDataBaseManager>
{
    // 制作者 田内

    [Header("データベース")]
    [SerializeField]
    private StaffTypeDataBase m_staffTypeDataBase = null;

    public StaffTypeDataBase StaffTypeDataBase
    {
        get { return m_staffTypeDataBase; }
    }

    //============================================
    //              実行処理
    //============================================

    public StaffTypeData GetStaffTypeData(StaffType _type)
    {

        foreach (var data in m_staffTypeDataBase.StaffTypeDataBaseList)
        {
            // アイテムの種類が一致
            if (data.StaffType == _type)
            {
                return data;
            }
        }

        Debug.LogError(_type.ToString() + "このIDのスタッフタイプは登録されていません");
        return null;

    }

}

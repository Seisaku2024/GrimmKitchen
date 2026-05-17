using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngredientInfo;

public class IngredientTypeDataBaseManager : BaseManager<IngredientTypeDataBaseManager>
{
    // 制作者 田内
    // 食材タイプデータベースを管理するマネージャークラス

    [Header("データベース")]
    [SerializeField]
    private IngredientTypeDataBase m_ingredientTypeDataBase = null;

    //=================================================
    //                  実行処理
    //=================================================

    /// <summary>
    /// 引数IDのデータを取得する
    /// </summary>
    public IngredientTypeData GetData(IngredientTypeID _id)
    {
        if (m_ingredientTypeDataBase == null)
        {
            Debug.LogError("データベースがシリアライズされていません");
            return null;
        }


        foreach(var data in m_ingredientTypeDataBase.IngredientTypeDataList)
        {
            if(data.IngredientTypeID==_id)
            {
                return data;
            }
        }

        Debug.LogError("シリアライズされていません : " + _id.ToString());
        return null;
    }


}

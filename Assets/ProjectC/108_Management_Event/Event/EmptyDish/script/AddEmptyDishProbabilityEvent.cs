using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Arbor;

public class AddEmptyDishProbabilityEvent : AddProbabilityEvent
{
    // 制作者 上甲、田内
    // 空皿イベントを出現させる

    [Header("客情報")]
    [SerializeField]
    protected FlexibleCustomerDataVariable m_flexibleCustomerDataVariable = null;

    //========================================================
    //                     実行処理
    //========================================================

    protected override void EventInitializeProcess()
    {
        if (m_createManageentEvent == null) return;

        if (m_flexibleCustomerDataVariable?.value?.CustomerData == null)
        {
            Debug.LogError("客情報がシリアライズされていません");
            return;
        }

        var customerData = m_flexibleCustomerDataVariable.value.CustomerData;
        var tableData = customerData.TargetTableSetData;
        if (tableData == null)
        {
            Debug.LogError("テーブルセットが存在しません");
            return;
        }

        if (m_createManageentEvent is not GenerateEmptyDishEvent)
        {
            Debug.LogError("GenerateEmptyDishEventにキャストできません");
            return;
        }

        // 空皿イベントにキャスト
        var eve = m_createManageentEvent as GenerateEmptyDishEvent;

        // イベントの必要なデータをセットする
        eve.SetData(customerData.gameObject, tableData);


        //var castEmptyDishEvent = m_createManageentEvent as GenerateEmptyDishEvent;
    }



}

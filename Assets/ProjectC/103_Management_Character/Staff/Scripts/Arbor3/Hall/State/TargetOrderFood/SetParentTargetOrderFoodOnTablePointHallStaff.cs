using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using OrderFoodInfo;

[AddBehaviourMenu("Staff/HallStaff/SetParentTargetOrderFoodOnTablePointHallStaff")]
[AddComponentMenu("")]
public class SetParentTargetOrderFoodOnTablePointHallStaff : BaseStaffStateBehaviour
{
    // 制作者　田内
    // 提供料理をテーブルの子オブジェクトにセットする処理

    //==============================================================
    //                      実行処理
    //==============================================================


    // テーブルに料理を設置する
    private void Set()
    {
        // スタッフデータ
        var data = GetStaffData();
        if (data == null) return;

        // 料理データ
        var targetFoodData = data.TargetOrderFoodData;
        if (targetFoodData != null)
        {

            // 設置されたことを記憶
            targetFoodData.CurrentOrderFoodState = OrderFoodState.Set;

            // テーブルの位置にアイテムをセット
            targetFoodData.transform.SetParent(targetFoodData.TargetTableSetData.TablePoint);
            targetFoodData.transform.InitializeLocalTransform(_scale: false);

        }
    }


    // Use this for enter state
    public override void OnStateBegin()
    {
        Set();
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

using OrderFoodInfo;

[AddBehaviourMenu("Staff/HallStaff/SetParentTargetOrderFoodOnHavePointHallStaff")]
[AddComponentMenu("")]
public class SetParentTargetOrderFoodOnHavePointHallStaff : BaseStaffStateBehaviour
{
    // ターゲットの料理を持ちポイントの子オブジェクトにセット
    // 制作者　田内

    //=====================================================
    //                  実行処理
    //=====================================================

    // 料理を持つ
    private void SetTargetOrderFoodPosition()
    {
        // スタッフデータが存在するか確認
        var data = GetStaffData();
        if (data == null) return;

        // 料理データが存在するか確認
        var targetFoodData = data.TargetOrderFoodData;
        if (targetFoodData == null) return;

        // ターゲット料理ステートを移動状態に変更
        targetFoodData.CurrentOrderFoodState = OrderFoodState.Carry;

        // ターゲットの料理を子オブジェクトとしてセット
        targetFoodData.transform.SetParent(data.HavePoint.transform);
        targetFoodData.transform.InitializeLocalTransform(_scale: false);
    }

    public override void OnStateBegin()
    {
        SetTargetOrderFoodPosition();
    }

}

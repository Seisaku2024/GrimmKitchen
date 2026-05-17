using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using OrderFoodInfo;

[AddComponentMenu("")]
public class CookingTargetOrderFoodChefStaff : BaseStaffStateBehaviour
{
    // 制作者 田内
    // 料理を作成する

    [SerializeField]
    private StateLink m_successLink = null;

    //========================================
    //              実行処理
    //========================================

    // 料理を作成する
    private void CookingTargetOrderFood()
    {
        var data = GetStaffData();
        if (data == null || data.StaffStatusData == null || data.TargetOrderFoodData == null) return;


        // 料理作成カウント実行
        // 引数はスタッフの能力値

        // デフォ値に加えて追加値
        float ratio = (float)data.StaffStatusData.CookingValue / (float)StaffManager.instance.MaxStatusValue;
        ratio *= StaffManager.instance.AdditionCookingRatio;

        if (data.TargetOrderFoodData.CreatCount(ratio))
        {
            SetTransition(m_successLink);
        }

    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        CookingTargetOrderFood();
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Arbor.BehaviourTree;

using StaffInfo;

[AddComponentMenu("")]
public class DecoStaffType : BaseStaffDecorator
{
    // 制作者 田内
    // スタッフのタイプで判定するデコレーター

    [SerializeField]
    private StaffType m_staffType = StaffType.Hall;

    //=====================================
    //             実行処理
    //=====================================

    protected override bool OnConditionCheck()
    {
        var data = GetStaffData();
        if (data == null || data.StaffStatusData == null) return false;

        if (data.StaffStatusData.StaffType != m_staffType) return false;
        else return true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBeenTargetObjectStaffGangster : BaseGangsterStateBehaviour
{

    // 制作者　田内
    // ターゲットのスタッフに自身のデータをセットする

    void RemoveTargetObject()
    {
        var data = GetGangsterData();
        if (data == null) return;

        if (data.TargetStaffData == null) return;

        var obj = GetGangsterGameObject();
        if (obj == null) return;

        data.TargetStaffData.BeenTargetObjectList.Remove(obj);
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        RemoveTargetObject();
    }
}

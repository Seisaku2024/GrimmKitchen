using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;

public class AppearCustomer : AppearObject
{
    // 制作者 田内
    // 客を出現させる


    //================================
    //          実行処理
    //================================

    protected override void UpdateCreate()
    {
        // 制限時間を超えていれば
        if (ManagementGameDataManager.instance.IsTimeOver()) return;

        // 料理を提供できなければ
        if (!CounterManager.instance.IsOrder()) return;

        // 待ち列に客が入れなければ
        if (!QueueManager.instance.IsInQueue()) return;


        // オブジェクト作成
        var obj = CreateObject();
        if (obj == null) return;


        // キャラクターモーターを更新する
        var core = obj.GetComponent<MyCharacterController>();
        if (core == null)
        {
            Debug.LogError("キャラクターコントローラーが存在しません");
            return;
        }

        // 初期地点を設定
        bool isRight = Random.Range(0, 2) == 0;
        Vector3 pos;

        if (isRight)
        {
            pos = CustomerPasserbyRootManager.instance.RightSideAppearPos;
        }
        else
        {
            pos = CustomerPasserbyRootManager.instance.LeftSideAppearPos;
        }

        var data = obj.GetComponent<CustomerData>();
        if (data)
        {
            data.IsRightSide = isRight;
            data.CurrentCustomerState = CustomerStateInfo.CustomerState.Passerby;
        }

        // モーターの座標を更新
        core.SetPositionMotor(pos);

    }

}

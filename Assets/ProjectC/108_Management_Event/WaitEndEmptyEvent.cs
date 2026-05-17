/*!
 * @file WaitEndEvent.cs
 * @brief 空でイベントしては枠を確保して待機させておきたいときに使うイベント
 * 外部からSetEventEndを呼び出すことで終了させる
 * @author 上甲
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ManagementGameInfo;

public class WaitEndEmptyEvent : BaseManagementEvent
{
    public override void OnUpdate()
    {
    }

    private void Start()
    {
        ManagementEventManager.instance.AddEventList(this);
    }
}

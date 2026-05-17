/*!
 * @file PlantWateringEvent.cs
 * @brief 植物の水やりイベント警告表示用
 * 具体的な更新,イベント判定は PotentialPlantWateringEvent で行う
 * @author 上甲
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantWateringEvent : BaseManagementEvent
{

    override public void OnUpdate()
    {
    }
    public void OnWater()
    {
        SetEventEnd(ManagementGameInfo.EventSolutionType.Solution);
    }
}

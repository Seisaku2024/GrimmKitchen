using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlantWateringEventData", menuName = "ScriptableObjects/ManagementEvent/作成 水やりイベントデータ")]
[System.Serializable]
public class PlantWateringEventData : ManagementEventData
{
    public float m_SatisfactionIncreasePerSecond = 0.25f;
}

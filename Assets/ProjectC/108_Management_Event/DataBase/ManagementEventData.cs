using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagementEventInfo;

namespace ManagementEventInfo
{
    public enum ManagementEventID
    {
        Gangster = 0,
        Cleaning = 1,
        DineDash = 2,
        EmptyDish=3,
        PlantWatering = 4,
        HeavyVisitorsComing=5,
    }
}

[CreateAssetMenu(fileName = "ManagementEventData", menuName = "ScriptableObjects/ManagementEvent/作成 ManagementEventData")]
[System.Serializable]
public class ManagementEventData : ScriptableObject
{
    //制作者 田内
    // 経営イベントデータ

    //=============================================

    [Header("ID")]
    [SerializeField]
    private ManagementEventID m_eventID = ManagementEventID.Gangster;

    public ManagementEventID EventID
    {
        get { return m_eventID; }
    }

    //=============================================

    [Header("イベント名")]
    [SerializeField]
    private UnityEngine.Localization.LocalizedString m_eventName = null;

    public UnityEngine.Localization.LocalizedString EventName
    {
        get { return m_eventName; }
    }

    //=============================================

    [Header("イベント説明文")]
    [SerializeField]
    private UnityEngine.Localization.LocalizedString m_eventDescription = null;

    public UnityEngine.Localization.LocalizedString EventDescription
    {
        get { return m_eventDescription; }
    }

    //=============================================

    [Header("イベント画像")]
    [SerializeField]
    private Sprite m_eventSprite = null;

    public Sprite EventSprite
    {
        get { return m_eventSprite; }
    }

    //=============================================

    [Header("発生イベント")]
    [SerializeField]
    private BaseManagementEvent m_event = null;

    public BaseManagementEvent Event
    {
        get { return m_event; }
    }

    //=============================================

    [Header("追加満足度")]
    [SerializeField]
    [Min(0)]
    private int m_addSatisfactionValue = 10;

    public int AddSatisfactionValue
    {
        get { return m_addSatisfactionValue; }
    }

}

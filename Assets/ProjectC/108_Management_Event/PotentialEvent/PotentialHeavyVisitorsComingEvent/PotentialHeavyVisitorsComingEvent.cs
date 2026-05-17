/*!
 * @file PotentialHeavyVisitorsComingEvent.cs
 * @brief 大量来店イベントの管理担当
 * @author 上甲
 */

using ExternPropertyAttributes;
using ManagementGameInfo;
using SaintsField;
using UnityEngine;

public class PotentialHeavyVisitorsComingEvent : PotentialBaseManagementEvent
{

    //[Header("必要満足度(%)"),SerializeField]
    //private float m_satisfaction = 80.0f;

    [Header("必要満足値 追々満足度(%)依存に変更"), SerializeField]
    private int m_satisfactionValue = 80;

    [Header("最大出現数倍率"), SerializeField]
    private float m_maxAppearRate = 1.5f;

    [Header("出現間隔倍率"), SerializeField]
    private float m_delayRate = 0.5f;

    [Header("入店抽選判定オブジェクト"), SerializeField]
    PassebryColliderAttach[] m_passebryColliderAttacher;

    [Header("入店確率"), SerializeField]
    private float m_probability = 100;

    [SerializeField, Header("表示用イベントプレファブ")]
    private GameObject m_eventPrefab = null;

    private GameObject m_eventPrefabInstance = null;

    private float defProbability = 0;
    private int defMaxAppear = 0;
    private float defDelay = 0;
    private AppearCustomer m_appearCustomer = null;


    private void Start()
    {
        ManagementEventManager.instance.AddPotentialEvent(this);
        m_appearCustomer = CustomerManager.instance.GetComponent<AppearCustomer>();
        if (m_appearCustomer != null)
        {
            defMaxAppear = m_appearCustomer.MaxAppear;
            defDelay = m_appearCustomer.AppearDelay;
        }
        else
        {
            Debug.LogError("AppearCustomerが見つかりません ", CustomerManager.instance);
        }

        if (m_passebryColliderAttacher == null)
        {
            Debug.LogError("PassebryColliderAttachが見つかりません シーンから探索します。");
            m_passebryColliderAttacher = Object.FindObjectsByType<PassebryColliderAttach>(FindObjectsSortMode.InstanceID);
            defProbability = m_passebryColliderAttacher[0].Probability;
        }

    }

    override public void OnUpdate()
    {
        if (ManagementGameDataManager.instance.IsTimeOut())
        {
            if (m_eventPrefabInstance)
            {
                m_eventPrefabInstance.GetComponent<BaseManagementEvent>().SetEventEnd(EventSolutionType.Solution);
            }
            return;
        }

        if (ManagementGameDataManager.instance.CurrentSatisfactionValue >= m_satisfactionValue)
        {
            isRegistable = true;
        }

    }
    public override void Reset()
    {
        //base.Reset();
        if (m_appearCustomer != null)
        {
            m_appearCustomer.MaxAppear = defMaxAppear;
            m_appearCustomer.AppearDelay = defDelay;
        }

        if (m_passebryColliderAttacher != null)
        {
            foreach (var item in m_passebryColliderAttacher)
            {
                item.Probability = defProbability;
            }
        }
    }

    public override void OnRegisterProcess()
    {
        if (m_eventPrefabInstance)
        { return; }

        if (m_appearCustomer != null)
        {
            m_appearCustomer.MaxAppear = (int)(defMaxAppear * m_maxAppearRate);
            m_appearCustomer.AppearDelay *= m_delayRate;
        }
        if (m_passebryColliderAttacher != null)
        {
            foreach (var item in m_passebryColliderAttacher)
            {
                item.Probability = m_probability;
            }
        }

        m_eventPrefabInstance = Instantiate(m_eventPrefab);
    }

    private void OnDestroy()
    {
        Reset();
    }
}

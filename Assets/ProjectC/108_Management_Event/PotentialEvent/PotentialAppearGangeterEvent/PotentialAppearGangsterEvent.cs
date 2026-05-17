using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @brief 一定時間毎に抽選を行い、特定の条件を満たせば通常のイベントを作成する
/// </summary>
public class PotentialAppearGangsterEvent : PotentialBaseManagementEvent
{
    float m_timer = 0;
    [SerializeField] public float m_interval = 10.0f;
    [SerializeField] GameObject m_gangsterPrefab = null;

    [SerializeField] public int m_appearProbability = 30;


    private void Start()
    {
        ManagementEventManager.instance.AddPotentialEvent(this);
    }
    public override void OnStart()
    {
    }

    override public void OnUpdate()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= m_interval)
        {
            m_timer = 0;
            if (IsAppearable())
            {
                isRegistable = true;
            }
        }
    }

    public override void OnRegisterProcess()
    {
        if (ManagementEventManager.instance.IsEventMax()) return;

        var obj = Instantiate(m_gangsterPrefab);

        AppearGangsterEvent gangsterEvent = obj.GetComponent<AppearGangsterEvent>();
        if (gangsterEvent != null)
        {
            ManagementEventManager.instance.AddEventList(gangsterEvent);
        }

        Reset();
    }

    private bool IsAppearable()
    {
        if (ManagementEventManager.instance.IsEventMax())
        {
            return false;
        }

        if (Random.Range(0, 100) < m_appearProbability)
        {
            return true;
        }
        return false;
    }
}

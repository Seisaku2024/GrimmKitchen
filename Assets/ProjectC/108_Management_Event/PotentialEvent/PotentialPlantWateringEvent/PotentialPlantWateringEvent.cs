/*!
 * @file PotentialPlantWateringEvent.cs
 * @brief 常駐し残水分ゲージに応じて通常のイベントを作成する
 * @author 上甲
 */

using ExternPropertyAttributes;
using UnityEngine;

public class PotentialPlantWateringEvent : PotentialBaseManagementEvent
{
    [Header("水ゲージが完全になくなるまでの時間")]
    [SerializeField]
    private float m_wateringTime = 15.0f;

    [Header("警告が出る割合 残り〇%で出る")]
    [Range(0, 1)]
    [SerializeField]
    private float m_warningRatio = 0.3f;// 警告が出るようになる(イベントが発行される)割合 残りvalue%の時に警告が出る

    [SerializeField]
    private float m_currentWateringTime = 0.0f;// 現在の水やり時間

    [SerializeField]
    private float m_ratio = 0.0f;// 水やり時間の割合

    private PlantWateringEvent m_event; // 警告用イベントインスタンス

    [SerializeField] PlantWateringEvent m_plantWateringWarningPrefab = null;

    [SerializeField]
    private PlantWateringAssignEvent m_plantWateringAssignEvent; // 水やり当たり判定

    [SerializeField] private HPBarController hPBarController = null;

    // 1秒あたりの評価増加量
    [Header("イベント未発生時の評価増加量 (1秒あたり)")]
    private float m_SatisfactionIncreasePerSecond = 0.25f;

    // 累積評価増加量 (小数部分を保持)
    private float m_accumulatedSatisfaction = 0.0f;

    private void Start()
    {
        if(ManagementEventManager.instance == null)
        {
            return;
        }

        ManagementEventManager.instance.AddPotentialEvent(this);

        var ev = ManagementEventManager.instance.ManagementEventDataBase.GetManagementEventData(ManagementEventInfo.ManagementEventID.PlantWatering) as PlantWateringEventData;

        if (ev)
        {
            m_SatisfactionIncreasePerSecond = ev.m_SatisfactionIncreasePerSecond;
        }

    }
    public void OnWater()
    {
        m_currentWateringTime = 0.0f;
    }
    public override void OnStart()
    {
        base.OnStart();
        m_currentWateringTime = Random.Range(0.0f, (m_wateringTime * 0.75f));
    }

    public override void OnUpdate()
    {
        m_currentWateringTime += Time.deltaTime;
        m_ratio = m_currentWateringTime / m_wateringTime;

        // イベントが発生していないとき、評価を増加させる
        if (m_event == null)
        {
            IncreaseSatisfaction(Time.deltaTime);
        }

        if (m_ratio >= (1.0f - m_warningRatio) && m_event == null)
        {
            isRegistable = true;
            OnRegisterProcess(); // 自動登録
        }

        if (m_plantWateringAssignEvent && m_plantWateringAssignEvent.IsWatered)
        {
            if (m_event)
            {
                m_event.OnWater();
            }
            Reset();
        }

        if (hPBarController)
        {
            hPBarController.SetHealth(Mathf.Clamp(m_wateringTime - m_currentWateringTime, 0, m_wateringTime), m_wateringTime);
        }
    }

    // 規定値評価を増加させるメソッド (1を超えた分だけ整数で加算)
    private void IncreaseSatisfaction(float deltaTime)
    {
        m_accumulatedSatisfaction += m_SatisfactionIncreasePerSecond * deltaTime;

        int addValue = Mathf.FloorToInt(m_accumulatedSatisfaction); // 1を超えた整数部分のみ取得
        if (addValue > 0)
        {
            ManagementGameDataManager.instance.AddSatisfactionValue(addValue); // 満足度を追加
            //EvaluationManager.Instance.AddEvaluation(addValue); // 評価を追加
            m_accumulatedSatisfaction -= addValue; // 使用した分を引く
        }
    }
    public override void Reset()
    {
        base.Reset();
        m_currentWateringTime = 0.0f;
        if (m_plantWateringAssignEvent)
        {
            m_plantWateringAssignEvent.IsWatered = false;
        }
    }

    public override void OnRegisterProcess()
    {
        if (m_event)
        {
            return;
        }

        if (ManagementEventManager.instance.IsEventMax()) return;

        m_event = Instantiate(m_plantWateringWarningPrefab);
        m_event.transform.SetParent(transform, false);

        ManagementEventManager.instance.AddEventList(m_event);

        isRegistered = true;
    }
}

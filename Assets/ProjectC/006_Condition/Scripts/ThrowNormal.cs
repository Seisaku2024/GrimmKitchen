using ConditionInfo;
using System.Collections.Generic;
using UnityEngine;

// 投げた時の回復料理の処理（伊波）
public class ThrowNormal : MonoBehaviour
{
    [Header("効果時間")]
    [SerializeField] private float m_time;

    [Header("回復するまでの時間")]
    [SerializeField] private float m_confusionTime;

    [Header("HP回復割合")]
    [SerializeField, EnumIndex(typeof(ConditionInfo.ResistanceID))]
    public float[] m_maxCureRate = new float[(int)ConditionInfo.ResistanceID.ResistanceTypeNum];

    [Header("エフェクト")]
    [SerializeField] private GameObject m_effectAssetPrefab;
    private GameObject m_effect;

    class HitData
    {
        public ConditionManager m_conditionManager;
        public float m_hitTime;
    }
    private List<HitData> m_hitConditionManagers = new List<HitData>();


    private void Update()
    {
        m_time -= Time.deltaTime;
        if (m_time <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        for (int i = 0; i < m_hitConditionManagers.Count; ++i)
        {
            // Nullチェック（山本）
            if (m_hitConditionManagers[i] == null || m_hitConditionManagers[i].m_conditionManager == null)
            {
                continue;
            }
            if (m_hitConditionManagers[i].m_conditionManager.transform.root == other.transform.root)
            {
                m_hitConditionManagers[i].m_hitTime += Time.deltaTime;
                if (m_hitConditionManagers[i].m_hitTime > m_confusionTime)
                {
                    m_hitConditionManagers[i].m_hitTime = 0.0f;
                    // 回復処理
                    if (m_hitConditionManagers[i].m_conditionManager.transform.parent.TryGetComponent(out CharacterCore core))
                    {
                        var cureRate = m_maxCureRate[(int)m_hitConditionManagers[i].m_conditionManager.Resistances[(int)ConditionID.Normal]];
                        core.Heal(cureRate * core.Status.MaxHP.Value);
                    }
                }
                return;
            }
        }

        ConditionManager conditionmanager = other.GetComponentInChildren<ConditionManager>();
        if (conditionmanager == null) return;

        HitData hitData = new HitData();
        hitData.m_conditionManager = conditionmanager;
        m_hitConditionManagers.Add(hitData);
    }
}

/*!
 * @file ManagementDifficultyManager.cs
 * @brief 難易度をグラフで管理するクラス
 * @author 上甲
 */

using ExternalPropertyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ManagementDifficultyManager : BaseManager<ManagementDifficultyManager>
{

    [SerializeField, Label("入店抽選を行っているオブジェクト")]
    PassebryColliderAttach m_passebryColliderAttach = null;

    [SerializeField, Label("迷惑客生成担当オブジェクト")]
    PotentialAppearGangsterEvent m_potentialAppearGangsterEvent = null;

    public CustomerEnterProbabilityData m_customerEnterProbabilityData = null;

    public GangsterAppearProbabilityData m_gangsterAppearProbabilityData = null;


    private void Update()
    {
        float elapsedTimeRatio = ManagementGameDataManager.instance.CurrentElapsedTimeRatio;

        // 通行客入店確率の更新
        UpdateCustomerEnterProbability(elapsedTimeRatio);
        // 迷惑客生成確率の更新
        UpdateGangsterAppearProbability(elapsedTimeRatio);

    }

    private void UpdateGangsterAppearProbability(float elapsedTimeRatio)
    {
        if (m_potentialAppearGangsterEvent == null || m_gangsterAppearProbabilityData == null)
        {
            return;
        }
        m_potentialAppearGangsterEvent.m_appearProbability = (int)Mathf.Clamp(m_gangsterAppearProbabilityData.m_gangsterApeearProbability.Evaluate(elapsedTimeRatio), 0.00f, 1.00f) * 100;
        m_potentialAppearGangsterEvent.m_interval = m_gangsterAppearProbabilityData.m_gangsterAppearCoolTime.Evaluate(elapsedTimeRatio);
    }
    private void UpdateCustomerEnterProbability(float elapsedTimeRatio)
    {
        if (m_passebryColliderAttach == null || m_customerEnterProbabilityData == null)
        {
            return;
        }

        var probability = Mathf.Clamp(m_customerEnterProbabilityData.m_customerEnterProbability.Evaluate(elapsedTimeRatio), 0.00f, 1.00f);
        probability *= 100; // 0.00~1.00を0~100に変換

        m_passebryColliderAttach.Probability = probability;
    }
}

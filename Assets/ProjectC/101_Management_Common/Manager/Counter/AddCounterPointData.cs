using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCounterPointData : MonoBehaviour
{
    [Header("マネージャーにセットするリスト")]
    [SerializeField]
    List<CounterPointData> m_counterPointDataList = new();

    //===================================================
    //                  実行処理
    //===================================================

    private void Start()
    {
        if (CounterManager.instance == null) return;
        CounterManager.instance.SettingCounterPoint(m_counterPointDataList);
    }
}

/*!
 * @file AddCleaningProbabilityEvent.cs
 * @brief Arborからクリーニングイベントを発生させる
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// @brief クリーニングイベントを発生させる
/// AddProbabilityEventを継承している
/// Arbor側で紐づけられるイベントスクリプトはCleaningEventにキャストできる必要がある
/// </summary>
[AddComponentMenu("")]
public class AddCleaningProbabilityEvent : AddProbabilityEvent
{
    [SerializeField]
    private float m_randomRange = 0.3f;

    [Header("客情報")]
    [SerializeField]
    protected FlexibleCustomerDataVariable m_flexibleCustomerDataVariable = null;

    //========================================================
    //                  実行処理
    //========================================================

    /// <summary>
    /// @brief クリーニングイベント用の初期化処理
    /// キャストに失敗するとエラーを出力して終了
    /// </summary>
    protected override void EventInitializeProcess()
    {
        if (m_createManageentEvent == null) return;
        if (m_createManageentEvent is not GenerateCleaningEvent)
        {
            Debug.LogError("CleaningEventにキャストできません");
            return;
        }

        if (m_flexibleCustomerDataVariable?.value?.CustomerData == null)
        {
            Debug.LogError("客情報がシリアライズされていません");
            return;
        }
        var customerData = m_flexibleCustomerDataVariable.value.CustomerData;

        // 変換
        var castCleaningEvent = m_createManageentEvent as GenerateCleaningEvent;

        castCleaningEvent.SetPosition(customerData.gameObject.transform.position);
        castCleaningEvent.SetRandomRange(m_randomRange);
    }
}

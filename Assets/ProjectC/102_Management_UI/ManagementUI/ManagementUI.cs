using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementUI : MonoBehaviour
{
    // 制作者 田内
    // 経営UIの処理をまとめたクラス


    [Header("提供料理スロット作成")]
    [SerializeField]
    private CreateManagementProvideFoodDataSlotList m_createProvideFoodSlotList = null;

    [Header("クリア条件スロット作成")]
    [SerializeField]
    private CreateManagementClearConditionChallengeSlotList m_createClearConditionChallengeSlotList = null;

    [Header("イベントUI作成")]
    [SerializeField]
    private CreateManagementEventUI m_createManagementEventUI = null;


    //===========================================================
    //                       実行処理
    //===========================================================

    void Start()
    {
        OnStart();
    }


    void Update()
    {
        OnUpdate();
    }


    private void OnStart()
    {
        #region nullチェック
        if (m_createProvideFoodSlotList == null)
        {
            Debug.LogError("CreateManagementProvideFoodSlotListがシリアライズされていません");
            return;
        }
        if(m_createClearConditionChallengeSlotList==null)
        {
            Debug.LogError("createClearConditionChallengeSlotListがシリアライズされていません");
            return;
        }
        #endregion

        _ = m_createProvideFoodSlotList.CreateSlot();

        _ = m_createClearConditionChallengeSlotList.CreateSlot();
    }

    private void OnUpdate()
    {
        #region nullチェック
        if (m_createManagementEventUI == null)
        {
            Debug.LogError("CreateManagementEventUIがシリアライズされていません");
            return;
        }
        #endregion

        m_createManagementEventUI.OnUpdate();

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;

public partial class StaffData : MonoBehaviour
{
    // 制作者 田内

    [Header("スタッフのステータス")]
    [SerializeField]
    private StaffStatusData m_staffStatusData = new();

    public StaffStatusData StaffStatusData
    {
        get { return m_staffStatusData; }
        set { m_staffStatusData = value; }
    }

    //===========================================
    //              実行処理
    //===========================================

    private void SetStatus()
    {
        if (m_staffStatusData == null)
        {
            Destroy(gameObject);
            Debug.LogError("スタッフのステータスが存在しません");
            return;
        }

        // 配膳(スピード)をセット
        if (gameObject.TryGetComponent<CharacterCore>(out var core))
        {
            // デフォ値に加えて追加値
            float ratio = (float)m_staffStatusData.ProvideValue / (float)StaffManager.instance.MaxStatusValue;
            ratio *= StaffManager.instance.AdditionProvideRatio;

            core.Status.WalkSpeed += ratio;
            core.Status.DushSpeed += ratio;
        }
    }
}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationStaffStatusUpWindow : ConfirmationWindow
{
    // 制作者 田内
    // スタッフ強化の確認ウィンドウ

    [Header("説明文")]
    [SerializeField]
    private StaffStatusDataDescription m_beforStaffStatusDataDescription = null;


    [Header("説明文")]
    [SerializeField]
    private StaffStatusDataDescription m_afterStaffStatusDataDescription = null;

    [Header("結果表示")]
    [SerializeField]
    private AfterStaffStatusUpUI m_afterStaffStatusUpUI = null;

    [Header("更新待機時間")]
    [SerializeField]
    private int m_delayCount = 1000;

    private StaffStatusData m_beforeStaffStatusData = null;
    private StaffStatusData m_afterStaffStatusData = null;

    //======================================
    //              実行処理
    //======================================

    public void SetData(StaffStatusData _before, StaffStatusData _after)
    {
        m_beforeStaffStatusData = _before;
        m_afterStaffStatusData = _after;
    }


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_afterStaffStatusUpUI == null)
        {
            Debug.LogError(" m_afterStaffStatusUpUIがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            m_afterStaffStatusUpUI.SetStaffStatusData(m_beforeStaffStatusData, m_afterStaffStatusData);

            // 説明文更新
            Before();
            Affter().Forget();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }


    private void Before()
    {
        if (m_beforStaffStatusDataDescription == null)
        {
            Debug.LogError("BeforStaffStatusDataDescriptionがシリアライズされていません");
            return;
        }
        m_beforStaffStatusDataDescription.UpdateDescription(m_beforeStaffStatusData);
    }

    private async UniTask Affter()
    {
        if (m_afterStaffStatusDataDescription == null)
        {
            Debug.LogError("AfterStaffStatusDataDescriptionがシリアライズされていません");
            return;
        }
        await UniTask.DelayFrame(m_delayCount);
        m_afterStaffStatusDataDescription.UpdateDescription(m_afterStaffStatusData);
    }
}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeStaffStatusUpWindow : JudgeWindow
{
    // 制作者 田内
    // ジャッジウィンドウ

    [Header("説明文")]
    [SerializeField]
    private StaffStatusUpDescription m_staffStatusUpDescription = null;

    // ステータスアップデータ
    private StaffStatusUpData m_staffStatusUpData = null;

    //=====================================
    //              実行処理
    //=====================================

    public void SetData(StaffStatusUpData _data)
    {
        m_staffStatusUpData = _data;
    }

    public override async UniTask OnInitialize()
    {
        if (m_staffStatusUpDescription == null)
        {
            Debug.LogError("StaffStatusUpDescriptionがシリアライズされていません");
            return;
        }

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            m_staffStatusUpDescription.UpdateDescription(m_staffStatusUpData);

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}

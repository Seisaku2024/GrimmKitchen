using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(TrialSessionController))]
public class SceneTransitionToInitializeTrialSessionButtonData : SceneTransitionButtonData
{
    // 制作者 田内
    // タイトルに戻りつつ初期化を行う

    // 試遊会用コントローラー
    private TrialSessionController m_trialSessionController = null;

    //=============================================
    //                  実行処理
    //=============================================

    protected override void Start()
    {
        // コンポーネントをセット
        gameObject.TryGetComponent(out m_trialSessionController);
        base.Start();
    }

    public override async UniTask OnPressUpdate()
    {
        try
        {
            m_trialSessionController.InitializeTrialSession();
            await base.OnPressUpdate();

            await UniTask.CompletedTask;
        }
        catch(System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagementStateUpdateInfo;

using Cysharp.Threading.Tasks;


public class ManagementStateUpdate_Update : BaseManagementStateUpdate
{

    // 制作者 田内
    // 経営終了処理



    //====================================================
    //                   実行処理
    //====================================================


    public override async UniTask OnInitialize()
    {
        // 開始
        Time.timeScale = 1.0f;

        await UniTask.CompletedTask;
    }

    public override async UniTask OnUpdate()
    {
        // ゲームが終了したら
        if (ManagementGameDataManager.instance.IsGameEnd())
        {
            SetEnd(m_nextManagementState);
        }

#if UNITY_EDITOR
        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, "Debug"))
        {
            SetEnd(ManagementState.End);
        }
#endif

        await UniTask.CompletedTask;
    }


    override public async UniTask OnExit()
    {
        await UniTask.CompletedTask;
    }

}

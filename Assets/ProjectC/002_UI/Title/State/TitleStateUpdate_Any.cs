using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

public class TitleStateUpdate_Any : BaseTitleStateUpdate
{
    // 制作者 田内

    //==========================================
    //             実行処理
    //==========================================

    public override async UniTask OnUpdate()
    {
        // 何かキーを押したら
        if (PlayerInputManager.instance.IsPressCurrentAnyKey())
        {
            SetEnd(m_nextTitleState);
        }

        await UniTask.CompletedTask;
    }

    private void OnAnyInput(InputControl control)
    {
        Debug.Log($"入力検知: {control.device.displayName} の {control.name}");
    }

}

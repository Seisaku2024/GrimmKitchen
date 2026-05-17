using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleStateUpdate_Start : BaseTitleStateUpdate
{
    // 制作者 田内

    //==========================================
    //             実行処理
    //==========================================

    public override async UniTask OnUpdate()
    {
        SetEnd(m_nextTitleState);

        await UniTask.CompletedTask;
    }
}

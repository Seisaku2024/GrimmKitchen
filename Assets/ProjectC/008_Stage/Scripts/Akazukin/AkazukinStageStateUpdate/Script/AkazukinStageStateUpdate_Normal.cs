using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkazukinStageStateUpdate_Normal : BaseAkazukinStageStateUpdate
{
    // 赤ずきんステージ（ノーマル状態）
    override public async UniTask OnUpdate()
    {
        await UniTask.CompletedTask;
    }
}

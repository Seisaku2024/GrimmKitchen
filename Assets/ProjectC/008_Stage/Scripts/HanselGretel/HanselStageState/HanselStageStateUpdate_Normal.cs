using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselStageStateUpdate_Normal : BaseHanselStageStateUpdate
{
   /// <summary>
   /// ノーマル状態（特に何の処理もしません）
   /// 山本
   /// </summary>
   /// <returns></returns>
   
    override public async UniTask OnUpdate()
    {
        await UniTask.CompletedTask;
    }
}

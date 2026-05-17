using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using UnityEngine.InputSystem;
using LobbyStateInfo;


public class LobbyStateUpdate_Normal : BaseLobbyStateUpdate
{
    // 通常の動作
    // 制作者　田内

    //====================================================
    //                   実行処理
    //====================================================


    override public async UniTask OnUpdate()
    {
        await UniTask.CompletedTask;
    }


   

}

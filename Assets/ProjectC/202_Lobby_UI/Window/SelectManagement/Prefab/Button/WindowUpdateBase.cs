using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Windowで更新する際の基底クラス（吉田）
/// </summary>
public class WindowUpdateBase : MonoBehaviour
{
    virtual public void OnInitialize()
    {

    }

    virtual public void OnUpdate()
    {

    }
    
    virtual public async UniTask OnUpdateTask()
    {
        await UniTask.CompletedTask;
    }
    
    virtual public void OnLateUpdate()
    {
    }
}

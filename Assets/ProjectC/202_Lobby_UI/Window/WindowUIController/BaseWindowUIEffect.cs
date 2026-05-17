using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BaseWindowUIEffect : MonoBehaviour
{
    // 制作者 田内
    // UIエフェクト

    //=====================================================
    //                       実行処理
    //=====================================================


    /// <summary>
    /// エフェクト再生
    /// </summary>
    virtual public async UniTask PlayEffect()
    {

        await UniTask.CompletedTask;
    }


    /// <summary>
    /// エフェクト非再生
    /// </summary>
    virtual public async UniTask UnPlayEffect()
    {

        await UniTask.CompletedTask;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TitleExitController : MonoBehaviour
{
    // タイトル 終了

    //========================================
    //              実行処理
    //========================================

    public async UniTask OnUpdate()
    {


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

        await UniTask.CompletedTask;
    }
}

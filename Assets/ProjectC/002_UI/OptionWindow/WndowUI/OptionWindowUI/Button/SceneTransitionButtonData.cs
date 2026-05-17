using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(SceneTransitionManager))]
public class SceneTransitionButtonData : ButtonData
{
    // 制作者 田内
    // タイトルに戻るボタン

    // シーン切り替え
    private SceneTransitionManager m_sceneTransitionManager = null;

    //=============================================
    //                  実行処理
    //=============================================

    virtual protected void Start()
    {
        // コンポーネントをセット
        gameObject.TryGetComponent(out m_sceneTransitionManager);
    }


    public override async UniTask OnPressUpdate()
    {
        try
        {
            await m_sceneTransitionManager.SceneChange();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}

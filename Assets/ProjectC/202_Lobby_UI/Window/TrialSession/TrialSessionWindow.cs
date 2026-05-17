using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class TrialSessionWindow : ConfirmationWindow
{
    // 体験会用ウィンドウ
    // 制作者　田内

    [Header("シーン変更")]
    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;

    protected override async UniTask OnUpdateClose()
    {
        #region nullチェック
        if (m_sceneTransitionManager==null)
        {
            Debug.LogError("SceneTransitionManagerがシリアライズされていません");
            return;
        }
        #endregion

        _ = m_sceneTransitionManager.SceneChange();

        await UniTask.CompletedTask;
    }


}

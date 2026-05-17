using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipMovieWindow : ConfirmationWindow
{

    [SerializeField]
    private WindowUpdateBase m_showAnyPress = null;

    protected override async UniTask OnUpdateConfirmation()
    {
        if (m_showAnyPress != null)
        {
            m_showAnyPress.OnUpdate();
        }
    }

    protected override async UniTask OnUpdateClose()
    {
        #region nullチェック
        if (CutSceneManager.instance == null)
        {
            Debug.LogError("CutSceneManagerがステージに存在しません");
            return;
        }
        #endregion


        // Timeline飛ばす
        double sec = CutSceneManager.instance.PlayableDirector.duration;
        CutSceneManager.instance.PlayableDirector.time = sec;

        Animator animator = null;

        if (IMetaAI<CharacterCore>.Instance == null)
        {
            return;
        }

        // プレイヤーのアニメーション取得
        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (core.GroupNo == CharacterGroupNumber.player)
            {
                animator = core.m_animator;
                break;
            }
        }

        if (animator)
        {
            AnimationEx.SkipAnimation(animator);
            animator.SetBool("IsSkipAnim", true);
        }


        await UniTask.CompletedTask;
    }
}

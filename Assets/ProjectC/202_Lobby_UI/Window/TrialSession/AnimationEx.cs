using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationEx
{
    //再生中のアニメーションを終わりまでスキップする
    public static void SkipAnimation(Animator animator, int layer = 0)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
        animator.Play(stateInfo.fullPathHash, layer, 1); //再生時間が0～1の範囲なので強制的に１にする
    }
}

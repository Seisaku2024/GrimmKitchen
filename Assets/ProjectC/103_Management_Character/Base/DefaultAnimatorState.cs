using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("Animator/DefaultAnimatorTrigger")]
public class DefaultAnimatorState : StateBehaviour
{
    // 制作者　田内
    // animatorをデフォルトの状態にする

    [Header("デフォルトにするアニメーター")]
    [SerializeField, SlotType(typeof(Animator))]
    private FlexibleComponent m_animator = new FlexibleComponent(FlexibleHierarchyType.Self);

    //==============================================================

    private Animator GetAnimator()
    {
        if (m_animator == null) return null;

        if (m_animator.value is Animator)
        {
            var animator = m_animator.value as Animator;
            return animator;
        }
        else
        {
            return null;
        }

    }

    private void Default()
    {
        var animator = GetAnimator();
        if (animator != null)
        {
            // ステートをデフォルトにする
            animator.Rebind();

            // アニメーターのすべてのパラメータを取得
            foreach (var parameter in animator.parameters)
            {
                // トリガータイプのパラメータをリセット
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(parameter.name);
                }
            }


            for (int layer = 0; layer < animator.layerCount; layer++)
            {
                // ステートを取得
                var currentState = animator.GetCurrentAnimatorStateInfo(layer);

                // ステートを最初に設定
                animator.Play(currentState.shortNameHash, layer, 0f);
            }

            // 即座に更新
            animator.Update(0);
        }
    }
    //=========================================================================

    // Use this for enter state
    public override void OnStateBegin()
    {
        Default();
    }
}

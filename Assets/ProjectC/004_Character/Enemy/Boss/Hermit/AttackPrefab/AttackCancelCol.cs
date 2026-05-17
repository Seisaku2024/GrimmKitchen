using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーからの攻撃がこのコンポーネントに当たったら、プレイヤーをノックバックさせる

public class AttackCancelCol : MonoBehaviour
{
    Animator animator;

    private void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!animator)
        {
            CharacterMeta meta = IMetaAI<CharacterCore>.Instance as CharacterMeta;
            if (meta == null)
            {
                Debug.LogError("CharacterMetaが見つかりません");
                return;
            }

            animator = meta.Player.m_animator;
        }

        if(collision.transform.TryGetComponent(out AttackApplicant atk))
        {
            if (collision.transform.root == animator.transform.root)
            {
                animator.SetTrigger("KnockBack");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!animator)
        {
            CharacterMeta meta = IMetaAI<CharacterCore>.Instance as CharacterMeta;
            if (meta == null)
            {
                Debug.LogError("CharacterMetaが見つかりません");
                return;
            }

            animator = meta.Player.m_animator;
        }

        if (other.transform.TryGetComponent(out AttackApplicant atk))
        {
            if (other.transform.root == animator.transform.root)
            {
                animator.SetTrigger("KnockBack");
            }
        }
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private string m_onDestroySEName;
    [SerializeField] private GameObject m_brokenEffect;

    [SerializeField] private Rigidbody m_rigidbody;

    private void Start()
    {
        if(!m_rigidbody)
        {
            Debug.LogError("RigidBodyがセットされていません", gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.StartsWith("BossHermitBubble"))
        {
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(transform.root.gameObject);
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(transform.root.gameObject);
            return;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("NoHitEnemySkillCharacter"))
        {
            Destroy(transform.root.gameObject);
            return;
        }

        m_rigidbody.linearVelocity = Vector3.zero;
    }

    private void OnDestroy()
    {
        if (!string.IsNullOrEmpty(m_onDestroySEName))
        {
            // 効果音
            SoundManager.Instance.Start3DPlayback(m_onDestroySEName, transform.position);
        }
        if (m_brokenEffect)
        {
            Instantiate(m_brokenEffect, transform.position, Quaternion.identity);
        }
    }
}

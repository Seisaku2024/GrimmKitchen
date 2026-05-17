using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class RotatePlayer : StateBehaviour
{
    private CharacterCore m_playerCore = null;

    // Use this for initialization
    void Start()
    {

    }

    // Use this for awake state
    public override void OnStateAwake()
    {
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (core.GroupNo == CharacterGroupNumber.player)
            {
                m_playerCore = core;
            }
        }

        if (m_playerCore == null) return;

        CharacterCore chrCore = null;

        if (transform.root.TryGetComponent(out CharacterCore characterCore))
        {
            chrCore = characterCore;
            var dir = Vector3.forward;
            var v = Vector3.RotateTowards(chrCore.transform.forward, m_playerCore.transform.forward, 10000.0f, 0);
            chrCore.SetRotateToTarget(v, false);
        }

    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        if (m_playerCore == null) return;

        CharacterCore chrCore = null;

        if (transform.root.TryGetComponent(out CharacterCore characterCore))
        {
            chrCore = characterCore;
            var dir = m_playerCore.transform.position - characterCore.transform.position;
            dir.Normalize();
            chrCore.SetRotateToTarget(dir, false);
        }
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}

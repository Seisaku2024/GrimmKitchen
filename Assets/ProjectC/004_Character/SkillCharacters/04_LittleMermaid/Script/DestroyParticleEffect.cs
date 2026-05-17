using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleEffect : MonoBehaviour
{
    // 指定したパーティクルが生成ストップしたらパーティクルをDeleteする
    [SerializeField]
    private ParticleSystem m_particleSystem = null;

    private void Start()
    {
        if (m_particleSystem == null) return;

        var main = m_particleSystem.main;

        main.stopAction = ParticleSystemStopAction.Callback;

    }

    private void OnParticleSystemStopped()
    {
        Destroy(gameObject.transform.root.gameObject);
    }

}

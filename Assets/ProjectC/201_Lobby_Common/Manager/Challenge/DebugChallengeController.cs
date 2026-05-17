using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;

[DefaultExecutionOrder(10000)]
public class DebugChallengeController : MonoBehaviour
{
    // 制作者 田内

    // 初期値セット用
    [Header("デバッグ用")]
    [SerializeField]
    bool m_isDebug = false;
    [SerializeField]
    ChallengeID m_debugChallengeID = ChallengeID.None;

    // Start is called before the first frame update
    private void Awake()
    {
        if (m_isDebug)
        {
            ChallengeManager.instance.SetChallengeID(m_debugChallengeID);
        }
    }

}

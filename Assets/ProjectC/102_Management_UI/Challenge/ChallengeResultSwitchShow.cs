using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 制作者　吉田
/// チャレンジの結果によって、アクティブ状態を切り替える
/// 
/// </summary>
public class ChallengeResultSwitchShow : WindowUpdateBase
{
    enum ChallengeResult
    {
        success,
        failure
    }

    [Header("表示するタイミング")]
    [SerializeField] 
    private ChallengeResult m_show = ChallengeResult.success;

    [Header("表示するオブジェクト")]
    [Header("※Noneの場合このオブジェクト")]
    [SerializeField]
    private GameObject m_showObject = null;

    public override void OnInitialize()
    {
        base.OnInitialize();
        if (m_showObject == null)
        {
            m_showObject = gameObject;
        }

        // クリアしていれば
        if (ManagementGameDataManager.instance.IsClearChallenge())
        {
            m_showObject.SetActive(m_show == ChallengeResult.success);
        }
        // していなければ
        else
        {
            m_showObject.SetActive(m_show == ChallengeResult.failure);
        }
    }
}

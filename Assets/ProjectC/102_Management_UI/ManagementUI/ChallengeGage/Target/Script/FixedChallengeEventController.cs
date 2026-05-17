using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 制作者 吉田 (追加 田内)
/// 
/// ターゲット（満足ゲージの目標地点・イベント地点）のコントローラー
/// ぬかされたかどうかを判定する
/// 
/// 使いにくかったら、変更してください。
/// </summary>
[RequireComponent(typeof(BarSignController))]
public class FixedChallengeEventController : MonoBehaviour
{

    //=================================================
    // 位置を設定したり、取得したりするコンポーネント
    private BarSignController m_barSignController = null;

    //=================================
    // ターゲットのチャレンジイベント
    private ChallengeData.FixedChallengeEvent m_targetFixedChallengeEvent = null;


    //===========================
    // 一度満足値が超えているかどうか
    private bool m_isOver = false;
    public bool IsOver { get => m_isOver; }


    [Header("Over時に表示するオブジェクト")]
    [SerializeField]
    private GameObject m_noOverObject = null;

    [Header("NoOver時に表示するオブジェクト")]
    [SerializeField]
    private GameObject m_overObject = null;


    //================================================
    //                  実行処理
    //================================================

    public void SetTargetFixedChallengeEvent(ChallengeData.FixedChallengeEvent _fixedChallengeEvent)
    {
        m_targetFixedChallengeEvent = _fixedChallengeEvent;
    }


    /// <summary>
    /// BarSignControllerにデータをセットする
    /// </summary>
    public void SetBarSignControllerData(float _maxRate, float _currentRate)
    {
        if (m_barSignController == null)
        {
            if (TryGetComponent(out BarSignController barSignController) == false)
            {
                Debug.LogError("m_barSignController is null");
                return;
            }
            m_barSignController = barSignController;
        }

        m_barSignController.SetPos(_maxRate, _currentRate);
    }


    /// <summary>
    /// 現在の値が目標値を超えているか確認する
    /// </summary>
    public bool CheckOver(float _currentRate)
    {
        if (m_barSignController == null)
        {
            if (TryGetComponent(out BarSignController barSignController) == false)
            {
                Debug.LogError("m_barSignController is null");
                return false;
            }
            m_barSignController = barSignController;
        }

        if (m_barSignController.Rate <= _currentRate)
        {
            OnOver();
            return true;
        }
        return false;
    }

    // 目標値を超えた状態
    private void OnOver()
    {
        // 初回クリア時のみイベントを発生
        if (m_isOver == false && m_targetFixedChallengeEvent != null)
        {
            // 作成・追加
            var eve = Instantiate(m_targetFixedChallengeEvent.ManagementEvent);
            ManagementEventManager.instance.AddEventList(eve);
        }


        m_isOver = true;
        if (m_overObject != null)
        {
            m_overObject.SetActive(true);
        }
        if (m_noOverObject != null)
        {
            m_noOverObject.SetActive(false);
        }
    }


    // 目標値を超えていない状態
    private void OnNoOver()
    {
        m_isOver = false;
        if (m_overObject != null)
        {
            m_overObject.SetActive(false);
        }
        if (m_noOverObject != null)
        {
            m_noOverObject.SetActive(true);
        }
    }

    private void Start()
    {
        OnNoOver();
    }
}

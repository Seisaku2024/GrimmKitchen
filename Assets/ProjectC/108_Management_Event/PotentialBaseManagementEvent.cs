/*!
 * @file PotentialBaseManagementEvent.cs
 * @brief 常駐し特定の条件を満たせば通常のイベントを作成する
 * @author 上甲
 */
using UnityEngine;

/// <summary>
/// @brief 常駐し特定の条件を満たせば通常のイベントを作成する
/// @details イベントの追加可否はisRegistableで判定
///　イベントの追加はOnRegisterProcessを定義する
///　監視後再度追加可能にする場合はResetを定義/呼び出す
/// </summary>
public class PotentialBaseManagementEvent : BaseManagementEvent
{
    //! @brief イベント合流時自身を破棄するか
    [Header("イベント合流時自身を破棄するか")]
    [SerializeField]
    private bool isRegistedBreak = false;

    //! @brief イベント追加可能か 何らかの判定処理で変更可
    protected bool isRegistable = false;

    public virtual void OnRegisterProcess()
    {
        Debug.LogError("OnRegisterProcess がオーバーライドされていません");
        isRegistered = true;
    }

    public virtual void Reset()
    {
        isRegistered = false;
        isRegistable = false;
    }


    // 以下管理用=================================================

    //! @brief イベント追加済みか 重複追加を防ぐ用
    protected bool isRegistered = false;

    public bool IsCanAddEvent
    {
        // 追加可能　かつ　未追加
        get { return isRegistable && (!isRegistered); }
    }
    public bool IsAddBreak
    {
        get { return isRegistedBreak; }
    }

    public bool Registed
    {
        get { return isRegistered; }
        set { isRegistered = value; }
    }

}

using HanselStageInfo;
using UnityEngine;

public class BaseHanselStageStateUpdate : BaseGameStateUpdate
{
    //基底クラス（山本）


    [Header("自身のステート状態")]
    [SerializeField]
    protected GameStageState m_hanselStageState = GameStageState.Normal;

    [Header("終了後に進むステート")]
    [SerializeField]
    protected GameStageState m_nextHanselStageState = GameStageState.Normal;


    public override int GetState()
    {
        // ステートをセット
        return (int)m_hanselStageState;
    }

    public override int GetNextState()
    {
        return (int)m_nextHanselStageState;
    }


    /// <summary>
    /// ステート処理を終了する
    /// </summary>
    virtual protected void SetEnd(GameStageState _state)
    {
        m_nextHanselStageState = _state;
        m_isEnd = true;
    }
}

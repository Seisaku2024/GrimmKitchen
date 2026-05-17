using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TitleStateInfo;

public class BaseTitleStateUpdate : BaseGameStateUpdate
{
    // 制作者 田内
    // 基底クラス

    [Header("自身のステート")]
    [SerializeField]
    protected TitleState m_titleState = TitleState.None;


    [Header("終了後に進むステート")]
    [SerializeField]
    protected TitleState m_nextTitleState = TitleState.None;


    public override int GetState()
    {
        // ステートをセット
        return (int)m_titleState;
    }


    public override int GetNextState()
    {
        return (int)m_nextTitleState;
    }

    /// <summary>
    /// ステート処理を終了する
    /// </summary>
    virtual protected void SetEnd(TitleState _state)
    {
        m_nextTitleState = _state;
        m_isEnd = true;
    }

}

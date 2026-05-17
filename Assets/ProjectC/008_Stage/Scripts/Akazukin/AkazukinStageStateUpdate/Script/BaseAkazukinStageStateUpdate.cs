using AkazukinStageInfo;
using HanselStageInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAkazukinStageStateUpdate : BaseGameStateUpdate
{
    //赤ずきんステージ基底クラス（山本）


    [Header("自身のステート状態")]
    [SerializeField]
    protected AkazukinStageState m_akazukinStageState = AkazukinStageState.Normal;

    [Header("終了後に進むステート")]
    [SerializeField]
    protected AkazukinStageState m_nextAkazukinStageState = AkazukinStageState.Normal;


    public override int GetState()
    {
        var manager = AkazukinStageUpdateManager.instance;

        // ステートをセット
        return (int)m_akazukinStageState;
    }

    public override int GetNextState()
    {
        return (int)m_nextAkazukinStageState;
    }


    /// <summary>
    /// ステート処理を終了する
    /// </summary>
    virtual protected void SetEnd(AkazukinStageState _state)
    {
        m_nextAkazukinStageState = _state;
        m_isEnd = true;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using TitleStateInfo;

namespace TitleStateInfo
{
    public enum TitleState
    {
        None = 0,
        Start = 1,
        Any = 2,
        Menu = 3,
    }
}


public class TitleStateUpdateManager : BaseGameStateUpdateController<TitleStateUpdateManager>
{
    // 経営パートの進行を管理するマネージャークラス
    // 制作者　田内


    [Header("デバッグ用")]
    [SerializeField]
    private TitleState m_titleState = TitleState.None;


    [System.Serializable]
    private class InitilizeState
    {
        [Header("シーン名")]
        [SerializeField]
        private string m_sceneName = "";

        public string SceneName
        {
            get { return m_sceneName; }
        }

        [Header("変更ステート")]
        [SerializeField]
        private TitleState m_titleState = TitleState.None;

        public TitleState TitleState
        {
            get { return m_titleState; }
        }

    }

    [Header("初期ステートリスト")]
    [SerializeField]
    private List<InitilizeState> m_initilizeStateList = new();

    [Header("初期ステート")]
    [SerializeField]
    private TitleState m_initilizeState = TitleState.None;

    //=================================================================
    //                      実行処理
    //=================================================================


    // 初期ステートをセット
    override protected void SetInitializeState()
    {
        // どのシーンからきたか
        var beforeSceneName = SceneNameManager.instance.BeforeSceneName;
        if (beforeSceneName == "None")
        {
            // 初期の値をセット
            m_currentStateDevice.Value = (int)m_titleState;
        }
        else
        {
            // 一致するシーン名を取得
            foreach (var state in m_initilizeStateList)
            {
                if (beforeSceneName == state.SceneName)
                {
                    // 更新
                    m_currentStateDevice.Value = (int)state.TitleState;
                    return;
                }
            }

            // 何もなければ
            m_currentStateDevice.Value = (int)m_initilizeState;
        }

    }

}

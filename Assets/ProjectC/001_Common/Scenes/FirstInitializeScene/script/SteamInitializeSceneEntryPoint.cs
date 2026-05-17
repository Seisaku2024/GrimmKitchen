using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Steamworks;

public class SteamInitializeSceneEntryPoint : EntryPointBase
{
    [SerializeField]
    SceneTransitionManager m_SceneTransitionManager;

    // 遷移演出の終了後に呼び出されます。
    protected override async UniTask OnEnter(ISceneDataReader reader, CancellationToken cancellationToken)
    {
        if (SteamAPI.IsSteamRunning())
        {
            m_SceneTransitionManager.SceneChange();
        }
    }

}

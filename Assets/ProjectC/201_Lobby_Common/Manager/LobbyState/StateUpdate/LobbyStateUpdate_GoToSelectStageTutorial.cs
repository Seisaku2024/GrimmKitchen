using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyStateUpdate_GoToSelectStageTutorial : BaseLobbyStateUpdate
{
    public override async UniTask OnInitialize()
    {
        await base.OnInitialize();
    }

    public override UniTask OnUpdate()
    {
        //指定したコライダーのSetCheckLobbyInputActionを確認
        var colTrans = LobbyStateUpdateManager.instance.GetProgressColliderTransform(m_lobbyState);

        if(colTrans.gameObject.TryGetComponent(out SetCheckLobbyStateInputAction setCheckLobbyStateInputAction))
        {
            if(setCheckLobbyStateInputAction.bSearchObjFlg)
            {
                SetEnd(m_nextLobbyState);
            }
        }

        return base.OnUpdate();
    }

}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkazukinStageStateUpdate_GoToNextStoryAndChangeCharacterPosition : AkazukinStageStateUpdate_GoToNextStory
{
    public override UniTask OnInitialize()
    {
        _ = base.OnInitialize();

        var changeCharacterInformation = AkazukinStageUpdateManager.instance.GetChangeCharacterInformation(m_akazukinStageState,m_npcCore.NPCParameters.SpeakerType);

        if (changeCharacterInformation!=null)
            m_npcCore.CharaCtrl.SetPositionMotor(changeCharacterInformation.changeTransform.position);


        return UniTask.CompletedTask;
    }

    public override UniTask OnUpdate()
    {
        return base.OnUpdate();
    }

}

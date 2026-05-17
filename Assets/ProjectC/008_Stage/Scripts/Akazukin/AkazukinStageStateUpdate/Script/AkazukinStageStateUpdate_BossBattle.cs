using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AkazukinStageStateUpdate_BossBattle : BaseAkazukinStageStateUpdate
{
    private Transform m_BossObj;

    public override UniTask OnInitialize()
    {
        if (AkazukinStageUpdateManager.instance)
        {
            m_BossObj = AkazukinStageUpdateManager.instance.BossChara;
        }



        Observable.EveryUpdate()
       .Where(_ => m_BossObj == null)
       .Take(1)
       .Subscribe(_ =>
       {
           SetEnd(m_nextAkazukinStageState);
       });


        return base.OnInitialize();
    }


}

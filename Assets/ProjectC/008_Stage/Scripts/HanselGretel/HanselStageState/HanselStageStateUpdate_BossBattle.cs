using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HanselStageStateUpdate_BossBattle : BaseHanselStageStateUpdate
{
    private List<CharacterCore> m_enemyCoreList = new List<CharacterCore>();

    public override UniTask OnInitialize()
    {
        m_enemyCoreList = HanselStageStateUpdateManager.instance.GetEnemyCharacterCoreList(m_hanselStageState);

        if (m_enemyCoreList == null)
        {
            Debug.LogError("エネミーリストが登録されていません");
        }

        return base.OnInitialize();

    }

    override public async UniTask OnUpdate()
    {
        await UniTask.CompletedTask;


        foreach (var core in m_enemyCoreList)
        {
            if (core == null) continue;
        }

        //null削除
        m_enemyCoreList.RemoveAll(item => item == null);


        // リスト空なら次のステートへと移行
        if (m_enemyCoreList.Count==0)
        {
            SetEnd(m_nextHanselStageState);
        }

    }
}

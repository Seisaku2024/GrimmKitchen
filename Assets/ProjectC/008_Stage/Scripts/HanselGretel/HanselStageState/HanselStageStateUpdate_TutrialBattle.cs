using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselStageStateUpdate_TutrialBattle : BaseHanselStageStateUpdate
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
        if (m_enemyCoreList.Count == 0)
        {
            // コライダーのアクティブを変更
            var colList = HanselStageStateUpdateManager.instance.GetBarrierColliderList(m_hanselStageState);
            foreach (var col in colList)
            {
                col.gameObject.SetActive(false);
            }

            SetEnd(m_nextHanselStageState);
        }

    }

}

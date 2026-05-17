/**
* @file JudgeStun.cs
* @brief 当たり判定に基づくスタン判定
*/
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using ConditionInfo;

/**
* @brief 当たり判定に基づくスタン判定　伊波
* @details 自分がスタンになるか判断する。主に突進系
*/
public class JudgeStun : MonoBehaviour
{
    private OwnerInfoTag m_ownerInfoTag;

    void Start()
    {
        TryGetComponent(out m_ownerInfoTag);
        
        this.OnTriggerEnterAsObservable()
         .Where(_ => enabled/* && _.gameObject.isStatic*/)
         .Subscribe(collider =>
         {
             if (collider.isTrigger)
             {
                 return;
             }

             if (collider.gameObject.layer != LayerMask.NameToLayer("Ignore Camera Collider")
              && collider.gameObject.layer != LayerMask.NameToLayer("Default")) return;

             if (collider.gameObject.layer == LayerMask.NameToLayer("Player")) return;
             if (collider.gameObject.layer == LayerMask.NameToLayer("SkillCharacter")) return;
             if (collider.gameObject.layer == LayerMask.NameToLayer("NoHitEnemySkillCharacter")) return;


             var conditionData = ConditionDataBaseManager.instance.GetConditionData(ConditionID.Stun);
             if (conditionData == null) return;

             if (conditionData.ConditionPrefab == null) return;
             Transform managerObj = transform.root.Find("ConditionManager");
             if (!managerObj.TryGetComponent(out ConditionManager manager)) return;

             manager.AddCondition(conditionData.ConditionPrefab, false);

             Destroy(gameObject);

             return;
         }
         ).AddTo(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimatorEventCreateObjectRotateTargetVec : AnimatorEvents.EventNodeBase
{
    //　ターゲットに向けてオブジェクトを生成する（山本）

    [Header("発生させるプレハブ")]
    [SerializeField] GameObject m_obj;
    public override void OnEvent(Animator animator)
    {
        base.OnEvent(animator);

        if (m_obj == null) return;
        var charaCore = animator.transform.GetComponentInParent<CharacterCore>();
        if (charaCore == null) return;

        var targetVec = charaCore.PlayerSkillsParameters.TargetPosition - charaCore.PlayerSkillsParameters.StartSkillPos;

        Quaternion targetQua = Quaternion.LookRotation(targetVec, charaCore.transform.up);

        //　新しいオブジェクトをシーン内に配置する（親子関係だと親の回転に影響してしまう）
        //　登録したスキル開始地点から発射
        var newObj = GameObject.Instantiate(m_obj, charaCore.PlayerSkillsParameters.StartSkillPos, targetQua);

    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アニメーターイベントで任意のタイミングにプレイヤーの登録しているオブジェクトをActiveにする
// 指定した名前からアクセスできるようにする（山本）

[System.Serializable]
public class AnimatorEventActiveRegisterItem : AnimatorEvents.EventNodeBase
{

    [Header("ステート切り替え時に非アクティブにするかどうか？")]
    [SerializeField] private bool m_doNonActiveOnExitState = true;
    [Header("登録されたオブジェクトの名前")]
    [SerializeField] private string m_stringName;

    private GameObject m_registerObject;

    // 時間が来たときに実行
    public override void OnEvent(Animator animator)
    {

        if (animator.transform.parent.TryGetComponent(out GameObjectRegistry gameObjectRegistry))
        {
            m_registerObject = gameObjectRegistry.GetObjectByKey(m_stringName);
            m_registerObject.SetActive(true);
        }

    }
    public override void OnExit(Animator animator)
    {
        base.OnExit(animator);
        if (m_doNonActiveOnExitState)
        {
            if (m_registerObject == null) return;
            m_registerObject.SetActive(false);
        }
    }
}
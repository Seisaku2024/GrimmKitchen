using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using UnityEngine.UI;


public class ManagementStateUpdate_Start : BaseManagementStateUpdate
{

    // 制作者 田内
    // 経営開始処理

    [Header("ターゲットのImages")]
    [SerializeField]
    private Image m_targetImage = null;

    [Header("プレイヤー初期位置")]
    [SerializeField]
    private Vector3 m_startPlayerPos = Vector3.zero;

    //====================================================
    //                   実行処理
    //====================================================


    public override async UniTask OnInitialize()
    {

        // プレイヤーを初期位置にセット
        if (IMetaAI<CharacterCore>.Instance != null)
        {

            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (core.GroupNo == CharacterGroupNumber.player)
                {
                    core.CharaCtrl.SetPositionMotor(m_startPlayerPos);
                    break;
                  
                }
            }

        }


        // 停止
        Time.timeScale = 0.0f;

        await UniTask.CompletedTask;
    }

    public override async UniTask OnUpdate()
    {
        // ターゲットのImageが無くなれば
        if (m_targetImage == null)
        {
            SetEnd(m_nextManagementState);
        }


        await UniTask.CompletedTask;
    }


    override public async UniTask OnExit()
    {
        await UniTask.CompletedTask;
    }
}

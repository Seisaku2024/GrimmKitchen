using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagementStateUpdateInfo;

public class ManagementStateInputToCreateWindow :InputToCreateWindow
{
    // 制作者 田内
    // 経営ステートを確認しつつウィンドウを作成する

    [Header("作成可能ステート")]
    [SerializeField]
    private List<ManagementState> m_stateList = new();


    //============================================
    //                実行処理
    //============================================

    protected override void UpdateInput()
    {
        foreach(var id in m_stateList)
        {
            if(ManagementStateUpdateManager.instance.IsState((int)id))
            {
                base.UpdateInput();
            }
        }

    }

}

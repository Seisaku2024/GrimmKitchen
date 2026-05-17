using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class AddTotalEarnedMoneyButton : ButtonData
{
    // 制作者 田内
    // 所持金追加

    [Header("金額追加量")]
    [SerializeField]
    private int m_addTotalEarnedMoney = 10000;

    //=============================================
    //                  実行処理
    //=============================================

    public override async UniTask OnPressUpdate()
    {
        ManagementDataManager.instance.TotalEarnedMoney += m_addTotalEarnedMoney;

        await UniTask.CompletedTask;
    }

}

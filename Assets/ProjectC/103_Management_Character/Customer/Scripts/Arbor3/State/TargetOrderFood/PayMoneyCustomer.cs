/*!
 * @file PayMoneyCustomer.cs
 * @brief 頼んだ料理の金額を払うステート
 * @author 田内
 * @date 10/16
 *        食い逃げ時にお金を払う処理を追加 上甲
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddBehaviourMenu("Customer/PayMoneyCustomer")]
[AddComponentMenu("")]
public class PayMoneyCustomer : BaseCustomerStateBehaviour
{
    //! @brief 合計金額から返却される割合
    [SerializeField, Range(0, 1), Header("食い逃げ成敗時合計金額から返却される割合")]
    float m_payBackRate = 0.3f;


    // 制作者　田内

    //! @brief 通常支払いか食い逃げか
    [Header("true 通常支払い false 食い逃げ時支払い")]
    [SerializeField] bool m_isNormalPay = true;


    //===================================================================================================================

    //! @brief お金を払う処理
    private void Pay()
    {
        var data = GetCustomerData();
        if (data == null) return;

        var targetFood = data.TargetOrderFoodData;
        if (targetFood == null) return;

        // お金を支払う
        ManagementGameDataManager.instance.AddEarnedMoney(data.TotalPrice);

        // 提供した数を足す
        foreach (var id in data.EatFoodList)
        {
            ManagementGameDataManager.instance.AddSoldNumFoodData(id);
        }
    }

    //! @brief 食い逃げ失敗時にお金を払う処理
    private void PayBack()
    {
        var data = GetCustomerData();
        if (data == null) return;

        float value = data.TotalPrice * m_payBackRate;
        ManagementGameDataManager.instance.AddEarnedMoney((int)value);
    }



    public override void OnStateBegin()
    {
        if (m_isNormalPay)
        {
            Pay();
        }
        else
        {
            PayBack();
        }
    }
}

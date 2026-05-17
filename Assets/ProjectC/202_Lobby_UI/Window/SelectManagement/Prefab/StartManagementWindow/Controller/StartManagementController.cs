using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManagementController : MonoBehaviour
{
    // 制作者 田内
    // 経営を開始できるかどうか


    //=======================================================
    //             実行処理
    //=======================================================

    /// <summary>
    /// スタッフの条件を満たしているか
    /// </summary>
    public bool IsStaff()
    {
        // スタッフ関係が問題なければ
        if (StaffManager.instance.IsSettingStaffPoint())
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// スタッフの条件を満たしているか
    /// </summary>
    public bool IsSalaryPrice()
    {
        // スタッフの給料が問題なければ
        if (StaffManager.instance.IsSalaryPriceStaffPoint())
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// 提供料理をセットしているか
    /// </summary>
    public bool IsSettingProvideFood()
    {
        // 提供料理関係が問題なければ
        if (ProvideFoodManager.instance.IsSettingProvideFood())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 提供料理が提供できる個数存在するか
    /// </summary>
    public bool IsProvideFoodNum()
    {
        // 提供料理関係が問題なければ
        if (ProvideFoodManager.instance.IsProvideAll())
        {
            return true;
        }

        return false;
    }


    public bool IsStart()
    {
        // 全ての条件を満たしていれば
        if (IsStaff() && IsSalaryPrice() && IsSettingProvideFood() && IsProvideFoodNum())
        {
            return true;
        }

        return false;
    }

}

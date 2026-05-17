using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;



public class StaffMemberDataBaseManager : BaseManager<StaffMemberDataBaseManager>
{
    // 制作者 田内

    [Header("ランダムスタッフデータベース")]
    [SerializeField]
    private StaffMemberDataBase m_normalStaffMemberDataBase = null;

    public StaffMemberDataBase NormalStaffMemberDataBase
    {
        get { return m_normalStaffMemberDataBase; }
    }

    [Header("レアスタッフデータベース")]
    [SerializeField]
    private StaffMemberDataBase m_rareStaffMemberDataBase = null;

    public StaffMemberDataBase RareStaffMemberDataBase
    {
        get { return m_rareStaffMemberDataBase; }
    }

    [Header("激レアスタッフデータベース")]
    [SerializeField]
    private StaffMemberDataBase m_superRareStaffMemberDataBase = null;

    public StaffMemberDataBase SuperRareStaffMemberDataBase
    {
        get { return m_superRareStaffMemberDataBase; }
    }

    //============================================
    //              実行処理
    //============================================

    public StaffMemberData GetStaffMemberData(StaffID _id)
    {

        // 激レアリスト
        foreach (var data in m_superRareStaffMemberDataBase.StaffMemberDataBaseList)
        {
            // IDが一致
            if (data.StaffID == _id)
            {
                return data;
            }
        }

        // レアリスト
        foreach (var data in m_rareStaffMemberDataBase.StaffMemberDataBaseList)
        {
            // IDが一致
            if (data.StaffID == _id)
            {
                return data;
            }
        }

        // ノーマルリスト
        foreach (var data in m_normalStaffMemberDataBase.StaffMemberDataBaseList)
        {
            // IDが一致
            if (data.StaffID == _id)
            {
                return data;
            }
        }

        Debug.LogError(_id.ToString() + "このIDのスタッフは登録されていません");
        return null;

    }

    public enum Rarity
    {
        N = 0,
        R = 1,
        SR = 2
    }
    public Rarity GetStaffRarity(StaffID _id)
    {
        // 激レアリスト
        foreach (var data in m_superRareStaffMemberDataBase.StaffMemberDataBaseList)
        {
            // IDが一致
            if (data.StaffID == _id)
            {
                return Rarity.SR;
            }
        }

        // レアリスト
        foreach (var data in m_rareStaffMemberDataBase.StaffMemberDataBaseList)
        {
            // IDが一致
            if (data.StaffID == _id)
            {
                return Rarity.R;
            }
        }

        // ノーマルリスト
        foreach (var data in m_normalStaffMemberDataBase.StaffMemberDataBaseList)
        {
            // IDが一致
            if (data.StaffID == _id)
            {
                return Rarity.N;
            }
        }

        Debug.LogError(_id.ToString() + "このIDのスタッフは登録されていません");
        return Rarity.N;

    }

    /// <summary>
    /// レアリティを基にランダムでスタッフデータを取得
    /// </summary>
    /// <returns></returns>
    public StaffMemberData GetRandomRarityStaffMemberData(StaffRarityType _staffRarityType)
    {
        List<StaffMemberData> list = new();

        switch (_staffRarityType)
        {
            // ノーマルリストから取得
            case StaffRarityType.Normal:
                {
                    list = m_normalStaffMemberDataBase.StaffMemberDataBaseList.GetShuffleRandomList();
                    break;
                }

            // レアリストから取得
            case StaffRarityType.Rare:
                {
                    list = m_rareStaffMemberDataBase.StaffMemberDataBaseList.GetShuffleRandomList();
                    break;
                }

            // 激レアリストから取得
            case StaffRarityType.SuperRare:
                {
                    list = m_superRareStaffMemberDataBase.StaffMemberDataBaseList.GetShuffleRandomList();
                    break;
                }
        }

        foreach (var data in list)
        {
            if (data == null) continue;
            return data;
        }

        return null;
    }



}

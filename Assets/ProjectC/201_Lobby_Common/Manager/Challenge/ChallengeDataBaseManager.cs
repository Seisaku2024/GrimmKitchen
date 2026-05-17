using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;

namespace ChallengeInfo
{
    public enum ChallengeID
    {
        None = 0,

        // メインストーリー
        UnlockStage2 = 2,
        UnlockStage3 = 3,







        // 料理解放
        UnlockCurry = 101,
        UnlockPotatoSalad = 102,
        UnlockShabuShabu = 103,
        UnlockSnakeSake = 104,

        // 常駐イベント
        AddMoney1 = 201,


        AddRandomVegetableIngredient = 251,


        // デバッグ用のテスト
        Test = 999,

    }

    public enum ChallengeType
    {
        None = 0,
        Main,
        Sub,
        All = 100,
    }

    public enum ChallengePlayType
    {
        None = 0,
        Once,
        Multiple,
    }

}


public class ChallengeDataBaseManager : BaseManager<ChallengeDataBaseManager>
{
    // 制作者 田内
    // チャレンジデータベースを管理するマネージャー

    //======================================================

    [Header("無しデータ")]
    [SerializeField]
    private ChallengeData m_noneChallengeData = null;

    //======================================================

    [Header("メインデータベース")]
    [SerializeField]
    private ChallengeDataBase m_mainChallengeDataBase = null;

    public ChallengeDataBase MainChallengeDataBase
    {
        get { return m_mainChallengeDataBase; }
    }

    //======================================================

    [Header("サブデータベース")]
    [SerializeField]
    private ChallengeDataBase m_subChallengeDataBase = null;

    public ChallengeDataBase SubChallengeDataBase
    {
        get { return m_subChallengeDataBase; }
    }

    //======================================================

    [Header("チャレンジタイプデータベース")]
    [SerializeField]
    private ChallengeTypeDataBase m_challengeTypeDataBase = null;

    public ChallengeTypeDataBase ChallengeTypeDataBase
    {

        get { return m_challengeTypeDataBase; }
    }

    //======================================================

    [Header("チャレンジプレイタイプデータベース")]
    [SerializeField]
    private ChallengePlayTypeDataBase m_challengePlayTypeDataBase = null;

    public ChallengePlayTypeDataBase ChallengePlayTypeDataBase
    {

        get { return m_challengePlayTypeDataBase; }
    }


    //============================================
    //              実行処理
    //============================================

    protected override void Load()
    {
        foreach (var data in m_mainChallengeDataBase.ChallengeDataList)
        {
            if (data == null) continue;
            var saveLoadData = ChallengeSaveLoader.Load(data.ChallengeID);
            data.Load(saveLoadData);
        }
        foreach (var data in m_subChallengeDataBase.ChallengeDataList)
        {
            if (data == null) continue;
            var saveLoadData = ChallengeSaveLoader.Load(data.ChallengeID);
            data.Load(saveLoadData);
        }
    }


    /// <summary>
    /// 引数IDのデータを取得する
    /// </summary>
    public ChallengeData GetData(ChallengeID _id)
    {
        if (m_noneChallengeData != null)
        {
            if (_id == m_noneChallengeData.ChallengeID) return m_noneChallengeData;
        }
        else
        {
            Debug.LogError("NoneChallengeDataがシリアライズされていません");
        }


        if (m_mainChallengeDataBase != null)
        {
            foreach (var data in m_mainChallengeDataBase.ChallengeDataList)
            {
                if (data == null) continue;
                if (data.ChallengeID == _id) return data;
            }
        }
        else
        {

            Debug.LogError("MainChallengeデータベースがシリアライズされていません");
        }

        if (m_subChallengeDataBase != null)
        {
            foreach (var data in m_subChallengeDataBase.ChallengeDataList)
            {
                if (data == null) continue;
                if (data.ChallengeID == _id) return data;
            }
        }
        else
        {
            Debug.LogError("SubChallengeデータベースがシリアライズされていません");
        }


        Debug.LogError(_id.ToString() + "このIDのチャレンジデータは登録されていません");
        return null;
    }

    /// <summary>
    /// 引数IDのデータを取得する
    /// </summary>
    public ChallengeTypeData GetTypeData(ChallengeType _type)
    {
        // 無しであれば
        if (ChallengeType.None == _type || ChallengeType.All == _type) return null;

        if (m_challengeTypeDataBase != null)
        {
            foreach (var data in m_challengeTypeDataBase.ChallengeTypeDataList)
            {
                if (data == null) continue;
                if (data.ChalengeType == _type) return data;
            }
        }
        else
        {

            Debug.LogError("ChallengeTypeデータベースがシリアライズされていません");
        }


        Debug.LogError(_type.ToString() + "このIDのチャレンジタイプデータは登録されていません");
        return null;
    }


    // <summary>
    /// 引数IDのデータを取得する
    /// </summary>
    public ChallengePlayTypeData GetPlayTypeData(ChallengePlayType _type)
    {
        // 無しであれば
        if (ChallengePlayType.None == _type) return null;

        if (m_challengePlayTypeDataBase != null)
        {
            foreach (var data in m_challengePlayTypeDataBase.ChallengePlayTypeDataList)
            {
                if (data == null) continue;
                if (data.ChallengePlayType == _type) return data;
            }
        }
        else
        {

            Debug.LogError("ChallengePlayTypeデータベースがシリアライズされていません");
        }


        Debug.LogError(_type.ToString() + "このIDのチャレンジプレイタイプデータは登録されていません");
        return null;
    }


    /// <summary>
    /// 引数IDのチャレンジタイプを取得する
    /// </summary>
    public ChallengeType GetChallengeType(ChallengeID _id)
    {
        // メイン
        if (m_mainChallengeDataBase != null)
        {

            foreach (var data in m_mainChallengeDataBase.ChallengeDataList)
            {
                if (data == null) continue;

                // 一致すれば
                if (data.ChallengeID == _id)
                {
                    return ChallengeType.Main;
                }
            }

        }

        // サブ
        if (m_subChallengeDataBase != null)
        {
            foreach (var data in m_subChallengeDataBase.ChallengeDataList)
            {
                if (data == null) continue;

                // 一致すれば
                if (data.ChallengeID == _id)
                {
                    return ChallengeType.Sub;
                }
            }

        }

        return ChallengeType.None;
    }



    /// <summary>
    /// リスト内のデータを全て取得する
    /// </summary>
    /// <returns></returns>
    public List<ChallengeData> GetDataList()
    {
        List<ChallengeData> list = new();
        if (m_mainChallengeDataBase != null)
        {
            list.AddRange(m_mainChallengeDataBase.ChallengeDataList);
        }
        else
        {

            Debug.LogError("MainChallengeデータベースがシリアライズされていません");
        }

        if (m_subChallengeDataBase != null)
        {
            list.AddRange(m_subChallengeDataBase.ChallengeDataList);
        }
        else
        {
            Debug.LogError("SubChallengeデータベースがシリアライズされていません");
        }

        return list;
    }



}

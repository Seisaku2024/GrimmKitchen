using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagementGameInfo;
using UniRx;
using DG.Tweening;
using OccaSoftware.SuperSimpleSkybox.Runtime;
using FoodInfo;
using ChallengeInfo;
using System.Linq;

namespace ManagementGameInfo
{
    public enum CameCustomerType
    {
        Normal,
        Angry,
    }

    public enum EventSolutionType
    {
        /// <summary>
        /// 解決
        /// </summary>
        Solution,

        /// <summary>
        /// 未解決
        /// </summary>
        UnSolution,
    }

}

public class ManagementGameDataManager : BaseManager<ManagementGameDataManager>
{
    // 経営パートプレイ時の情報を管理するクラス
    // 制作者　田内


    //============
    // 制限時間

    [Header("制限時間")]
    [SerializeField]
    [Range(0.1f, 600.0f)]
    private float m_timeLimit = 120.0f;

    // 環境光・平行光源関係（山本）------------------------------------------------------------------------

    [Header("環境光の強度を変える時間")]
    [SerializeField]
    [Range(0.1f, 600.0f)]
    private float m_changeAmbientPowTime = 60.0f;

    [Header("環境光の強度が完全に変わるまでの時間（Dotween）")]
    [SerializeField]
    [Range(0.1f, 600.0f)]
    private float m_changeTime = 30.0f;

    //初期の強度
    private float m_startAmbientPow = 0.0f;

    // 変更先の強度
    [SerializeField]
    private float m_endAmbientPow = 0.3f;

    // 平行光源（太陽）の管理クラス
    [SerializeField]
    private Sun m_sunParameter;

    //------------------------------------------------------------------------------------

    private Tweener m_weener = null;


    public float TimeLimit
    {
        get { return m_timeLimit; }
    }

    public bool IsTimeOut()
    {
        if (m_timeLimit < m_currentElapsedTime)
        {
            return true;
        }
        return false;
    }
    //===============
    // 提供料理

    private ReactiveCollection<ReactiveProperty<ManagementProvideFoodData>> m_provideFoodDataRPRC = new(new());

    public ReactiveCollection<ReactiveProperty<ManagementProvideFoodData>> ProvideFoodDataRPRC
    {
        get { return m_provideFoodDataRPRC; }
    }

    //===============
    // チャレンジ条件

    private ReactiveCollection<ReactiveProperty<BaseClearConditionChallengeData>> m_clearConditionChallengeDataRPRC = new(new());

    public ReactiveCollection<ReactiveProperty<BaseClearConditionChallengeData>> ClearConditionChallengeDataRPRC
    {
        get { return m_clearConditionChallengeDataRPRC; }
    }

    //=============
    // 経過した時間

    private float m_currentElapsedTime = 0.0f;

    public float CurrentElapsedTime
    {
        get { return m_currentElapsedTime; }
    }

    // 全体から見た経過時間の割合
    public float CurrentElapsedTimeRatio
    {
        get { return m_currentElapsedTime / m_timeLimit; }
    }


    //===============
    // 現在の満足値

    private ReactiveProperty<int> m_currentSatisfactionValueRP = new(0);

    public int CurrentSatisfactionValue
    {
        get { return m_currentSatisfactionValueRP.Value; }
    }

    public System.IObservable<int> CurrentSatisfactionValueRP
    {
        get { return m_currentSatisfactionValueRP; }
    }


    //=================
    // 稼いだ金額

    private ReactiveProperty<int> m_earnedMoneyRP = new(0);

    public int EarnedMoney
    {
        get { return m_earnedMoneyRP.Value; }
    }

    public System.IObservable<int> EarnedMoneyRP
    {
        get { return m_earnedMoneyRP; }
    }

    //===========
    // 来店者数

    private Dictionary<CameCustomerType, uint> m_cameCustomerNumDictionary = new();

    public Dictionary<CameCustomerType, uint> CameCustomerNumDictionary
    {
        get { return m_cameCustomerNumDictionary; }
    }


    //==========================
    // イベント数

    private Dictionary<EventSolutionType, uint> m_eventSolutionNumDictionary = new();

    public Dictionary<EventSolutionType, uint> EventSolutionNumDictionary
    {
        get { return m_eventSolutionNumDictionary; }
    }

    //======================================================
    //                      実行処理
    //======================================================

    // 山本追加
    private void Start()
    {
        //初期強度を記憶
        m_startAmbientPow = RenderSettings.ambientIntensity;

        // 田内追加
        CreateProvideFoodDataRPRC();
        CreateClearConditionChallengeDataRPRC();
    }

    private void Update()
    {
        TimeCount();
    }


    // 経過時間をカウント
    private void TimeCount()
    {
        if (m_currentElapsedTime < m_timeLimit)
        {
            m_currentElapsedTime += Time.deltaTime;
        }
        else
        {
            //　光源を傾かせないようにする(山本)
            if (m_sunParameter)
            {
                m_sunParameter.RotationsPerHour = 0;
            }

        }

        if (m_weener != null) return;

        //　環境光を指定時間になると弱める処理（山本）
        if (m_currentElapsedTime > m_changeAmbientPowTime)
        {
            m_weener = DOVirtual.Float(
                 m_startAmbientPow, m_endAmbientPow, m_changeTime,
                value =>
                {
                    RenderSettings.ambientIntensity = value;
                }
                );

        }

    }


    /// <summary>
    /// 稼いだ金額を追加
    /// </summary>
    public void AddEarnedMoney(int _value)
    {
        m_earnedMoneyRP.Value += _value;
    }


    /// <summary>
    /// 満足度を追加
    /// </summary>
    public void AddSatisfactionValue(int _satisfactionValue)
    {
        m_currentSatisfactionValueRP.Value += _satisfactionValue;

        if (m_currentSatisfactionValueRP.Value < 0)
        {
            m_currentSatisfactionValueRP.Value = 0;
        }
    }


    /// <summary>
    /// 来客数を追加
    /// </summary>
    public void AddCameCustomerNum(CameCustomerType _id, uint _value = 1)
    {

        if (m_cameCustomerNumDictionary.ContainsKey(_id))
        {
            m_cameCustomerNumDictionary[_id] += _value;
        }
        // 存在しなければ
        else
        {
            m_cameCustomerNumDictionary[_id] = _value;
        }
    }

    /// <summary>
    /// イベント数を追加
    /// </summary>
    public void AddEventSolutionNum(EventSolutionType _id, uint _value = 1)
    {

        if (m_eventSolutionNumDictionary.ContainsKey(_id))
        {
            m_eventSolutionNumDictionary[_id] += _value;
        }
        // 存在しなければ
        else
        {
            m_eventSolutionNumDictionary[_id] = _value;
        }
    }

    /// <summary>
    /// 経過時間が制限時間を過ぎたかどうか
    /// </summary>
    public bool IsTimeOver()
    {
        if (m_timeLimit < m_currentElapsedTime)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ゲームが終了したかどうか
    /// </summary>
    public bool IsGameEnd()
    {
        if (IsProvideAll() == false || IsTimeOver() == true)
        {
            // 客が残っていないか
            if (CustomerManager.instance.IsCustomerExistence() == false)
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// 経営情報を更新する
    /// この関数はプレイで稼いだ金額等を経営情報管理クラスに更新するものです
    /// 適切なところで使用してください
    /// </summary>
    public void SettingManagementData()
    {
        // この関数はプレイで稼いだ金額等を経営情報管理クラスに更新するものです
        // 適切なところで使用してください

        // 稼いだ金額を総額に追加
        ManagementDataManager.instance.TotalEarnedMoney += m_earnedMoneyRP.Value;

        ManagementDataManager.instance.TotalEvaluation += m_currentSatisfactionValueRP.Value;

    }


    //=======================================
    //            提供料理
    //=======================================

    private void CreateProvideFoodDataRPRC()
    {
        foreach (var id in ProvideFoodManager.instance.ProvideFoodIDList)
        {
            ReactiveProperty<ManagementProvideFoodData> rpData = new();
            rpData.Value = new(id);
            m_provideFoodDataRPRC.Add(rpData);
        }
    }

    /// <summary>
    /// 引数IDと一致するRP版提供料理データを取得
    /// </summary>
    public ReactiveProperty<ManagementProvideFoodData> GetProvideFoodDataRP(FoodID _id)
    {
        foreach (var data in m_provideFoodDataRPRC)
        {
            if (data == null || data.Value == null) continue;
            if (data.Value.FoodID == _id) return data;
        }
        return null;
    }

    /// <summary>
    /// 引数IDと一致する提供料理データを取得
    /// </summary>
    public ManagementProvideFoodData GetProvideFoodData(FoodID _id)
    {
        foreach (var data in m_provideFoodDataRPRC)
        {
            if (data == null || data.Value == null) continue;
            if (data.Value.FoodID == _id) return data.Value;
        }
        return null;
    }

    /// <summary>
    /// 引数提供料理にボーナスを追加
    /// </summary>
    public void AddBonusNum(FoodID _id, uint _bonusNum = 1)
    {
        var data = GetProvideFoodData(_id);
        if (data == null) return;

        data.AddBonusNum();
    }


    /// <summary>
    /// 引数提供料理に売り上げ数を加算
    /// </summary>
    public void AddSoldNumFoodData(FoodID _id)
    {
        var data = GetProvideFoodData(_id);
        if (data == null) return;

        data.AddSoldNum();
    }

    /// <summary>
    /// 引数提供料理から必要素材を取り除く
    /// </summary>
    public void AddRemoveNumProvideFoodData(FoodID _id)
    {
        var data = GetProvideFoodData(_id);
        if (data == null) return;

        data.AddRemoveNum();
    }


    /// <summary>
    /// 提供料理が提供可能かどうか確認する
    /// </summary>
    public bool IsProvideAll()
    {
        foreach (var data in m_provideFoodDataRPRC)
        {
            if (data == null || data.Value == null) continue;
            if (data.Value.IsProvide()) return true;
        }
        return false;
    }


    /// <summary>
    /// リストからランダムで料理データを取得
    /// 提供できるものが無ければnullを返す
    /// </summary>
    public ReactiveProperty<ManagementProvideFoodData> GetRandomProvideFoodData()
    {
        // ランダムにする
        var randomList = m_provideFoodDataRPRC.ToList().GetShuffleRandomList();

        foreach (var data in randomList)
        {
            if (data == null) continue;
            if (FoodData.IsProvide(ProvideFoodManager.instance.PocketType, data.Value.FoodID)) return data;
        }
        return null;
    }

    //=======================================
    //            チャレンジ
    //=======================================

    private void CreateClearConditionChallengeDataRPRC()
    {
        var challengeData = ChallengeDataBaseManager.instance.GetData(ChallengeManager.instance.ChallengeID);
        foreach (var data in challengeData.ClearConditionChallengeList)
        {
            if (data == null) continue;
            // 作成
            ReactiveProperty<BaseClearConditionChallengeData> rpClearConditionChallenge = new(Instantiate(data, gameObject.transform));
            m_clearConditionChallengeDataRPRC.Add(rpClearConditionChallenge);
        }
    }


    /// <summary>
    /// 選択中のチャレンジがクリアされたか
    /// </summary>
    public bool IsClearChallenge()
    {
        foreach (var data in m_clearConditionChallengeDataRPRC)
        {
            if (data == null || data.Value == null) continue;

            // 一つでもクリアしていなければfalse
            if (data.Value.IsClear() == false)
            {
                return false;
            }
        }
        // 問題なければtrue
        return true;
    }

    /// <summary>
    /// 選択中のチャレンジがクリアされたか
    /// </summary>
    public void ClearChallenge()
    {
        if (IsClearChallenge() == false) return;

        var data = ChallengeDataBaseManager.instance.GetData(ChallengeManager.instance.ChallengeID);
        if (data == null) return;

        // 更新
        data.OnChallengeClear();

        // プレイ可能が一度のみであれば外す
        if (data.IsPlay() == false)
        {
            ChallengeManager.instance.SetChallengeID(ChallengeID.None);
        }

    }

    public float GetCurrentClearConditionAdvanceValue()
    {
        float value = 0.0f;

        foreach (var data in m_clearConditionChallengeDataRPRC)
        {
            if (data == null || data.Value == null) continue;

            value += data.Value.GetClearRatio();
        }

        return value;
    }

    public float GetCurrentClearConditionAdvanceRatio()
    {
        if (m_clearConditionChallengeDataRPRC.Count <= 0) return 0.0f;

        int count = m_clearConditionChallengeDataRPRC.Count;
        float ratio = GetCurrentClearConditionAdvanceValue();

        return ratio / count;
    }


}

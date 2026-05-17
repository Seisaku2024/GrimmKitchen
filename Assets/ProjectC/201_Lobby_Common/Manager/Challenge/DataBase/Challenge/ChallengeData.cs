using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;
using IngredientInfo;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "ChallengeData", menuName = "ScriptableObjects/Challenge/作成 ChallengeData")]
public class ChallengeData : ScriptableObject
{
    // 制作者 田内
    // チャレンジ情報

    [System.Serializable]
    public class FixedChallengeEvent
    {
        [Header("イベント")]
        [SerializeField]
        private BaseManagementEvent m_managementEvent = null;

        public BaseManagementEvent ManagementEvent
        {
            get { return m_managementEvent; }
        }

        [Header("イベント開始値(%)")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float m_startValue = 0.5f;

        public float StartValue
        {
            get { return m_startValue; }
        }
    }


    //=====================================================

    [Header("ID")]
    [SerializeField]
    private ChallengeID m_challengeID = ChallengeID.None;

    public ChallengeID ChallengeID
    {
        get { return m_challengeID; }
    }

    //=====================================================

    [Header("チャレンジ名")]
    [SerializeField]
    private LocalizedString m_challengeName = new();

    public LocalizedString ChallengeName
    {
        get { return m_challengeName; }
    }


    //=====================================================

    [Header("チャレンジ画像")]
    [SerializeField]
    private Sprite m_challengeSprite = null;

    public Sprite ChallengeSprite
    {
        get { return m_challengeSprite; }
    }

    //=====================================================

    [Header("説明文")]
    [SerializeField]
    private LocalizedString m_challengeDescription = new();

    public LocalizedString ChallengeDescription
    {
        get { return m_challengeDescription; }
    }

    //====================================

    [Header("売れやすい料理")]
    [SerializeField]
    private IngredientTypeID m_businessConditionsIngredientTypeID = IngredientTypeID.None;

    public IngredientTypeID BusinessConditionsIngredientTypeID
    {
        get { return m_businessConditionsIngredientTypeID; }
    }


    //=====================================================

    [Header("高値割合")]
    [SerializeField]
    [Min(1)]
    private float m_businessConditionsRatio = 1.2f;

    public float BusinessConditionsRatio
    {
        get { return m_businessConditionsRatio; }
    }

   
    //=====================================================

    [Header("プレイタイプ")]
    [SerializeField]
    private ChallengePlayType m_playType = ChallengePlayType.Once;

    public ChallengePlayType PlayType
    {
        get { return m_playType; }
    }

    //============================
    // 初期ロック

    [Header("初期ロック")]
    [SerializeField]
    private bool m_initializeLock = false;

    public bool InitializeLock { get { return m_initializeLock; } }

    //=====================================================

    [Header("クリア条件リスト")]
    [SerializeField]
    private List<BaseClearConditionChallengeData> m_clearConditionChallengeList = null;

    public List<BaseClearConditionChallengeData> ClearConditionChallengeList
    {
        get { return m_clearConditionChallengeList; }
    }

    //=====================================================

    [Header("固定発生イベントリスト")]
    [SerializeField]
    private List<FixedChallengeEvent> m_fixedChallengeEventList = new();

    public List<FixedChallengeEvent> FixedChallengeEventList
    {
        get { return m_fixedChallengeEventList; }
    }

    //=====================================================

    [Header("報酬処理")]
    [SerializeField]
    private List<BaseRewardChallengeData> m_rewardChallengeDataList = null;

    public List<BaseRewardChallengeData> RewardChallengeDataList
    {
        get { return m_rewardChallengeDataList; }
    }

    //=====================================================
    // クリアデータ

    private ClearChallengeData m_clearChallengeData = null;

    public ClearChallengeData ClearChallengeData
    {
        get
        {
            // 存在しなければ初期値をセット
            if (m_clearChallengeData == null) m_clearChallengeData = new(m_challengeID);
            return m_clearChallengeData;
        }
    }

    //=====================================================
    //                     実行処理
    //=====================================================

    public void Load(ChallengeSaveLoad _data)
    {
        if (_data == null) return;
        m_clearChallengeData = new(m_challengeID);
        m_clearChallengeData.ClearNum = _data.ClearNum;
        m_clearChallengeData.IsClear = _data.IsClear;
        m_clearChallengeData.IsLock = _data.IsLock;
    }

    /// <summary>
    /// チャレンジをクリアする
    /// </summary>
    public void OnChallengeClear()
    {
        // 存在しなければ新規作成
        if (m_clearChallengeData == null) m_clearChallengeData = new(m_challengeID);

        // クリアを記録
        m_clearChallengeData.IsClear = true;

        // クリア回数を加算
        m_clearChallengeData.ClearNum++;

        // 報酬が存在すれば処理を行う
        foreach (var data in m_rewardChallengeDataList)
        {
            var rewardChallenge = Instantiate(data);
            rewardChallenge.UpdateRewardChallenge();
            if (rewardChallenge != null) Destroy(rewardChallenge.gameObject);
        }
    }

    /// <summary>
    /// プレイできるかどうか
    /// </summary>
    public bool IsPlay()
    {
        if (m_clearChallengeData == null) m_clearChallengeData = new(m_challengeID);

        // 一度しかプレイできない且つ、既にクリアしているか
        if (m_playType == ChallengePlayType.Once && m_clearChallengeData.IsClear == true) return false;

        // 初期ロックされている且つ、アンロックされていないか
        if (m_initializeLock == true && m_clearChallengeData.IsLock == true) return false;

        return true;
    }


    /// <summary>
    /// チャレンジの種類を取得
    /// </summary>
    public ChallengeType GetChallengeType()
    {
        return ChallengeDataBaseManager.instance.GetChallengeType(m_challengeID);
    }

    /// <summary>
    /// 引数IDと一致した場合、引数値を割合計算して返信
    /// </summary>
    public int GetConvertValueBusinessConditionsRatio(int _value, IngredientTypeID _id)
    {
        if (_id == IngredientTypeID.None) return _value;

        // IDが一致すれば人気商品割合を加算
        if ((m_businessConditionsIngredientTypeID & _id) == _id)
        {
            // 計算して返信
            float value = (float)_value * (float)m_businessConditionsRatio;
            return (int)value;
        }

        // 一致しない場合はそのまま
        return _value;
    }



}



/// <summary>
/// 読み込み/書き込み用チャレンジデータ
/// </summary>
[System.Serializable]
public class ChallengeSaveLoad
{

    public ChallengeSaveLoad(ChallengeID _id)
    {
        var data = ChallengeDataBaseManager.instance.GetData(_id);
        if (data == null) return;

        ChallengeID = _id;
        IsLock = data.InitializeLock;
        IsClear = false;
        ClearNum = 0;
    }


    public ChallengeSaveLoad(ChallengeID _id, ClearChallengeData _data)
    {
        if (_data == null) return;
        ChallengeID = _id;
        IsLock = _data.IsLock;
        IsClear = _data.IsClear;
        ClearNum = _data.ClearNum;
    }

    public ChallengeID ChallengeID = ChallengeID.None;

    public bool IsLock = false;

    public bool IsClear = false;

    public uint ClearNum = 0;
}


public class ClearChallengeData
{
    public ClearChallengeData(ChallengeID _id)
    {
        var data = ChallengeDataBaseManager.instance.GetData(_id);
        if (data == null) return;

        IsLock = data.InitializeLock;
        IsClear = false;
        ClearNum = 0;
    }


    public bool IsLock = false;

    public bool IsClear = false;

    public uint ClearNum = 0;
}
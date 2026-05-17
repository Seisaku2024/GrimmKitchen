using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Localization;

public abstract class BaseClearConditionChallengeData : MonoBehaviour
{
    // 制作者 田内
    // チャレンジのクリア条件

    /// <summary>
    /// 更新イベント
    /// </summary>
    public class GlobalChangeClearConditionChallenge
    {
        // 変更が加わったイベント
        public BaseClearConditionChallengeData ClearConditionChallengeData = null;
    }


    [Header("条件名")]
    [SerializeField]
    private LocalizeValueController m_conditionName = null;

    public LocalizeValueController ConditionName
    {
        get { return m_conditionName; }
    }


    [Header("条件画像")]
    [SerializeField]
    private Sprite m_conditionSprite = null;

    public Sprite ConditionSprite
    {
        get { return m_conditionSprite; }
    }

    //============================================
    //              実行処理
    //============================================

    private void Update()
    {
        UpdateClearConditionChallenge();
    }


    /// <summary>
    /// 実行される処理
    /// </summary>
    public void UpdateClearConditionChallenge()
    {

    }


    /// <summary>
    /// クリアしたかどうか
    /// </summary>
    abstract public bool IsClear();


    /// <summary>
    /// 条件テキスト
    /// </summary>
    abstract public string GetConditionText();

    /// <summary>
    /// 追加　吉田
    /// 条件の個数表示のみ
    /// </summary>
    abstract public string GetConditionNumText();


    /// <summary>
    /// 現在のクリア割合
    /// (※ 0～1の割合で返信すること)
    /// </summary>
    abstract public float GetClearRatio();


    /// <summary>
    /// クリア条件が更新されたことを発信
    /// </summary>
    protected void PublishChangeClearConditionChallenge()
    {
        // イベント送信
        GlobalChangeClearConditionChallenge eve = new();
        eve.ClearConditionChallengeData= this;
        MessageBroker.Default.Publish<GlobalChangeClearConditionChallenge>(eve);
    }
}

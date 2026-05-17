using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;


[RequireComponent(typeof(ChallengeDescription))]
public class ChangeChallengeDescription : MonoBehaviour
{
    // 制作者 田内
    // チャレンジの説明文を表示

    [Header("コントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;

    //========================================================
    // 説明文
    protected ChallengeDescription m_challengeDescription = null;

    //============================================
    // 選択中のチャレンジデータ
    protected ChallengeData m_challengeData = null;


    //==============================================
    //              実行処理
    //==============================================

    virtual protected void Start()
    {
        // 選択中のチャレンジが変更されれば再度更新する
        ChallengeManager.instance.ChallengeIDRP.Subscribe(_ =>
        {
            // 説明文を更新
            if (m_challengeDescription == null) m_challengeDescription = gameObject.GetComponent<ChallengeDescription>();
            m_challengeDescription.UpdateDescription(m_challengeData);
        }).AddTo(this);
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    virtual public void OnInitialize()
    {
        // 初期化
        SetDescription();
    }


    /// <summary>
    /// 実行処理
    /// </summary>
    virtual public void OnUpdate()
    {
        if (IsChangeDescription())
        {
            SetDescription();
        }
    }


    // 説明文を変更できるか確認
    protected bool IsChangeDescription()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return false;
        }
        #endregion

        return m_selectUIController.IsSelectChangeFlg;
    }



    virtual protected void SetDescription()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectTutorialControllerがシリアライズされていません");
            return;
        }

        m_challengeData = null;

        var data = m_selectUIController.CurrentSelectUI;
        if (data != null)
        {
            if (data.TryGetComponent<ChallengeSlotData>(out var slotData))
            {
                // ターゲット更新
                m_challengeData = slotData.ChallengeData;
            }
        }

        // 説明文を更新
        if (m_challengeDescription == null) m_challengeDescription = gameObject.GetComponent<ChallengeDescription>();
        m_challengeDescription.UpdateDescription(m_challengeData);

    }
}

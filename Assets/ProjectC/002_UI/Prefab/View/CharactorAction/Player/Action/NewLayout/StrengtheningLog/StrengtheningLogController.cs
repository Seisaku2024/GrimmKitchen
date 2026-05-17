using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using static FoodData;
using static FoodData.AddStatus;
using static BaseDoTweenUI;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization;

public class StrengtheningLogController : BaseDoTweenUI
{
    [SerializeField] private TextMeshProUGUI m_text;

    //[Header("ログの文章")]
    //[SerializeField, EnumIndex(typeof(AddStatusType))] private UnityEngine.Localization.LocalizedString[] m_logText;
    struct LogStatus
    {
        public AddStatusType statusType;
        public int addValue;
        public LogStatus(AddStatusType _statusType, int _addVal)
        {
            statusType = _statusType; addValue = _addVal;
        }
    }
    private List<LogStatus> m_addStatus = new();
    public void SetAddStatus(AddStatus status, int addValue)
    {
        m_addStatus.Add(new LogStatus(status.AddType, addValue));
        if (m_sequence == null)
        {
            ChangeText();
        }
        StartDoTween();
    }

    [System.Serializable]
    class DoAlphaCanvasGroupData : BaseEasingData
    {
        [Header("透明度")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float m_targetAlpha = 0.0f;

        public float TargetAlpha
        {
            get { return m_targetAlpha; }
        }
    }
    [Header("ターゲットの透明度")]
    [SerializeField]
    private List<DoAlphaCanvasGroupData> m_targetAlphaList = new();

    private CanvasGroup m_canvasGroup;

    void Start()
    {
        if (!TryGetComponent(out m_canvasGroup))
        {
            Debug.Log("CanvasGroupが見つかりません");
        }

        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public override void StartDoTween()
    {
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }

        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        if (m_sequence == null) m_sequence = DOTween.Sequence();

        // 一度初期化
        // KillSequence();

        foreach (var data in m_targetAlphaList)
        {
            if (data == m_targetAlphaList[m_targetAlphaList.Count - 1])
            {
                m_sequence.Append(
                    m_canvasGroup.DOFade(data.TargetAlpha, data.Duration).
                    SetDelay(data.Delay).
                    SetEase(data.Ease).
                    SetLink(gameObject).
                    OnComplete(
                        () => ReStart()
                        )
                    );
            }
            else
            {
                m_sequence.Append(
                    m_canvasGroup.DOFade(data.TargetAlpha, data.Duration).
                    SetDelay(data.Delay).
                    SetEase(data.Ease).
                    SetLink(gameObject));
            }
        }

        m_sequence.SetLoops(m_loopCount, m_loopType);
        m_sequence.SetUpdate(m_isGameStopMove);
    }

    void ReStart()
    {
        if (m_addStatus.Count <= 0)
        {
            m_sequence = null;
            return;
        }

        ChangeText();
        StartDoTween();
    }

    void ChangeText()
    {
        if (m_addStatus.Count <= 0)
        {
            m_sequence = null;
            return;
        }

        LocalizedString textLocalyze = new();

        if (m_addStatus[0].addValue <= 0)
        {
            textLocalyze = new LocalizedString(tableReference: "PlayerStatus", entryReference: "MaxLevelUp");
            m_text.text = textLocalyze.GetLocalizedString();
            m_addStatus.Remove(m_addStatus[0]);
            return;
        }


        switch (m_addStatus[0].statusType)
        {
            case AddStatusType.Attack:
                textLocalyze = new LocalizedString(tableReference: "PlayerStatus", entryReference: "AttackLevelUpLog")
                {
                    { "Value", new FloatVariable { Value = m_addStatus[0].addValue}},
                };
                break;
            case AddStatusType.Hp:
                textLocalyze = new LocalizedString(tableReference: "PlayerStatus", entryReference: "HPLevelUpLog")
                {
                    { "Value", new FloatVariable { Value = m_addStatus[0].addValue}},
                };
                break;
            case AddStatusType.Defence:
                textLocalyze = new LocalizedString(tableReference: "PlayerStatus", entryReference: "DefenceLevelUpLog")
                {
                    { "Value", new FloatVariable { Value = m_addStatus[0].addValue}},
                };
                break;
            case AddStatusType.Stamina:
                textLocalyze = new LocalizedString(tableReference: "PlayerStatus", entryReference: "StaminaLevelUpLog")
                {
                    { "Value", new FloatVariable { Value = m_addStatus[0].addValue}},
                };
                break;
        }
        m_text.text = textLocalyze.GetLocalizedString();
        m_addStatus.Remove(m_addStatus[0]);
    }
}

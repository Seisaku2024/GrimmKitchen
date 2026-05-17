using Speaker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;

//会話情報をまとめた構造体
[Serializable]
public struct  ConversationInformaton
{
    //会話テキスト
    [SerializeField]
    private UnityEngine.Localization.LocalizedString m_localizedString;
    public UnityEngine.Localization.LocalizedString LocalizedString => m_localizedString;

    //話者（テキストの名前に表示される）
    [SerializeField]
    private UnityEngine.Localization.LocalizedString m_speakerName;
    public UnityEngine.Localization.LocalizedString SpeakerName => m_speakerName;

    //カメラターゲットタイプ(話者のタイプ)
    [SerializeField]
    private SpeakerType m_speakerType;
    public SpeakerType SpeakerType => m_speakerType;

    //アニメーション（対応キーをストリングで）
    [SerializeField]
    private string m_animeTrigger;
    public string AnimeTrigger => m_animeTrigger;

    // ターゲットとカットシーンカメラの距離
    [SerializeField]
    private float m_distantCamera;
    public float DistantCamera => m_distantCamera;

    // 会話時に表示するアイテム名
    [SerializeField]
    private string m_activeItemName;
    public string ActiveItemName => m_activeItemName;

    // 会話時に向くキャラクター（変更用）
    [SerializeField]
    private SpeakerType m_faceSpeakeTypeInChange;
    public SpeakerType FaceSpeakeTypeInChange => m_faceSpeakeTypeInChange;

}


[CreateAssetMenu]
[System.Serializable]
public class ConversationData : ScriptableObject
{
    // 会話用のデータ(山本)
    [Header("ストーリータイプ")]
    [SerializeField] private StoryType m_storyType = StoryType.None;
    public StoryType StoryType { get { return m_storyType; } }

    [Header("会話フェーズ")]
    [SerializeField] private TalkingFase m_talkingFase = TalkingFase.Fase1;
    public TalkingFase TalkingFase { get { return m_talkingFase; } set { m_talkingFase = value; } }

    [Header("会話が登録されているローカライズテーブル")]
    [SerializeField] private UnityEngine.Localization.LocalizedStringTable m_table = null;
    public UnityEngine.Localization.LocalizedStringTable Table { get { return m_table; } }

    [Header("話者の名前を表示するかどうか")]
    [SerializeField] private bool m_isSpeakerNameWindow = true;
    public bool IsSpeakerNameWindow { get { return m_isSpeakerNameWindow; } }

    [Header("話者の名前")]
    [SerializeField] private UnityEngine.Localization.LocalizedString m_speakerName;
    public UnityEngine.Localization.LocalizedString SpeakerName { get { return m_speakerName; } }


    [Header("会話用テキストのリスト2")]
    [SerializeField]
    private SerializableDictionary<TalkingFase, List<ConversationInformaton>> m_text2List;
    public List<ConversationInformaton> GetLocalizedStringList2(TalkingFase _talkingFase)
    {
        if (m_text2List.ContainsKey(_talkingFase) == false)
        {
            Debug.LogError("指定されたフェーズが登録されてません。");
        }

        return m_text2List[_talkingFase];
    }

    [Header("報酬リスト")]
    [SerializeField]
    private SerializableDictionary<TalkingFase, BaseRewardChallengeData> m_rewardList;

    public SerializableDictionary<TalkingFase, BaseRewardChallengeData> RewardList { get { return m_rewardList; } }

    [Header("NPCプレハブ")]
    [SerializeField]
    private GameObject m_NPCPrefab = null;
    public GameObject NPCPrefab { get { return m_NPCPrefab; } }

}


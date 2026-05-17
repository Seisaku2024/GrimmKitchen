using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ConversationDataBaseManager : BaseManager<ConversationDataBaseManager>
{
    // 会話データベースのマネージャー

    [Header("会話データベース")]
    [SerializeField] private ConversationDataBase m_conversationDataBase = null;
    public ConversationDataBase ConversationDataBase => m_conversationDataBase;

    // 話者タイプから適切なデータを返す
    public ConversationData GetConvaersationData(StoryType _storyType)
    {
        if (m_conversationDataBase == null)
        {
            Debug.LogError("会話データベースが登録されていません。");
            return null;
        }

        // 返すデータ
        ConversationData conversationData = null;

        foreach (var data in m_conversationDataBase.ConversationDataList)
        {
            if (data.StoryType == _storyType)
            {
                conversationData = data;
                break;
            }
        }

        if (conversationData == null)
        {
            Debug.LogError("指定した話者タイプは登録されてません。");
            return null;
        }

        return conversationData;

    }

    // 指定話者タイプの会話データに指定したフェーズをセットする
    public void SetTalkingFase(StoryType _storyType,TalkingFase _talkingFase)
    {
        if (m_conversationDataBase == null)
        {
            Debug.LogError("会話データベースが登録されていません。");
        }

        foreach (var data in m_conversationDataBase.ConversationDataList)
        {
            if (data.StoryType == _storyType)
            {
                data.TalkingFase = _talkingFase;
                break;
            }
        }

    }


}

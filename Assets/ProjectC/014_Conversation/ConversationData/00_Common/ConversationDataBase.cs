using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConversationDataBase : ScriptableObject
{
    // 会話データのデータベース（山本）
    [Header("会話データの種類")]
    [SerializeField]private List<ConversationData> m_conversationDataList;

    public List<ConversationData> ConversationDataList { get { return m_conversationDataList; } }

}

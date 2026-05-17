using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ProximityCreateConversationWindow : ProximityCreateWindow
{
    // 近くでactionボタンを押すことでシリアライズしたウィンドウを作成する(会話ウィンドウ)
    // BoxColliderが必要です

    [Header("作成する会話用ウィンドウ")]
    [SerializeField]
    protected ConversationWindowController m_conversationWindowController = null;

    [Header("話者タイプ")]
    [SerializeField] private SpeakerType m_speakerType = SpeakerType.None;

    [Header("ストーリータイプ")]
    [SerializeField] private StoryType m_storyType=StoryType.None;  

    [Header("カメラターゲット")]
    [SerializeField] private Transform m_targetTrans = null;

    private bool m_isSpeak = false;

    // ウィンドウを作成
    public override async UniTask CreateWindow()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {

            if (m_isSpeak)
            {
                return;
            }

            if (m_isCreate == false) return;
            if (m_createWindow != null) return;

            if (m_conversationWindowController == null)
            {
                Debug.LogError("windowContorollerがシリアライズされていません");
                return;
            }

            // 作成
            m_conversationWindow = Instantiate(m_conversationWindowController);
            await m_conversationWindow.CreateConversationWindow<BaseWindow>(m_speakerType,m_storyType, m_targetTrans);
            cancelToken.ThrowIfCancellationRequested();
            if (m_conversationWindow != null) Destroy(m_conversationWindow.gameObject);

            m_isSpeak = true;

            if (gameObject.TryGetComponent(out ProximitySwitchShowActionUI showActionUI))
            {
                showActionUI.gameObject.SetActive(false);
            }

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}

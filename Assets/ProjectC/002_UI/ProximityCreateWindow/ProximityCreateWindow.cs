using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;


[RequireComponent(typeof(BoxCollider))]
public class ProximityCreateWindow : MonoBehaviour
{

    // 近くでactionボタンを押すことでシリアライズしたウィンドウを作成する
    // BoxColliderが必要です
    // 制作者　田内


    [Header("作成するウィンドウ")]
    [SerializeField]
    protected WindowController m_windowController = null;


    [Header("タグ")]
    [SerializeField]
    protected string m_tag = "Player";


    // ウィンドウが作成できるかどうか
    protected bool m_isCreate = false;

    public bool IsCreate { get { return m_isCreate; } }

    // 作成したウィンドウ保持
    protected WindowController m_createWindow = null;
    // nullか確認できるようにする用（山本）
    public WindowController GetCreateWindow { get { return m_createWindow; } }

    // 会話用のウィンドウ保持するため（山本）
    protected ConversationWindowController m_conversationWindow;
    public ConversationWindowController GetConversationWindow => m_conversationWindow;


    private void Start()
    {
        // MetaAIに登録
        IMetaAI<ProximityCreateWindow>.Instance.RegisterObject(this);
    }


    // ウィンドウを作成
    public virtual async UniTask CreateWindow()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {

            if (m_isCreate == false) return;
            if (m_createWindow != null) return;

            if (m_windowController == null)
            {
                Debug.LogError("windowContorollerがシリアライズされていません");
                return;
            }

            // 作成
            m_createWindow = Instantiate(m_windowController);
            await m_createWindow.CreateWindow<BaseWindow>();
            cancelToken.ThrowIfCancellationRequested();
            if (m_createWindow != null) Destroy(m_createWindow.gameObject);

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    virtual protected void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(m_tag))
        {
            m_isCreate = true;
        }
    }


    virtual protected void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(m_tag))
        {
            m_isCreate = false;
        }
    }

}

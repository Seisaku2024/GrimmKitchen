using Unity.Cinemachine;
using Cysharp.Threading.Tasks;
using Speaker;
using System;
using UnityEngine;

public class ConversationWindowController : WindowController
{
    // 会話ウィンドウ用のコントローラー（山本）

    // カメラ切り替えるかどうか
    private bool m_bChangeCamera = true;
    public bool ChangeCamera { get { return m_bChangeCamera; } set { m_bChangeCamera = value; } }

    public async UniTask<WindowType> CreateConversationWindow<WindowType>
        (
        SpeakerType _speakerType = SpeakerType.None,
        StoryType _storyType = StoryType.None,
        Transform _lookAtTransform = null,
        bool _bSelef = false,
        Action startTrasitionAction = null,
        Action endTrasitionAction = null,
        float startTrasitionTime = 2.0f,
        float endTrasitionTime = 2.0f,
        System.Func<WindowType, UniTask> onBeforeInitialize = null) where WindowType : BaseWindow
    {
        if (m_createWindowObject != null)
        {
            // 作成済みであればここ
            return null;
        }

        if (m_window == null)
        {
            Debug.LogError("作成したいウィンドウが登録されていません");
            return null;
        }

        try
        {
            // ウィンドウを作成
            m_createWindowObject = Instantiate(m_window, transform);

            // 子オブジェクトからコンポーネントを取得
            var window = m_createWindowObject.GetComponentInChildren<ConversationWindow>();
            if (window == null)
            {
                Debug.LogError("ConversationWindowコンポーネントがアタッチされていません");
                DestroyWindow();
                return null;
            }

            // 会話用ウィンドウに話者タイプをセットする
            window.SpeakerType = _speakerType;
            window.StoryType = _storyType;

            // 会話用のカメラのLookAtに親のTransformをセット

            window.SetConversationCameraLookAtTransform = _lookAtTransform;
            window.SetConversationCameraFollowTransform = _lookAtTransform.root.transform;

            // カメラ切り替えるかどうか
            window.ChangeCameFlg = m_bChangeCamera;

            var cancelToken = window.GetCancellationTokenOnDestroy();

            // 非表示
            m_createWindowObject.SetActive(false);

            // 外部処理の実行
            if (onBeforeInitialize != null)
            {
                if (window is WindowType == false)
                {
                    Debug.LogError("キャストが行えません");
                }
                else
                {
                    await onBeforeInitialize(window as WindowType);
                    cancelToken.ThrowIfCancellationRequested();
                }
            }

            // 初期化処理
            await window.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();


            // 表示
            m_createWindowObject.SetActive(true);

            // トランジション開始(幕開け)
            TransitionController transitionController = CutSceneManager.instance.TransitionController;
            if (transitionController)
            {
                transitionController.StartTransition(0.0f, startTrasitionTime,startTrasitionAction);
            }


            // 表示処理
            await window.OnShow();
            cancelToken.ThrowIfCancellationRequested();


            // 自分で操作する場合は通さない
            if (_bSelef == false)
            {
                // 本処理 
                await window.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // トランジション開始(〆)
                if (transitionController)
                {
                    transitionController.StartTransition(0.0f, endTrasitionTime,endTrasitionAction);
                }

                // 非表示処理
                await window.OnClose();
                cancelToken.ThrowIfCancellationRequested();


                // 終了処理
                await window.OnDestroy();
                cancelToken.ThrowIfCancellationRequested();

            }

            // ウィンドウを返す
            return window as WindowType;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);

            // エラーが出た場合ウィンドウを削除する
            DestroyWindow();
        }

        return null;
    }



    

}

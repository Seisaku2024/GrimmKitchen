using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using NaughtyAttributes;

using Cysharp.Threading.Tasks;

// 制作者(田内)
// キャンバスに割り当ててください

public class WindowController : MonoBehaviour
{

    protected enum CreateWindowType
    {
        Self,   // 自身で作成する
        Start,  // Startメソッドで一度だけ作成する
    }


    [Header("作成タイプ")]
    [SerializeField]
    protected CreateWindowType m_createWindowType = CreateWindowType.Self;

    [Header("作成するウィンドウ")]
    [SerializeField]
    protected GameObject m_window = null;


    //====================
    // 作成したウィンドウ

    protected GameObject m_createWindowObject = null;

    //================================================================
    //                        実行処理
    //================================================================

    private void Start()
    {
        _ = UpdateStart();
    }


    /// <summary>
    /// 外部でウィンドウを作成する
    /// </summary>
    public async UniTask CreateWindow()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ウィンドウを作成
            await CreateWindow<BaseWindow>();
            cancelToken.ThrowIfCancellationRequested();

            await UniTask.CompletedTask;
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }



    /// <summary>
    /// このウィンドウコントローラーはウィンドウを作成しているかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsCreateWindow()
    {
        if (m_createWindowObject != null) return true;
        else return false;
    }


    #region
    /// <typeparam name="WindowType"></typeparam>
    /// <param name="_bSelef">作成までは行い、その後の動作は自身で操作する</param>
    /// <param name="_isType">停止などの処理を行うかどうか</param>
    /// <param name="onBeforeInitialize">独自の処理を追加する場合</param>
    #endregion
    public async UniTask<WindowType> CreateWindow<WindowType>(bool _bSelef = false, System.Func<WindowType, UniTask> onBeforeInitialize = null) where WindowType : BaseWindow
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
            var window = m_createWindowObject.GetComponentInChildren<BaseWindow>();
            if (window == null)
            {
                Debug.LogError("BaseWindowコンポーネントがアタッチされていません");
                DestroyWindow();
                return null;
            }


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


            // 表示処理
            await window.OnShow();
            cancelToken.ThrowIfCancellationRequested();


            // 自分で操作する場合は通さない
            if (_bSelef == false)
            {
                // 本処理 
                await window.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

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


    protected async UniTask UpdateStart()
    {
        if (m_createWindowType != CreateWindowType.Start) return;

        // ウィンドウを作成
        await CreateWindow<BaseWindow>();

        await UniTask.CompletedTask;
    }

    // ウィンドウを削除する
    protected void DestroyWindow()
    {
        if (m_createWindowObject == null) return;
        Destroy(m_createWindowObject);
    }


}

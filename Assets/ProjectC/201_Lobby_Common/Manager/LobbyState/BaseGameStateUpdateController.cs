using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using UniRx;

public class BaseGameStateUpdateController<T> : BaseManager<T> where T : MonoBehaviour
{
    // 制作者 田内
    // 状況を判断して行う処理を変更するマネージャー

    protected ReactiveProperty<int> m_currentStateDevice = new(0);

    public System.IObservable<int> CurrentStateDevice
    {
        get { return m_currentStateDevice; }
    }

    [Header("実行処理リスト")]
    [SerializeField]
    private List<BaseGameStateUpdate> m_stateUpdateList = new();

    // 実行中の処理
    private BaseGameStateUpdate m_currentStateUpdate = null;

    [Header("アクティブの変更（暫定処理）")]
    [SerializeField]
    private bool m_isActive = false;

    //===================================================
    //                   実行処理
    //===================================================


    protected override void StartInstance()
    {
        gameObject.SetActive(m_isActive);
    }

    private void Start()
    {
        StartStateUpdate().Forget();
    }

    private void OnDestroy()
    {
        // メモリリーク対策
        m_currentStateDevice.Dispose();
    }


    public bool IsState(int _state)
    {
        if (m_currentStateDevice.Value == _state) return true;
        return false;
    }


    virtual protected async UniTask StartStateUpdate()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 初期状態をセットする
            SetInitializeState();

            await ChangeState(m_currentStateDevice.Value);
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;

    }


    // 初期ステートをセット
    virtual protected void SetInitializeState()
    {
        #region TEMPLATE
        // どのシーンからきたか
        var beforeName = SceneNameManager.instance.BeforeSceneName;

        // このシーンから開始した場合は確認用なので無視する
        if (beforeName == "None") return;
        #endregion
    }

    virtual protected void SetState(string _sceneName,int _state)
    {
        
    }


    private async UniTask ChangeState(int _state)
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 終了処理
            await ExitState();
            cancelToken.ThrowIfCancellationRequested();

            // 削除処理
            await DestroyState();
            cancelToken.ThrowIfCancellationRequested();

            // ステートを変更・実行開始
            m_currentStateDevice.Value = _state;
            await SetStateUpdate();

            // 初期化処理
            await InitializeState();
            cancelToken.ThrowIfCancellationRequested();

            // 実行処理
            await UpdateState();
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    /// <summary>
    /// 初期処理
    /// </summary>
    private async UniTask InitializeState()
    {
        if (m_currentStateUpdate == null) return;

        await m_currentStateUpdate.OnInitialize();

        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 毎フレーム実行される処理
    /// </summary>
    private async UniTask UpdateState()
    {
        if (m_currentStateUpdate == null) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken != null)
            {
                // 実行処理
                await m_currentStateUpdate.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // ステート処理が終了すれば
                if (m_currentStateUpdate.IsEnd == true)
                {
                    // ステートを変更
                    await ChangeState(m_currentStateUpdate.GetNextState());
                    return;
                }

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }



    /// <summary>
    /// 終了処理
    /// </summary>
    private async UniTask ExitState()
    {
        if (m_currentStateUpdate == null) return;

        await m_currentStateUpdate.OnExit();

        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 削除処理
    /// </summary>
    private async UniTask DestroyState()
    {
        if (m_currentStateUpdate == null) return;

        await m_currentStateUpdate.OnDestroy();

        await UniTask.CompletedTask;
    }



    /// <summary>
    /// 一致するステートの処理をセット
    /// </summary>
    protected async UniTask SetStateUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // 作成済みのステート処理があれば
            if (m_currentStateUpdate != null)
            {
                // 削除処理
                await DestroyState();
                cancelToken.ThrowIfCancellationRequested();

                m_currentStateUpdate = null;
            }

            // 実行終了であれば
            if (m_currentStateDevice.Value == -1) return;

            foreach (var update in m_stateUpdateList)
            {
                if (update == null) return;

                if (update.GetState() == m_currentStateDevice.Value)
                {
                    // 新しいステート処理を作成・セット
                    m_currentStateUpdate = Instantiate(update);
                    return;
                }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        Debug.LogError(m_currentStateDevice + "対応する処理が存在しません");

    }


}

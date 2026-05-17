using UnityEngine;
using Cysharp.Threading.Tasks;

public class SelectChallengeController : MonoBehaviour
{
    // 制作者 田内
    // 経営で使用するチャレンジを選択するコントローラー

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    [Header("チャレンジポップアップウィンドウ")]
    [SerializeField]
    private WindowController m_judgeWindow = null;

    private bool m_selected = false;

    //===============================================
    //                  実行処理
    //===============================================

    /// <summary>
    /// 実行処理
    /// </summary>
    public async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await SetChallenge();
            cancelToken.ThrowIfCancellationRequested();


        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    /// <summary>
    /// チャレンジIDを更新する
    /// </summary>
    private async UniTask SetChallenge()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        // 選択されれば
        if (m_selectUIController.IsPress == false) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            var current = m_selectUIController.CurrentSelectUI;
            if (current == null) return;

            // チャレンジスロットデータであれば
            if (current.TryGetComponent<ChallengeSlotData>(out var slotData))
            {
                // 選択したチャレンジデータ
                var challengeData = slotData.ChallengeData;

                //// ジャッジウィンドウを作成
                //if (await CreateWIndow(challengeData) == false)
                //{
                //    // ジャッジ判定がfalseならセットしない
                //    cancelToken.ThrowIfCancellationRequested();
                //    return;
                //}

                await UniTask.DelayFrame(2);

                m_selected = true;

                // チャレンジIDを更新
                ChallengeManager.instance.SetChallengeID(challengeData.ChallengeID);

                return;
            }

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    /// <summary>
    /// ジャッジウィンドウを作成
    /// </summary>
    private async UniTask<bool> CreateWIndow(ChallengeData _data)
    {
        // ジャッジウィンドウがなければ即座にセット
        if (m_judgeWindow == null) return true;
        if (_data == null) return false;

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            var controller = Instantiate(m_judgeWindow);
            var window = await controller.CreateWindow<ChallengeJudgeWindow>(true, async _ =>
             {
                 // データをセット
                 _.SetData(_data);
                 await UniTask.CompletedTask;
             });
            cancelToken.ThrowIfCancellationRequested();

            bool judge = await window.OnSelfUpdate();
            cancelToken.ThrowIfCancellationRequested();

            await window.OnClose();
            cancelToken.ThrowIfCancellationRequested();

            await window.OnDestroy();
            cancelToken.ThrowIfCancellationRequested();

            if (controller != null) Destroy(window.gameObject);

            return judge;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
            return true;
        }
    }

    public bool IsClose()
    {
        return m_selected;
    }


}

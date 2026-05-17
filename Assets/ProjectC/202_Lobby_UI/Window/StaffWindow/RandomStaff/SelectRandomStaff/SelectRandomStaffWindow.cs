using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRandomStaffWindow : BaseWindow
{
    // 制作者 田内
    // ランダムスタッフコントローラーで取得したスタッフ選択ウィンドウ

    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スタッフ選択コントローラー")]
    [SerializeField]
    private SelectKeepRandomStaffController m_selectRandomStaffController = null;

    [Header("スタッフスロット作成")]
    [SerializeField]
    private CreateRandomStaffSlotList m_createRandomStaffSlotList = null;

    [Header("スタッフ説明文")]
    [SerializeField]
    private ChangeStaffStatusDataDescription m_changeStaffStatusDataDescription = null;

    [Header("スクロールビュー")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("確認用ウィンドウ")]
    [SerializeField]
    private WindowController m_judgeWindowController = null;

    // 追加（吉田）
    [Header("更新処理が必要なスクリプト")]
    [SerializeField]
    private List<WindowUpdateBase> m_windowUpdateBaseList = new();


    //===========================================================
    //                        実行処理
    //===========================================================

    /// <summary>
    /// データをセットする
    /// このウィンドウを使用する場合、絶対にデータをセットすること
    /// </summary>
    public void SetData(RandomStaffController _controller)
    {
        #region nullチェック
        if (m_createRandomStaffSlotList == null)
        {
            Debug.LogError("CreateRandomStaffSlotListがシリアライズされていません");
            return;
        }
        #endregion

        m_createRandomStaffSlotList.SetData(_controller);
    }


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_createRandomStaffSlotList == null)
        {
            Debug.LogError("CreateRandomStaffSlotListがシリアライズされていません");
            return;
        }
        if (m_changeStaffStatusDataDescription == null)
        {
            Debug.LogError("ChangeStaffStatusDataDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // スロット初期化・作成
            await m_createRandomStaffSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // 選択コントローラー初期化
            await m_selectRandomStaffController.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            m_changeStaffStatusDataDescription.OnInitialize();

            // 追加（吉田）
            foreach (var window in m_windowUpdateBaseList)
            {
                window.OnInitialize();
            }


        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }


    }

    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_selectRandomStaffController == null)
        {
            Debug.LogError("SelectRandomStaffControllerがシリアライズされていません");
            return;
        }
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPositionがシリアライズされていません");
            return;
        }
        if (m_changeStaffStatusDataDescription == null)
        {
            Debug.LogError("ChangeStaffStatusDataDescriptionがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.destroyCancellationToken;
        try
        {

            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択コントローラーの実行処理
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // スクロールビューの座標更新
                m_changeScrollViewPosition.OnUpdate();

                // 説明文の更新
                m_changeStaffStatusDataDescription.OnUpdate();

                // スタッフを選択するコントローラーの実行処理
                await m_selectRandomStaffController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // 追加を行う
                if (await m_selectRandomStaffController.AddManagerData()) return;
                cancelToken.ThrowIfCancellationRequested();

                // 追加（吉田）
                foreach (var window in m_windowUpdateBaseList)
                {
                    window.OnUpdate();
                }

                // 後実行処理
                m_selectUIController.OnLateUpdate();
                m_selectRandomStaffController.OnLateUpdate();


                // ウィンドウを閉じる
                if (IsClose())
                {
                    bool close = await CheckCloseWindow();
                    cancelToken.ThrowIfCancellationRequested();

                    if (close) return;
                }

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // 本当に閉じるか確認するウィンドウ
    private async UniTask<bool> CheckCloseWindow()
    {
        #region nullチェック
        if (m_judgeWindowController == null)
        {
            Debug.LogError("m_judgeWindowControllerがシリアライズされていません");
            return false;
        }
        #endregion

        var cancelToken = this.destroyCancellationToken;

        try
        {
            var controller = Instantiate(m_judgeWindowController);
            var createiWndow = await controller.CreateWindow<JudgeWindow>(true);
            cancelToken.ThrowIfCancellationRequested();


            // 実行処理 返信結果を取得
            bool judge = await createiWndow.OnSelfUpdate();
            cancelToken.ThrowIfCancellationRequested();

            await createiWndow.OnClose();
            cancelToken.ThrowIfCancellationRequested();

            await createiWndow.OnDestroy();
            cancelToken.ThrowIfCancellationRequested();

            if (controller != null) Destroy(controller.gameObject);

            return judge;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }


        await UniTask.CompletedTask;
        return false;
    }


}

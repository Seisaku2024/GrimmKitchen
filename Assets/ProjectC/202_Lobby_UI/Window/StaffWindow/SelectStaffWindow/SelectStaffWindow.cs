using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ButtonInfo;

public class SelectStaffWindow : BaseWindow
{
    // 制作者 田内
    // スタッフを選択するウィンドウ

    [Header("チュートリアルウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_tutorialWindowController = null;


    [Header("UI選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    [Header("スタッフスロット作成（ホール）")]// (追加：吉田)
    [SerializeField]
    private CreateStaffPointSlotList m_createHallStaffPointSlotList = null;
    [Header("スタッフスロット作成（シェフ）")]// (追加：吉田)
    [SerializeField]
    private CreateStaffPointSlotList m_createChefStaffPointSlotList = null;


    [Header("スタッフセットコントローラー")]
    [SerializeField]
    private SetStaffController m_setStaffController = null;

    [Header("スタッフDescriptionコントローラー")]
    [SerializeField]
    private ChangeStaffPointDataDescription m_staffPointDescription = null;

    [Header("スタッフDescriptionコントローラー")]
    [SerializeField]
    private AddStaffListToSelectController m_addStaffList = null;


    //===========================================================
    //                      実行処理
    //===========================================================

    // この画面が停止したロビーのカメラ操作だけを、画面終了時に復元する。
    private readonly List<ManagementCameraController> m_pausedCameraControllers = new();

    private void OnEnable()
    {
        foreach (var cameraController in FindObjectsByType<ManagementCameraController>(FindObjectsSortMode.None))
        {
            if (cameraController.gameObject.scene != gameObject.scene || !cameraController.enabled)
            {
                continue;
            }

            m_pausedCameraControllers.Add(cameraController);
            cameraController.enabled = false;
        }
    }

    private void OnDisable()
    {
        foreach (var cameraController in m_pausedCameraControllers)
        {
            if (cameraController != null)
            {
                cameraController.enabled = true;
            }
        }

        m_pausedCameraControllers.Clear();
    }

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_createHallStaffPointSlotList == null)
        {
            Debug.LogError("CreateHallStaffPointSlotListがシリアライズされていません");
            return;
        }
        if (m_createChefStaffPointSlotList == null)
        {
            Debug.LogError("CreateChefStaffPointSlotListがシリアライズされていません");
            return;
        }
        if (m_staffPointDescription == null)
        {
            Debug.LogError("ChangeStaffPointDataDescriptionがシリアライズされていません");
            return;
        }
        if (m_addStaffList == null)
        {
            Debug.LogError("AddStaffListToSelectControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            // スロット作成
            await m_createHallStaffPointSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();
            await m_createChefStaffPointSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            m_addStaffList.OnInitialize();
            m_staffPointDescription.OnInitialize();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }


    }

    public override async UniTask OnUpdate()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return;
        }
        if (m_setStaffController == null)
        {
            Debug.LogError("SetStaffControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // チュートリアルを表示
            await CreateTutorialWindow();
            cancelToken.ThrowIfCancellationRequested();

            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択コントローラーを更新
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // スタッフを選択するコントローラー実行処理
                await m_setStaffController.OnUpdate();

                // 詳細情報を更新
                m_staffPointDescription.OnUpdate();

                // UI選択コントローラーを後更新
                m_selectUIController.OnLateUpdate();

                // スタッフを選択するコントローラー実行処理
                m_setStaffController.OnLateUpdate();

                // ウィンドウを閉じる
                if (IsClose())
                {
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

    }



    // チュートリアルウィンドウを表示
    private async UniTask CreateTutorialWindow()
    {
        #region nullチェック
        if (m_tutorialWindowController == null)
        {
            Debug.LogError("TutorialWindowControllerがシリアライズされていません");
            return;
        }
        #endregion
        // チュートリアルを表示
        var data = StoryProgressManager.instance.GetStoryProgressData(StoryProgressType.PlayManagement);
        if (data.AchieveFlg == false)
        {
            var controller = Instantiate(m_tutorialWindowController);
            await controller.CreateWindow<BaseWindow>();
            if (controller != null) Destroy(controller.gameObject);
        }

        await UniTask.CompletedTask;
    }
}

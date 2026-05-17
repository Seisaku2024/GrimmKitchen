using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaintsField;

public class PoseWindow : BaseWindow
{
    // ポーズ中のウィンドウ（吉田）

    [Header("Tabコントローラー")]
    [SerializeField]
    [RichLabel("Tab SelectUIController")]
    private SelectUIController m_selectTab = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("更新が必要なコンポーネント")]
    [SerializeField]
    private List<WindowUpdateBase> m_updateComponents = new List<WindowUpdateBase>();

    [Header("変更した音量を記録する")]
    [SerializeField]
    private VolumeSaveLoader m_saveLoader = null;

    //============================================================
    //                      実行処理
    //============================================================


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_selectTab == null)
        {
            Debug.LogError("selectUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();
            // スライダー調整
            m_selectTab.SetSliderNoSelectColor();
            cancelToken.ThrowIfCancellationRequested();

            foreach (var component in m_updateComponents)
            {
                component.OnInitialize();
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
        if (m_selectTab == null)
        {
            Debug.LogError("selectUIControllerがシリアライズされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // ウィンドウUI処理
                await m_selectTab.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();
                // 更新が必要なコンポーネント
                foreach (var component in m_updateComponents)
                {
                    component.OnUpdate();

                    await component.OnUpdateTask();
                    cancelToken.ThrowIfCancellationRequested();
                }

                // スクロール処理
                m_changeScrollViewPosition.OnUpdate();

                // ウィンドウUI後処理
                m_selectTab.OnLateUpdate();
                cancelToken.ThrowIfCancellationRequested();
                // 更新が必要なコンポーネント
                foreach (var component in m_updateComponents)
                {
                    component.OnLateUpdate();
                }

                // ウィンドウを閉じる
                if (IsClose())
                {
                    // 変更した音量を保存
                    m_saveLoader.SaveVolume();
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


}

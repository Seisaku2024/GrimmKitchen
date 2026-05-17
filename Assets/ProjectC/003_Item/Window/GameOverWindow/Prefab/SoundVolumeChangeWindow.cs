using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVolumeChange: BaseWindow
{
    // サウンド音量を変更するウィンドウ（山本）


    [Header("スロットコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("変更した音量を記録する")]
    [SerializeField]
    private VolumeSaveLoader m_saveLoader = null;

    //============================================================
    //                      実行処理
    //============================================================


    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_selectUIController == null)
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
            m_selectUIController.SetSliderNoSelectColor();
            cancelToken.ThrowIfCancellationRequested();


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
            Debug.LogError("SelectUIコントローラーがシリアライズされていません");
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
                await m_selectUIController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();


                // スライダー調整
                m_selectUIController.UpdateSlider();
                cancelToken.ThrowIfCancellationRequested();


                // ウィンドウUI後処理
                m_selectUIController.OnLateUpdate();
                cancelToken.ThrowIfCancellationRequested();

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

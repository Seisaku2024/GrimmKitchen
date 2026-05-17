using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 制作者 よしだ　
// インベントリ　と ステータス　を表示するウィンドウ
// 切り替えと、ウィンドウの更新・表示を行う
public class ActionPlayerInfoWindow : BaseWindow
{
    [Space(15)]

    [Header("タブ選択コントローラー")]
    [SerializeField]
    private SelectUIController m_selectTabController = null;

    [Header("Inventory")]
    [SerializeField]
    private InventoryWindow m_inventoryWindow = null;

    [Header("PlayerParameter")]
    [SerializeField]
    private PlayerParameterWindow m_playerParameterWindow = null;


    //[SerializeField]
    //private
    //[SerializeField]
    //private

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_selectTabController == null)
        {
            Debug.LogError("m_selectTabControllerコンポーネントがアタッチされていません");
            return;
        }
        if (m_inventoryWindow == null)
        {
            Debug.LogError("m_inventoryWindowコンポーネントがアタッチされていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            await m_inventoryWindow.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            await m_playerParameterWindow.OnInitialize();
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
        if (m_selectTabController == null)
        {
            Debug.LogError("m_selectTabControllerコンポーネントがアタッチされていません");
            return;
        }
        if (m_selectTabController == null)
        {
            Debug.LogError("m_selectTabControllerコンポーネントがアタッチされていません");
            return;
        }
        if (m_inventoryWindow == null)
        {
            Debug.LogError("m_inventoryWindowコンポーネントがアタッチされていません");
            return;
        }
        #endregion


        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // UI選択の更新
                await m_selectTabController.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                if(m_inventoryWindow.gameObject.activeSelf)
                {
                    await m_inventoryWindow.OnUpdate();
                    cancelToken.ThrowIfCancellationRequested();
                }
                else if(m_playerParameterWindow.gameObject.activeSelf)
                {
                    await m_playerParameterWindow.OnUpdate();
                    cancelToken.ThrowIfCancellationRequested();
                }

                // UI選択の後処理
                m_selectTabController.OnLateUpdate();

                // 閉じる
                if (IsClose()) return;

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

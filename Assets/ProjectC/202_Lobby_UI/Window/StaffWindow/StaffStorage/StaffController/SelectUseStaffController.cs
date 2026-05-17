using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using SelectUseStaffInfo;


public class SelectUseStaffController : MonoBehaviour
{
    // 制作者 田内
    // スタッフの使い道を決めるコントローラー

    [System.Serializable]
    private class CreateStaffValueControllerData
    {
        public SelectUseStaffID SelectUseStaffItemID = SelectUseStaffID.None;

        public WindowController WindowControlle = null;
    }

    [Header("SelectUIController")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    [Header("SelectStaffWindowコントローラー")]
    [SerializeField]
    private WindowController m_selectUseStaffWindowController = null;

    [Header("表示リスト")]
    [SerializeField]
    private List<SelectUseStaffID> m_selectUseStaffIDList = new();

    [Header("スタッフウィンドウ")]
    [SerializeField]
    private List<CreateStaffValueControllerData> m_createStaffValueControllerDataList = null;


    //==================================
    //          実行処理
    //==================================

    /// <summary>
    /// 実行処理
    /// </summary>
    public async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await SelectUI();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        await UniTask.CompletedTask;
    }



    private async UniTask SelectUI()
    {
        return;

        #region nullチェック
        if (m_selectUseStaffWindowController == null)
        {
            Debug.LogError("SelectUseStaffWindowController がシリアライズされていません");
            return;
        }
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーが登録されていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 選択したか ・ 選択しているUIがなければ
            if (m_selectUIController.IsPress == false) return;
            if (m_selectUIController.CurrentSelectUI == null) return;

            // 選択しているUIがアイテムスロットであれば
            var slotData = m_selectUIController.CurrentSelectUI.GetComponent<StaffStatusSlotData>();
            if (slotData == null || slotData.StaffStatusData == null) return;

            // ウィンドウを作成/動作
            var controller = Instantiate(m_selectUseStaffWindowController);
            var window = await controller.CreateWindow<SelectUseStaffWindow>(true, async _ =>
            {

                _.SetData(m_selectUseStaffIDList, slotData.StaffStatusData);

                await UniTask.CompletedTask;
            });
            cancelToken.ThrowIfCancellationRequested();

            // 処理
            SelectUseStaffID selectUseItemID = await window.OnUpdate();
            cancelToken.ThrowIfCancellationRequested();

            // 閉じる
            await window.OnClose();
            cancelToken.ThrowIfCancellationRequested();

            // 削除
            await window.OnDestroy();
            cancelToken.ThrowIfCancellationRequested();

            // ウィンドウコントローラーを削除
            if (controller != null) Destroy(controller.gameObject);

            // 使用
            await Use(selectUseItemID, slotData.StaffStatusData);
            cancelToken.ThrowIfCancellationRequested();

            return;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
            return;
        }
    }


    private async UniTask Use(SelectUseStaffID _id, StaffStatusData _data)
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            // 動作を決める
            switch (_id)
            {
                // 解雇
                case SelectUseStaffID.Dismissal:
                    {
                        bool judge = false;

                        #region Judge
                        foreach (var data in m_createStaffValueControllerDataList)
                        {
                            if (data.SelectUseStaffItemID != _id) continue;

                            if (data.WindowControlle == null)
                            {
                                Debug.LogError("WindowControllerがシリアライズされていません");
                                continue;
                            }

                            var controller = Instantiate(data.WindowControlle);
                            var window = await controller.CreateWindow<JudgeWindow>(_bSelef: true);
                            cancelToken.ThrowIfCancellationRequested();

                            judge = await window.OnSelfUpdate();
                            cancelToken.ThrowIfCancellationRequested();

                            await window.OnClose();
                            cancelToken.ThrowIfCancellationRequested();

                            await window.OnDestroy();
                            cancelToken.ThrowIfCancellationRequested();

                            if (controller != null) Destroy(controller.gameObject);
                            break;
                        }
                        #endregion

                        if (judge) StaffManager.instance.RemoveStaffStorage(_data);

                        break;
                    }

                case SelectUseStaffID.StatusUp:
                    {
                        foreach (var data in m_createStaffValueControllerDataList)
                        {
                            if (data.SelectUseStaffItemID != _id) continue;

                            if (data.WindowControlle == null)
                            {
                                Debug.LogError("WindowControllerがシリアライズされていません");
                                continue;
                            }

                            var controller = Instantiate(data.WindowControlle);
                            await controller.CreateWindow<StaffStatusUpWindow>(onBeforeInitialize: async _ =>
                               {
                                   _.SetData(_data);
                               });
                            cancelToken.ThrowIfCancellationRequested();

                            if (controller != null) Destroy(controller.gameObject);
                            break;
                        }
                        break;
                    }

                case SelectUseStaffID.Exit:
                    {
                        // 何もせず戻る
                        break;
                    }

                default:
                    {
                        Debug.Log("IDが当てはまりませんでした");
                        break;
                    }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}

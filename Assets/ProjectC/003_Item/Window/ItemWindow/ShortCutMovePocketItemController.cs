using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using Cysharp.Threading.Tasks;

public class ShortCutMovePocketItemController : MonoBehaviour
{
    // 制作者 田内
    // ショートカットでアイテムを移動させるコントローラー

    [Header("移動先")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.Inventory;

    [Header("移動ボタン")]
    [SerializeField]
    private InputActionButton m_inputActionButton = null;

    [Header("SelectUIController")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("移動時UI")]
    [SerializeField]
    private GameObject m_successUI = null;

    [SerializeField]
    private GameObject m_failedUI = null;

    //================================================
    //                  実行処理
    //================================================

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
    }


    private async UniTask SelectUI()
    {
        #region nullチェック
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーが登録されていません");
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_inputActionButton.IsInputActionTrriger())
            {
                // 選択したか ・ 選択しているUIがなければ
                if (m_selectUIController.CurrentSelectUI == null) return;

                // 選択しているUIがアイテムスロットであれば
                var slotData = m_selectUIController.CurrentSelectUI.GetComponent<PocketItemSlotData>();
                if (slotData == null || slotData.PocketItemData == null) return;
                PocketItemData data = slotData.PocketItemData;

                // 入る場合
                if (m_pocketType.GetPocketItemDataManager().IsInList())
                {
                    // 移動成功時
                    if (m_pocketType.GetPocketItemDataManager().AddItemList(data))
                    {
                        // 削除
                        slotData.PocketType.GetPocketItemDataManager().RemoveItemList(data);
                    }

                    // UI作成
                    if (m_successUI != null) Instantiate(m_successUI);
                }
                else
                {
                    // UI作成
                    if (m_failedUI != null) Instantiate(m_failedUI);
                }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
            return;
        }

        await UniTask.CompletedTask;
    }





}

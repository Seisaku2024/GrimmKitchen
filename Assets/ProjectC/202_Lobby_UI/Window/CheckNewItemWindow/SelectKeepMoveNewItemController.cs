using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using Cysharp.Threading.Tasks;

public class SelectKeepMoveNewItemController : SelectKeepUIController
{
    // 制作者 田内
    // ポケットアイテムデータをキープ

    [Header("追加ジャッジウィンドウ")]
    [SerializeField]
    private WindowController m_judgeWindow = null;


    //=========================================================
    //                  実行処理
    //=========================================================

    protected override bool IsSelectKeep(GameObject _ui)
    {
        if (_ui.TryGetComponent<PocketItemSlotData>(out var addSlotData) == false) return false;

        // 追加予定データリスト
        List<PocketItemData> list = new();
        list.Add(addSlotData.PocketItemData);
        foreach (var data in m_selectKeepObjectList)
        {
            if (data == null) continue;
            if (data.TryGetComponent<PocketItemSlotData>(out var slotData))
            {
                list.Add(slotData.PocketItemData);
            }
        }

        if (ManagementStorageManager.instance.IsInList(list) == false)
        {
            MaxSelectableUI();
            return false;
        }

        return true;
    }



    protected override void AddSelectKeepObject(GameObject _obj)
    {
        if (_obj.TryGetComponent<PocketItemSlotData>(out var slotData))
        {
            base.AddSelectKeepObject(_obj);
        }
    }

    protected override void RemoveSelectKeepObject(GameObject _obj)
    {
        if (_obj.TryGetComponent<PocketItemSlotData>(out var slotData))
        {
            base.RemoveSelectKeepObject(_obj);
        }
    }

    /// <summary>
    /// 移し替える
    /// </summary>
    public async UniTask<bool> Move()
    {

        if (m_selectKeepObjectList.Count <= 0)
        {
            // 何もなし
        }
        else
        {

            // 確認ウィンドウがあれば
            if (m_judgeWindow != null)
            {
                var cencelToken = this.GetCancellationTokenOnDestroy();
                try
                {
                    var controller = Instantiate(m_judgeWindow);
                    var window = await controller.CreateWindow<JudgeWindow>(_bSelef: true);
                    cencelToken.ThrowIfCancellationRequested();

                    bool judge = await window.OnSelfUpdate();
                    cencelToken.ThrowIfCancellationRequested();

                    await window.OnClose();
                    cencelToken.ThrowIfCancellationRequested();

                    await window.OnDestroy();
                    cencelToken.ThrowIfCancellationRequested();

                    if (judge == false) return false;

                }
                catch (System.OperationCanceledException ex)
                {
                    Debug.Log(ex);
                    return false;
                }
            }


            List<PocketItemData> itemList = new();
            // 移し替える
            foreach (var data in m_selectKeepObjectList)
            {
                if (data == null) continue;

                if (data.TryGetComponent<PocketItemSlotData>(out var slotData))
                {
                    if (slotData.PocketItemData == null) continue;
                    var pocketItemData = slotData.PocketItemData;
                    itemList.Add(new PocketItemData(pocketItemData.ItemTypeID, pocketItemData.ItemID, pocketItemData.Num));
                }
            }

            foreach (var data in itemList)
            {
                int num = data.Num;
                for (int i = 0; i < num; ++i)
                {
                    if (ManagementStorageManager.instance.AddItem(data.ItemTypeID, data.ItemID))
                    {
                        InventoryManager.instance.RemoveItem(data.ItemTypeID, data.ItemID);
                    }
                }
            }

        }




        // 新規獲得アイテムリスト初期化
        NewItemManager.instance.ItemDataRC.Clear();
        return true;
    }

}
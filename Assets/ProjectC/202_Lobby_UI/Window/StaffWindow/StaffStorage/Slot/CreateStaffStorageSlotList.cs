using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using System.Linq;

public class CreateStaffStorageSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // スタッフのスロットを作成

    protected enum SlotListType
    {
        MaxSlotDisplay, // スロットを最大まで作成する
        MinSlotDisplay, // スロットを最少で作成する
    }
    [Header("スロットリストの種類")]
    [SerializeField]
    protected SlotListType m_slotListType = SlotListType.MaxSlotDisplay;

    //======================================
    //          実行処理
    //======================================

    private void Start()
    {
        StaffManager.instance.StaffStorageRC.ObserveAdd().Subscribe(_ =>
        {
            SetSlotData(_.Value);

        }).AddTo(this);
    }


    protected override async UniTask CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットがシリアライズされていません");
            return;
        }


        // 現在所持しているアイテムリスト
        List<StaffStatusData> list = StaffManager.instance.StaffStorageRC.ToList();


        // 作成するスロットサイズ
        int size = GetSlotSize(list);

        // スタッフをストレージに入っている分作成する
        for (int i = 0; i < size; ++i)
        {
            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<StaffStatusSlotData>(out var slotData))
            {
                if (i < StaffManager.instance.StaffStorageRC.Count)
                {
                    // アイテムスロットのデータをセット
                    slotData.SetStaffStatusData(list[i]) ;
                }
                else
                {
                    // スロットのデータを初期化し、コンポーネントを削除
                    slotData.InitializeSlotData();
                }
            }
            else
            {
                Debug.LogError("StaffStatusSlotDataコンポーネントがアタッチされていません");
            }

            // リストに追加
            m_slotList.Add(slot);

            // UIcontrollerに追加
            AddSelectUIControler(slot);

        }

        await UniTask.CompletedTask;

    }

    protected int GetSlotSize(List<StaffStatusData> _list)
    {
        //　ポケットの最大サイズ
        int size = 0;

        switch (m_slotListType)
        {
            case SlotListType.MaxSlotDisplay:
                {
                    // 最大までスロット作成
                    size = (int)StaffManager.instance.MaxStaffStorage;
                    break;
                }

            case SlotListType.MinSlotDisplay:
                {
                    // 最小でスロット作成
                    size = _list.Count;
                    break;
                }
        }

        return size;
    }



    // 一番最新のスロットにデータをセットする
    private void SetSlotData(StaffStatusData _data)
    {
        foreach (var data in m_slotList)
        {
            if (data == null) continue;
            if (data.TryGetComponent<StaffStatusSlotData>(out var slotData))
            {
                if (slotData.StaffStatusData != null) continue;

                slotData.SetStaffStatusData(_data);

                break;
            }
        }
    }

}

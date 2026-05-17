using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class CreateManagementClearConditionChallengeSlotList : BaseCreateSlotList
{
    // 制作者　田内
    // 提供料理のスロットを作成する
    // マネージャーの提供料理リストに変更が加わるたびに自動的に更新

    [Tooltip("経営ゲーム用、提供可能数や売り上げ数などを更新する")]

    //====================================================
    //                  実行処理
    //====================================================

    private void Start()
    {
        ManagementGameDataManager.instance.ClearConditionChallengeDataRPRC.ObserveCountChanged().Subscribe(data =>
        {
            _ = CreateSlot();
        }).AddTo(this);
    }


    override protected async UniTask CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        DestroySlotList();


        // クリア条件
        foreach (var data in ManagementGameDataManager.instance.ClearConditionChallengeDataRPRC)
        {
            if (data == null) continue;

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<ManagementClearConditionChallengeSlotData>(out var slotData))
            {
                slotData.SetData(data.Value);
            }
            else
            {
                Debug.LogError("ClearConditionChallengeSlotコンポーネントがアタッチされていません");
            }

            m_slotList.Add(slot);

            // UIcontrollerに追加
            AddSelectUIControler(slot);
        }

        await UniTask.CompletedTask;
    }
}

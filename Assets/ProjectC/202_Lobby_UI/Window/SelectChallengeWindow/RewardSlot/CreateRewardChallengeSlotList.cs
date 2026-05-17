using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRewardChallengeSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // 報酬スロットを作成する


    //=======================
    // チャレンジデータ

    private ChallengeData m_challengeData = null;

    //==========================================================
    //                       実行処理
    //==========================================================


    public void SetData(ChallengeData _data)
    {
        m_challengeData = _data;
    }

    protected override async UniTask CreateSlotInstance()
    {

        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        // スロットを削除
        DestroySlotList();

        if (m_challengeData == null || m_challengeData.RewardChallengeDataList == null) return;

        // メインチャレンジ
        foreach (var data in m_challengeData.RewardChallengeDataList)
        {
            if (data == null) continue;
            if (data.IsDisplay == false) continue;

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<RewardChallengeSlotData>(out var slotData))
            {
                slotData.SetData(data);
            }
            else
            {
                Debug.LogError("RecipeItemSlotDataコンポーネントがアタッチされていません");
            }

            m_slotList.Add(slot);

            // UIcontrollerに追加
            AddSelectUIControler(slot);
        }


        await UniTask.CompletedTask;
    }

}

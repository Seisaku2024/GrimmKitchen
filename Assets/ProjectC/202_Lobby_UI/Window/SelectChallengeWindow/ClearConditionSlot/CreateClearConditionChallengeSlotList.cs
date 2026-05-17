using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class CreateClearConditionChallengeSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // クリア条件スロットの作成

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

        if (m_challengeData == null || m_challengeData.ClearConditionChallengeList == null) return;

        // メインチャレンジ
        foreach (var data in m_challengeData.ClearConditionChallengeList)
        {
            if (data == null) continue;

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<ClearConditionChallengeSlotData>(out var slotData))
            {
                slotData.SetData(data);
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

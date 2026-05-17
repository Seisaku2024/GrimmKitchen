using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using ChallengeInfo;

public class CreateChallengeSlotList : BaseCreateSlotList
{
    // 制作者 田内
    // チャレンジスロットデータを作成する



    [Header("作成するチャレンジ")]
    [SerializeField]
    private ChallengeType m_challengeType = ChallengeType.All;

    //============================================
    //              実行処理
    //============================================


    protected override async UniTask CreateSlotInstance()
    {
        if (m_slot == null)
        {
            Debug.LogError("作成するスロットが登録されていません");
            return;
        }

        List<ChallengeData> list = new();

        switch (m_challengeType)
        {
            case ChallengeType.Main:
                {
                    list.AddRange(GetMainChallengeDataList());
                    break;
                }

            case ChallengeType.Sub:
                {
                    list.AddRange(GetSubChallengeDataList());
                    break;
                }

            case ChallengeType.All:
                {
                    list.AddRange(GetMainChallengeDataList());
                    list.AddRange(GetSubChallengeDataList());
                    break;
                }
            default:
                {
                    // なにも作成しない
                    return;
                }

        }


        // チャレンジリスト
        foreach (var data in list)
        {
            if (data == null) continue;

            // プレイできなければ表示しない
            if (data.IsPlay() == false) continue;

            // 子オブジェクトにスロットを追加
            var slot = Instantiate(m_slot, transform);

            if (slot.TryGetComponent<ChallengeSlotData>(out var slotData))
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


    private List<ChallengeData> GetMainChallengeDataList()
    {
        List<ChallengeData> mainList = ChallengeDataBaseManager.instance.MainChallengeDataBase.ChallengeDataList;
        return mainList;
    }

    private List<ChallengeData> GetSubChallengeDataList()
    {
        List<ChallengeData> subList = ChallengeDataBaseManager.instance.SubChallengeDataBase.ChallengeDataList;
        return subList;
    }


}

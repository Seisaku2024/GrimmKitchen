using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(ChallengeDescription))]
public class ChangeCurrentChallengeDescription : MonoBehaviour
{
    // 制作者 田内
    // 現在選択中のレンジ説明文を表示する

    [Tooltip("選択中のチャレンジ説明文が表示される")]

    //========================================================
    // 説明文
    protected ChallengeDescription m_challengeDescription = null;

    //======================================================
    //                  実行処理
    //======================================================

   private void Start()
    {
        // 選択中のチャレンジが変更されれば再度更新する
        ChallengeManager.instance.ChallengeIDRP.Subscribe(_ =>
        {
            // 選択中のチャレンジデータを取得
            var data = ChallengeDataBaseManager.instance.GetData(ChallengeManager.instance.ChallengeID);

            // 説明文を更新
            if (m_challengeDescription == null) m_challengeDescription = gameObject.GetComponent<ChallengeDescription>();
            m_challengeDescription.UpdateDescription(data);
        }).AddTo(this);
    }

}

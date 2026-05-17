using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LobbyStateInfo;
using UniRx;

public class LobbyStateUI : MonoBehaviour
{
    // 制作者 田内
    // LobbyStateを基にアクティブをセット

    [Header("ステートリスト")]
    [SerializeField]
    private List<LobbyState> m_stateList = new();

    [Header("表示/非表示")]
    [SerializeField]
    private GameObject m_object = null;

    //=======================================================
    //                      実行処理
    //=======================================================

    private void Start()
    {
        LobbyStateUpdateManager.instance.CurrentStateDevice.Subscribe(device =>
        {
            // ステートに変更があった場合更新
            SetActiveUI();

        }).AddTo(this);
    }


    // アクティブセット
    private void SetActiveUI()
    {
        if (m_object == null) return;

        foreach (var state in m_stateList)
        {
            if (LobbyStateUpdateManager.instance.IsState((int)state))
            {
                m_object.SetActive(true);
                return;
            }
        }

        m_object.SetActive(false);
    }

}

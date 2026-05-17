using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LobbyStateInfo;
using UniRx;

[DefaultExecutionOrder(-999)]
public class SetActiveLobbyState : MonoBehaviour
{
    // 制作者 田内
    // ロビーステートを参考にアクティブをセット

    [Header("ステートリスト")]
    [SerializeField]
    private List<LobbyState> m_stateList = new();

    [Header("アクティブをセットするGameObject")]
    [SerializeField]
    private List<GameObject> m_objectList = new();


    //==================================================
    //              実行処理
    //==================================================


    void Start()
    {
        LobbyStateUpdateManager.instance.CurrentStateDevice.Subscribe(_ =>
        {

            bool flg = false;

            foreach (var state in m_stateList)
            {
                // 一致すれば
                if (LobbyStateUpdateManager.instance.IsState((int)state))
                {
                    flg = true;
                    break;
                }
            }

            SetActiveList(flg);

        });
    }

    private void SetActiveList(bool _active)
    {
        foreach(var obj in m_objectList)
        {
            if (obj == null) continue;
            obj.SetActive(_active);
        }
    }

}

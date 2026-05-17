using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManagementStateUpdateInfo;
using UniRx;

[DefaultExecutionOrder(-999)]
public class SetActiveMAnagementState : MonoBehaviour
{
    // 制作者 田内
    // 経営ステートを参考にアクティブをセット

    [Header("ステートリスト")]
    [SerializeField]
    private List<ManagementState> m_stateList = new();

    [Header("アクティブをセットするGameObject")]
    [SerializeField]
    private List<GameObject> m_objectList = new();


    //==================================================
    //              実行処理
    //==================================================


    void Start()
    {
        ManagementStateUpdateManager.instance.CurrentStateDevice.Subscribe(_ =>
        {

            bool flg = false;

            foreach (var state in m_stateList)
            {
                // 一致すれば
                if (ManagementStateUpdateManager.instance.IsState((int)state))
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
        foreach (var obj in m_objectList)
        {
            if (obj == null) continue;
            obj.SetActive(_active);
        }
    }

}

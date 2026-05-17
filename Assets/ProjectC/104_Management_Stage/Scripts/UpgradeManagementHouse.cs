using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeManagementHouseInfo;

public class UpgradeManagementHouse : MonoBehaviour
{
    // 制作者 田内
    // ステージ更新

    [Header("デフォルトオブジェクト(実行時に非アクティブ状態に変更)")]
    [SerializeField]
    private GameObject m_defaultHouse = null;

    [System.Serializable]
    private class UpgradeManagementHouseData
    {
        [Header("アップグレードID")]
        public UpgradeManagementHouseID UpgradeManagementHouseID = UpgradeManagementHouseID.Upgrade1;

        [Header("アクティブをセットするオブジェクト")]
        public GameObject StageObject = null;
    }

    [SerializeField]
    private List<UpgradeManagementHouseData> m_managementHouseDataList = new();

    //======================================
    //              実行処理
    //======================================

    private void Start()
    {
        if (m_defaultHouse != null)
        {
            m_defaultHouse.SetActive(false);
        }


        var targetID = ManagementDataManager.instance.UpgradeManagementHouseID;

        foreach (var data in m_managementHouseDataList)
        {
            if (data.UpgradeManagementHouseID == targetID)
            {
                if (data.StageObject != null)
                {
                    data.StageObject.SetActive(true);
                }
            }
            else
            {
                if (data.StageObject != null)
                {
                    data.StageObject.SetActive(false);
                }
            }
        }
    }
}

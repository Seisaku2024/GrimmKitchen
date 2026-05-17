using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManagementHouse : MonoBehaviour
{
    // 制作者 田内
    // シーンによってセットするオブジェクトを変更する

    [System.Serializable]
    private class Setting
    {
        [Header("シーン名")]
        public string SceneName = "GameScene";

        [Header("アクティブをセットするオブジェクト")]
        public List<GameObject> ObjectList = null;
    }

    [SerializeField]
    private List<Setting> m_settingList = new();

    //=========================================
    //              実行処理
    //=========================================

    private void Start()
    {
        Set();
    }


    private void Set()
    {
        foreach(var data in m_settingList)
        {
            if (data == null) continue;
            if (data.SceneName == SceneNameManager.instance.CurrentSceneName)
            {
                foreach(var obj in data.ObjectList)
                {
                    if (obj == null) continue;
                    obj.SetActive(true);
                }
            }
            else
            {
                foreach (var obj in data.ObjectList)
                {
                    if (obj == null) continue;
                    obj.SetActive(false);
                }
            }
        }
    }


}

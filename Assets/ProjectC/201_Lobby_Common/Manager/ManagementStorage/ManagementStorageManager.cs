using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using UniRx;

public class ManagementStorageManager : BasePocketItemDataController
{
    // 制作者 田内
    // 経営用ストレージ、このストレージから提供する料理を決める


    public static ManagementStorageManager instance;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (ManagementStorageManager)FindObjectOfType(typeof(ManagementStorageManager));

            DontDestroyOnLoad(gameObject);

            StartInstance();

        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }

    override protected void Load()
    {
        // 読み込み
        var data = PocketItemSaveLoader.Load(PocketType.ManagementStorage);
        m_itemDataRC = new ReactiveCollection<PocketItemData>(data.PocketItemDataList);

        base.Load();
    }

}

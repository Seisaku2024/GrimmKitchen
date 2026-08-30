using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PocketItemDataInfo;
using UniRx;

public class StorageManager : BasePocketItemDataController
{
    // 倉庫管理マネージャークラス(シングルトン)
    // 制作者　田内

    //========================
    // シングルトン


    public static StorageManager instance;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = FindAnyObjectByType<StorageManager>();

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
        var data = PocketItemSaveLoader.Load(PocketType.Storage);
        m_itemDataRC = new ReactiveCollection<PocketItemData>(data.PocketItemDataList);

        base.Load();
    }

}

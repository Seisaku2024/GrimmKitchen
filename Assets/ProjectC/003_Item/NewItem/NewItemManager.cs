using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using PocketItemDataInfo;

public class NewItemManager : BasePocketItemDataController
{
    // 新規獲得アイテムを保持しておくマネージャー(シングルトン)
    // 制作者　田内

    public static NewItemManager instance;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (NewItemManager)FindObjectOfType(typeof(NewItemManager));

            DontDestroyOnLoad(gameObject);

            StartInstance();

        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }

}

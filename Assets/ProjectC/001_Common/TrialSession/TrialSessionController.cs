using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialSessionController : MonoBehaviour
{
    // 制作者　田内
    // 試遊会用初期化コントローラー

    private enum InitializeType
    {
        Enable,
        Disable,
    }


    [Header("ゲームデータ初期化")]
    [SerializeField]
    private InitializeType m_gameDataType = InitializeType.Enable;

    [Header("ポケットアイテムデータ初期化")]
    [SerializeField]
    private InitializeType m_pocketItemDataType = InitializeType.Enable;

    [Header("スタッフデータ初期化")]
    [SerializeField]
    private InitializeType m_staffDataType = InitializeType.Enable;

    [Header("スタッフポイントデータ初期化")]
    [SerializeField]
    private InitializeType m_staffPointDataType = InitializeType.Enable;

    [Header("提供料理初期化")]
    [SerializeField]
    private InitializeType m_provideFoodDataType = InitializeType.Enable;

    //=================================================================
    //                      実行処理
    //=================================================================

    /// <summary>
    /// 試遊会用初期化
    /// </summary>
    public void InitializeTrialSession()
    {
        InitializeGameData();

        InitializePocketItemData();

        InitializeStaffData();

        InitializeStaffPointData();

        InitializeProvideFoodData();


        // 一旦削除するのもあり？

        /*
        GameDataManager.instance.DeleteInstance();

        StaffManager.instance.DeleteInstance();

        ManagementProvideFoodManager.instance.DeleteInstance();
        */

    }

    // ポケットアイテムデータ初期化
    private void InitializeGameData()
    {
        switch (m_gameDataType)
        {
            case InitializeType.Enable:
                {
                    ManagementDataManager.instance.OnInitialize();
                    break;
                }
        }
    }


    // ポケットアイテムデータ初期化
    private void InitializePocketItemData()
    {
        switch (m_pocketItemDataType)
        {
            case InitializeType.Enable:
                {
                    // 全てのポケットアイテムデータ初期化
                    BasePocketItemDataController.OnInitialize();
                    break;
                }
        }
    }

    // スタッフデータ初期化
    private void InitializeStaffData()
    {
        switch (m_staffDataType)
        {
            case InitializeType.Enable:
                {
                    // スタッフ初期化
                    StaffManager.instance.OnInitialize();
                    break;
                }
        }
    }

    // スタッフポイントデータ初期化
    private void InitializeStaffPointData()
    {
        switch (m_staffPointDataType)
        {
            case InitializeType.Enable:
                {
                    // スタッフ初期化
                    StaffManager.instance.OnInitializeStaffPointData();
                    break;
                }
        }
    }

    // 提供料理データ初期化
    private void InitializeProvideFoodData()
    {
        switch (m_provideFoodDataType)
        {
            case InitializeType.Enable:
                {
                    // スタッフ初期化
                    ProvideFoodManager.instance.OnInitialize();
                    break;
                }
        }
    }

}

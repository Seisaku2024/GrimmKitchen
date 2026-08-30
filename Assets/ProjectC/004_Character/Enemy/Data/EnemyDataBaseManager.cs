using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using SelectUseItemInfo;

[DefaultExecutionOrder(-100)]
public class EnemyDataBaseManager : BaseManager<EnemyDataBaseManager>
{
    [Header("敵のデータベース")]
    [SerializeField]
    private EnemyDataBase m_dataBase = null;

    public EnemyDataBase DataBase => m_dataBase;

    // ステージレベル等を反映したステータス
    // 実際に使うのはこっち
    private List<EnemyData> m_useEnemyData = new();


    public EnemyData GetEnemyData(EnemyID enemyID)
    {
        foreach (var data in m_useEnemyData)
        {
            if (data == null)
            {
                continue;
            }

            if (data.EnemyID == enemyID)
            {
                return data;
            }
        }

        Debug.LogError(
            $"{enemyID} この敵のデータベースは登録されていません"
        );

        return null;
    }


    public void ChangeEnemyLevel(StageEnemyStatus stageStatus)
    {
        foreach (var data in m_useEnemyData)
        {
            if (data == null)
            {
                continue;
            }

            EnemyData originalData =
                SearchDataID(
                    m_dataBase.OriginalEnemyDataBaseList,
                    data.EnemyID
                );

            if (originalData == null)
            {
                Debug.LogError(
                    $"{data.EnemyID} の元EnemyDataが見つかりません"
                );

                continue;
            }

            data.ChangeEnemyLevel(
                stageStatus,
                originalData
            );
        }
    }


    public void Start()
    {
        m_useEnemyData.Clear();

        if (m_dataBase == null)
        {
            Debug.LogError(
                "EnemyDataBaseManager: m_dataBase が設定されていません"
            );

            return;
        }

        if (m_dataBase.OriginalEnemyDataBaseList == null)
        {
            Debug.LogError(
                "EnemyDataBaseManager: OriginalEnemyDataBaseList が null です"
            );

            return;
        }

        foreach (var data in m_dataBase.OriginalEnemyDataBaseList)
        {
            if (data == null)
            {
                continue;
            }

            EnemyData copiedData =
                EnemyData.CreateCopy(data);

            if (copiedData == null)
            {
                continue;
            }

            m_useEnemyData.Add(copiedData);
        }
    }


    private EnemyData SearchDataID(
        List<EnemyData> list,
        EnemyID id)
    {
        if (list == null)
        {
            return null;
        }

        foreach (var data in list)
        {
            if (data == null)
            {
                continue;
            }

            if (data.EnemyID == id)
            {
                return data;
            }
        }

        return null;
    }
}
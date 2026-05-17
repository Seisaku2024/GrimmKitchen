using System.Collections;
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

    public EnemyDataBase DataBase { get { return m_dataBase; } }


    // ステージレベル等を反映したステータス　実際に使うのはこっち
    private List<EnemyData> m_useEnemyData = new();


    public EnemyData GetEnemyData(EnemyID _enemyID)
    {
        foreach (var list in m_useEnemyData)
        {
            // アイテムの種類が一致
            if (list.EnemyID == _enemyID)
            {
                return list;
            }
        }

        Debug.LogError(_enemyID.ToString() + "この敵のデータベースは登録されていません");
        return null;
    }

    public void ChangeEnemyLevel(StageEnemyStatus stageStatus)
    {
        foreach (var data in m_useEnemyData)
        {
            data.ChangeEnemyLevel(stageStatus, SearchDataID(m_dataBase.OriginalEnemyDataBaseList, data.EnemyID));
        }
    }

    public void Start()
    {
        m_useEnemyData.Clear();
        foreach (var data in m_dataBase.OriginalEnemyDataBaseList)
        {
            m_useEnemyData.Add(new EnemyData(data));
        }
    }

    private EnemyData SearchDataID(List<EnemyData> list, EnemyID id)
    {
        foreach (var data in list)
        {
            if (data.EnemyID == id)
            {
                return data;
            }
        }
        return new();
    }
}

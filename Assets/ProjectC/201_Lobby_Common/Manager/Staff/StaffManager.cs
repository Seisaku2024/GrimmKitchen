using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StaffInfo;
using System.Linq;

using NaughtyAttributes;
using Cysharp.Threading.Tasks;
using UniRx;


/// <summary>
/// スタッフストレージ取り除かれ
/// </summary>
public class GlobalRemoveStaffStorageData
{
    public static void PublishGlobalRemoveStaffStorageData()
    {
        // イベント送信
        GlobalRemoveStaffStorageData eve = new();
        MessageBroker.Default.Publish<GlobalRemoveStaffStorageData>(eve);
    }
}


/// <summary>
/// スタッフポイントデータ変更時
/// </summary>
public class GlobalChangeStaffPointData
{
    public static void PublishGlobalChangeStaffPointData()
    {
        // イベント送信
        GlobalChangeStaffPointData eve = new();
        MessageBroker.Default.Publish<GlobalChangeStaffPointData>(eve);
    }
}

public class StaffManager : BaseManager<StaffManager>
{
    // 制作者 田内
    // スタッフの情報を管理するマネージャークラス


    //========================================
    // スタッフのストレージリスト

    [Header("最大ストレージ")]
    [SerializeField]
    private uint m_maxStaffStorage = 20;

    public uint MaxStaffStorage
    {
        get { return m_maxStaffStorage; }
    }

    //========================================

    [BoxGroup("Status")]
    [Header("ステータス最大値")]
    [SerializeField]
    private int m_maxStatusValue = 999;
    public int MaxStatusValue
    {
        get { return m_maxStatusValue; }
    }


    [BoxGroup("Status")]
    [Header("移動時の追加割合")]
    [SerializeField]
    private float m_additionProvideRatio = 3.0f;
    public float AdditionProvideRatio
    {
        get { return m_additionProvideRatio; }
    }


    [BoxGroup("Status")]
    [Header("料理時の追加割合")]
    [SerializeField]
    private float m_additionCookingRatio = 3.0f;
    public float AdditionCookingRatio
    {
        get { return m_additionCookingRatio; }
    }

    //========================================
    // スタッフポイントデータ

    [Header("ポイント")]
    [SerializeField]
    private List<StaffPointData> m_staffPointDataList = new();

    public List<StaffPointData> StaffPointDataList
    {
        get { return m_staffPointDataList; }
    }

    //========================================
    // 初期スタッフ

    [Header("初期スタッフ")]
    [SerializeField]
    private List<StaffStatusData> m_initalizeStaffStorageList = new();

    //========================================
    // スタッフストレージ

    private ReactiveCollection<StaffStatusData> m_staffStorageRC = new();

    public ReactiveCollection<StaffStatusData> StaffStorageRC
    {
        get { return m_staffStorageRC; }
    }

    //========================================
    // 出現中のスタッフリスト

    private List<StaffData> m_staffDataList = new();

    //==============================================
    //                  実行処理
    //==============================================


    private void Update()
    {
        RemoveStaffData();
    }

    protected override void Load()
    {
        var saveLoad = StaffDataSaveLoader.Load();

        // 初期化
        m_staffStorageRC.Clear();

        // スタッフをロード
        foreach (var data in saveLoad.StaffStatusSaveLoadDataList)
        {
            if (data == null) continue;
            StaffStatusData newData = new();
            newData.Load(data);
            m_staffStorageRC.Add(newData);
        }

        foreach (var data in m_initalizeStaffStorageList)
        {
            if (data == null) continue;
            m_staffStorageRC.Add(data);
        }

        // ポイントにロード情報をセット
        foreach (var data in saveLoad.StaffPointSaveLoadDataList)
        {
            if (data == null) continue;
            m_staffPointDataList[data.PointNumber].Load(data);
        }

        // ステータスを検索
        foreach (var data in m_staffPointDataList)
        {
            if (data == null) continue;
            data.FindToSetStaffStatusData();
        }
    }

    /// <summary>
    ///  スタッフポイントの確認
    /// </summary>
    public void CheckSettingPointStaffStatus()
    {
        foreach (var data in m_staffPointDataList)
        {
            if (data == null) return;
            data.CheckAddedStaffStorage();
        }
    }

    /// <summary>
    /// ストレージ整頓
    /// </summary>
    public async UniTask TidyingStorage()
    {
        var list = m_staffStorageRC.ToList();
        m_staffStorageRC.Clear();

        // 昇順に入れ替え
        list = list.OrderByDescending(_ => _.StaffID).ToList();
        foreach (var data in list)
        {
            if (data == null) continue;
            m_staffStorageRC.Add(data);
        }

        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 引数スタッフを取得
    /// </summary>
    public List<StaffData> GetStaffDataList(StaffType _type)
    {
        List<StaffData> list = new();

        foreach (var data in m_staffDataList)
        {
            if (data == null || data.StaffStatusData == null) continue;
            if (data.StaffStatusData.StaffType == _type)
            {
                list.Add(data);
            }
        }

        return list;
    }


    /// <summary>
    /// スタッフポイントにスタッフをセット
    /// </summary>
    public bool SetStaffPointStaffStatus(StaffPointData _pointData, StaffStatusData _statusData)
    {
        if (_pointData == null) return false;
        if (m_staffPointDataList.Contains(_pointData) == false) return false;

        bool isChange = false;

        // 事前に既にセットされているものを取り除く
        foreach (var pointData in m_staffPointDataList)
        {
            if (pointData == null) continue;

            // 一致すればnullに
            if (pointData.StaffStatusData == _statusData)
            {
                pointData.SetStaffStatusData(null);
                isChange = true;
            }
        }

        // 当てはまるポイントにステータスをセット
        foreach (var staffPointData in m_staffPointDataList)
        {
            if (staffPointData == _pointData)
            {
                staffPointData.SetStaffStatusData(_statusData);
                isChange = true;
                break;
            }
        }

        // 変更が加われば
        if (isChange)
        {
            // イベント送信
            GlobalChangeStaffPointData.PublishGlobalChangeStaffPointData();
        }

        return isChange;
    }


    /// <summary>
    /// 引数スタッフステータスが追加されているかどうか
    /// </summary>
    public bool IsAddedStaffStorage(StaffStatusData _data)
    {
        foreach (var data in m_staffStorageRC)
        {
            if (data == null) continue;
            if (data == _data)
            {
                return true;
            }
        }

        return false;
    }



    /// <summary>
    /// スタッフをストレージに追加する
    /// </summary>
    public bool AddStaffStorage(StaffStatusData _data)
    {
        if (_data == null || IsAddStaffStorage()) return false;

        m_staffStorageRC.Add(_data);

        return true;
    }


    /// <summary>
    /// スタッフをストレージから取り除く
    /// </summary>
    public bool RemoveStaffStorage(StaffStatusData _data)
    {
        if (_data == null) return false;

        if (m_staffStorageRC.Remove(_data))
        {
            CheckSettingPointStaffStatus();
            return true;
        }

        return false;
    }




    /// <summary>
    /// スタッフが最大数に達しているかどうか
    /// </summary>
    public bool IsAddStaffStorage()
    {
        if (m_maxStaffStorage <= m_staffStorageRC.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// スタッフストレージの空き数を取得
    /// </summary>
    /// <returns></returns>
    public int GetFreeSpaceStaffStorageNum()
    {
        int freeSpaceNum = (int)m_maxStaffStorage;
        freeSpaceNum -= m_staffStorageRC.Count;

        return freeSpaceNum;
    }


    /// <summary>
    /// スタッフを追加する
    /// </summary>
    public void AddStaffData(StaffData _data)
    {
        if (_data == null) return;

        m_staffDataList.Add(_data);
    }


    /// <summary>
    /// 不必要なデータを取り除く
    /// </summary>
    public void RemoveStaffData(StaffData _data)
    {
        if (_data == null) return;

        m_staffDataList.Remove(_data);
        Destroy(_data.gameObject);
    }



    /// <summary>
    /// 合計給料を取得
    /// </summary>
    public int GetTotalSalaryPrice()
    {
        // 給料
        int price = 0;

        foreach (var data in m_staffPointDataList)
        {
            if (data == null || data.StaffStatusData == null) continue;
            price += data.StaffStatusData.SalaryPrice();
        }

        return price;
    }



    /// <summary>
    /// 引数のスタッフがスタッフポイントにセットされているかどうか
    /// </summary>
    public bool IsSettingStaffPoint(StaffStatusData _data)
    {
        if (_data == null) return false;

        foreach (var pointData in m_staffPointDataList)
        {
            if (pointData == null) continue;

            if (pointData.StaffStatusData == _data) return true;
        }
        return false;
    }


    /// <summary>
    /// シェフスタッフ・ホールスタッフがセットされているか
    /// </summary>
    /// <returns></returns>
    public bool IsSettingStaffPoint()
    {
        bool hall = false;
        bool chef = false;

        foreach (var pointData in m_staffPointDataList)
        {
            if (pointData == null) continue;
            if (pointData.StaffStatusData == null) continue;

            switch (pointData.SetStaffType)
            {
                case StaffType.Hall:
                    {
                        hall = true;
                        break;
                    }
                case StaffType.Chef:
                    {
                        chef = true;
                        break;
                    }
            }

            // 正常にセットされていれば
            if (hall && chef)
            {
                return true;
            }

        }

        return false;
    }

    /// <summary>
    /// スタッフの給料を保持しているかどうか
    /// </summary>
    public bool IsSalaryPriceStaffPoint()
    {
        var value = GetTotalSalaryPrice();

        if (value <= ManagementDataManager.instance.TotalEarnedMoney)
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// 引数IDスタッフを所持しているかどうか
    /// </summary>
    public bool IsAddedStaff(StaffID _id)
    {
        foreach (var staff in m_staffStorageRC)
        {
            // 既に追加されていればtrue
            if (staff.StaffID == _id) return true;
        }

        // 追加されていなければfalse
        return false;
    }

    // 不必要なデータ取り除く
    private void RemoveStaffData()
    {
        m_staffDataList.RemoveAll(_ =>
        {
            // nullであれば
            if (_ == null) return true;
            return false;
        });
    }

    // スタッフポイントからステータスをすべて取り除く
    private void RemoveStaffPointStaffStatus()
    {
        foreach (var data in m_staffPointDataList)
        {
            if (data == null) continue;
            data.SetStaffStatusData(null);
        }
    }


    public void OnInitialize()
    {
        // スタッフポイントのみ残した初期化、注意して行ってください

        // スタッフポイントのステータスを取り除く
        RemoveStaffPointStaffStatus();

        // スタッフ情報を初期化
        m_staffDataList.Clear();

        // スタッフのストレージ初期化
        m_staffStorageRC.Clear();
    }

    public void OnInitializeStaffPointData()
    {
        // スタッフポイントのステータスを取り除く
        RemoveStaffPointStaffStatus();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodInfo;
using ItemInfo;
using PocketItemDataInfo;
using UniRx;
using System.Linq;


public class ManagementProvideFoodData
{

    // 提供料理変更イベント
    public class GlobalChangeProvideFoodDataEvent
    {
        // 変更された提供料理データ
        public ManagementProvideFoodData ProvideFoodData = null;
    }

    // 提供料理追加イベント
    public class GlobalAddProvideFoodDataEvent
    {
        // 変更された提供料理データ
        public ManagementProvideFoodData ProvideFoodData = null;
    }

    public ManagementProvideFoodData(FoodID _id)
    {
        m_foodID = _id;
    }

    //========================
    // 料理ID
    private FoodID m_foodID = FoodID.Omelette;

    public FoodID FoodID
    {
        get { return m_foodID; }
    }

    //========================
    // ボーナス数

    private uint m_bonusNum = 0;

    public uint BonusNum
    {
        get { return m_bonusNum; }
    }

    //========================
    // 売れた数
    private uint m_soldNum = 0;

    public uint SoldNum
    {
        get { return m_soldNum; }
        set { m_soldNum = value; }
    }

    //========================
    // 取り除いた数
    private uint m_removeNum = 0;

    public uint RemoveNum
    {
        get { return m_removeNum; }
        set { m_removeNum = value; }
    }


    //====================================
    //          実行処理
    //====================================

    /// <summary>
    /// 提供可能かどうか
    /// </summary>
    public bool IsProvide()
    {
        if (m_bonusNum <= 0 && FoodData.IsProvide(ProvideFoodManager.instance.PocketType, m_foodID) == false) return false;
        return true;
    }

    /// <summary>
    /// 売り上げ数を加算
    /// </summary>
    public void AddBonusNum()
    {
        m_bonusNum++;
        PublishChangeProvideFoodDataEvent(this);
    }

    /// <summary>
    /// 売り上げ数を加算
    /// </summary>
    public void AddSoldNum()
    {
        m_soldNum++;
        PublishChangeProvideFoodDataEvent(this);
        PublishAddProvideFoodDataEvent(this);
    }

    /// <summary>
    /// 取り除いた数を加算、必要素材を取り除く
    /// </summary>
    public void AddRemoveNum()
    {
        // ボーナスから取り除く
        if (0 < m_bonusNum)
        {
            m_bonusNum--;
            PublishChangeProvideFoodDataEvent(this);
            return;
        }

        // 既に所持している料理から取り除く
        if (ProvideFoodManager.instance.PocketType.GetPocketItemDataManager().RemoveItem(ItemTypeID.Food, (uint)m_foodID))
        {
            m_removeNum++;
            PublishChangeProvideFoodDataEvent(this);
            return;
        }

        // 材料から取り除く
        if (FoodData.RemoveProvideNeedIngredient(ProvideFoodManager.instance.PocketType, m_foodID))
        {
            // 取り除いた数を加算
            m_removeNum++;
            PublishChangeProvideFoodDataEvent(this);
            return;
        }
    }

    private void PublishChangeProvideFoodDataEvent(ManagementProvideFoodData _data)
    {
        // イベント送信
        GlobalChangeProvideFoodDataEvent eve = new();
        eve.ProvideFoodData = _data;
        MessageBroker.Default.Publish<GlobalChangeProvideFoodDataEvent>(eve);
    }

    private void PublishAddProvideFoodDataEvent(ManagementProvideFoodData _data)
    {
        // イベント送信
        GlobalAddProvideFoodDataEvent eve = new();
        eve.ProvideFoodData = _data;
        MessageBroker.Default.Publish<GlobalAddProvideFoodDataEvent>(eve);
    }

}




public class ProvideFoodManager : BaseManager<ProvideFoodManager>
{

    // 制作者　田内
    // 提供する料理を操作・管理するマネージャー

    [Header("ポケットの種類")]
    [SerializeField]
    private PocketType m_pocketType = PocketType.ManagementStorage;

    public PocketType PocketType
    {
        get { return m_pocketType; }
    }


    [Header("最大で提供できる料理の種類")]
    [SerializeField]
    [Min(1)]
    private uint m_maxFoodListCount = 1;

    public uint MaxFoodListCount
    {
        get { return m_maxFoodListCount; }
    }

    //======================================
    // 提供する料理


    private ReactiveCollection<FoodID> m_provideFoodIDRC = new(new());

    public ReactiveCollection<FoodID> ProvideFoodIDRC
    {
        get { return m_provideFoodIDRC; }
    }

    public List<FoodID> ProvideFoodIDList
    {
        get { return m_provideFoodIDRC.ToList(); }
    }

    //======================================================================
    //                  実行処理
    //======================================================================


    protected override void Load()
    {
        var saveLoad = ProvideFoodSaveLoader.Load();

        // 初期化
        m_provideFoodIDRC.Clear();

        foreach (var data in saveLoad.ProvideFoodSaveLoadDataList)
        {
            if (data == null) continue;
            m_provideFoodIDRC.Add(data.FoodID);
        }
    }


    /// <summary>
    /// 提供料理を追加する
    /// </summary>
    public bool AddProvideFoodList(FoodID _foodID)
    {
        // 追加できるか確認する
        if (IsAddList() == false)
        {
            return false;
        }

        // 既に追加されていれば追加しない
        if (IsAddedProvideFood(_foodID))
        {
            return false;
        }

        // リストに追加
        m_provideFoodIDRC.Add(_foodID);

        return true;
    }


    /// <summary>
    /// 引数料理をリストから取り除く
    /// </summary>
    public bool RemoveProvideFoodList(FoodID _id)
    {
        bool isChange = false;

        m_provideFoodIDRC.RemoveAll(_ =>
        {
            // IDが一致すれば取り除く
            if (_ == _id)
            {
                isChange = true;
                return true;
            }
            return false;
        });

        return isChange;
    }



    /// <summary>
    /// 提供料理が作成可能かどうか確認する
    /// </summary>
    public bool IsProvideAll()
    {
        foreach (var id in m_provideFoodIDRC)
        {
            if (FoodData.IsProvide(m_pocketType, id)) return true;
        }

        return false;
    }


    /// <summary>
    /// 提供料理を追加できるか確認
    /// </summary>
    public bool IsAddList()
    {
        // 最大数を越えていたら追加しない
        if (m_maxFoodListCount <= m_provideFoodIDRC.Count)
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// 追加済みかどうか確認する
    /// </summary>
    public bool IsAddedProvideFood(FoodID _foodID)
    {
        foreach (var id in m_provideFoodIDRC)
        {
            if (id == _foodID) return true;
        }

        // 存在しなければ
        return false;
    }


    /// <summary>
    /// 提供可能な料理がセットされているか
    /// </summary>
    public bool IsSettingProvideFood()
    {
        // 料理がセットされていなければ
        if (m_provideFoodIDRC.Count <= 0) return false;

        return true;
    }


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void OnInitialize()
    {
        m_provideFoodIDRC = null;
        m_provideFoodIDRC = new();
    }


}

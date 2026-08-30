using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ItemInfo;
using IngredientInfo;
using FoodInfo;
using Cysharp.Threading.Tasks;
using System.Linq;


namespace PocketItemDataInfo
{
    public enum PocketType
    {
        Inventory = 0,              // インベントリ
        Storage = 1,             // 倉庫
        NewItem = 2,                // 新規アイテム
        ManagementStorage = 3,      // 経営用ストレージ
    }


    public static class PocketItemDataInfoExtensions
    {
        public static BasePocketItemDataController GetPocketItemDataManager(this PocketType _pocketType)
        {
            switch (_pocketType)
            {
                case PocketType.Inventory:
                    {
                        return InventoryManager.instance;
                    }
                case PocketType.Storage:
                    {
                        return StorageManager.instance;
                    }
                case PocketType.NewItem:
                    {
                        return NewItemManager.instance;
                    }
                case PocketType.ManagementStorage:
                    {
                        return ManagementStorageManager.instance;
                    }
            }

            Debug.LogError(_pocketType + "に対するマネージャーが存在しません");

            return null;
        }
    }

}


/// <summary>
/// アイテム追加時イベント
/// </summary>
public class GlobalAddItemEvent
{
    public BasePocketItemDataController PocketItemDataController = null;
    public PocketItemData PocketItemData = null;

    public static void PublishGlobalAddItemEven(BasePocketItemDataController _controller, PocketItemData _data)
    {
        // イベント送信
        GlobalAddItemEvent eve = new();
        eve.PocketItemDataController = _controller;
        eve.PocketItemData = _data;
        MessageBroker.Default.Publish<GlobalAddItemEvent>(eve);
    }
}


/// <summary>
/// アイテム取り除き時イベント
/// </summary>
public class GlobalRemoveItemEvent
{
    public BasePocketItemDataController PocketItemDataController = null;
    public PocketItemData PocketItemData = null;

    public static void PublishGlobalRemoveItemEven(BasePocketItemDataController _controller, PocketItemData _data)
    {
        // イベント送信
        GlobalRemoveItemEvent eve = new();
        eve.PocketItemDataController = _controller;
        eve.PocketItemData = _data;
        MessageBroker.Default.Publish<GlobalRemoveItemEvent>(eve);
    }
}


/// <summary>
/// ポケットアイテムリスト変更時イベント
/// </summary>
public class GlobalChangePocketItemListEvent
{
    public BasePocketItemDataController PocketItemDataController = null;
    public PocketItemData PocketItemData = null;

    public static void PublishGlobalChangePocketItemListEvent(BasePocketItemDataController _controller, PocketItemData _data)
    {
        // イベント送信
        GlobalChangePocketItemListEvent eve = new();
        eve.PocketItemDataController = _controller;
        eve.PocketItemData = _data;
        MessageBroker.Default.Publish<GlobalChangePocketItemListEvent>(eve);
    }
}


public partial class BasePocketItemDataController : MonoBehaviour
{
    // 制作者 田内
    // アイテムの管理を行うコントローラー

    //========================
    // 合計で所持できる数

    [Header("材料や料理を含めた、合計所持数")]
    [SerializeField]
    protected int m_listMaxSize = 20;

    public int ListMaxSize { get { return m_listMaxSize; } }

    //========================
    // 初期アイテム
    [System.Serializable]
    protected struct DebugPocketItemSaveLoadeData
    {
        [Header("所持数")]
        [SerializeField]
        [Min(0)]
        public int Num;

        [Header("アイテムタイプ")]
        [SerializeField]
        public ItemTypeID ItemTypeID;

        [Header("アイテムタイプ:Ingredientの場合")]
        public IngredientID IngredientID;

        [Header("アイテムタイプ:Foodの場合")]
        public FoodID FoodID;
    }

    [Header("デバッグ")]
    [SerializeField]
    protected bool m_isDebug = true;

    [Header("デバッグ用初期アイテム")]
    [SerializeField]
    protected List<DebugPocketItemSaveLoadeData> m_initializePocketItemList = new();

    //=========================
    // 所持している料理

    protected ReactiveCollection<PocketItemData> m_itemDataRC = new();

    public ReactiveCollection<PocketItemData> ItemDataRC
    {
        get
        {
            return m_itemDataRC;
        }
    }

    //======================================================
    //                  実行処理
    //======================================================

    virtual protected void StartInstance()
    {
        Load();

        MessageBroker.Default.Receive<SaveManager.GlobalDeleteSaveEvent>().Subscribe(_ =>
        {
            Load();
        });
    }

    virtual protected void Load()
    {
#if UNITY_EDITOR
        // デバッグ用
        if (m_isDebug)
        {
            // 初期アイテムのセット
            foreach (var data in m_initializePocketItemList)
            {
                switch (data.ItemTypeID)
                {
                    case ItemTypeID.Food:
                        {
                            PocketItemData newData = new(ItemTypeID.Food, (uint)data.FoodID, data.Num);
                            newData.IsSave = false;

                            m_itemDataRC.Add(newData);
                            break;
                        }
                    case ItemTypeID.Ingredient:
                        {
                            PocketItemData newData = new(ItemTypeID.Ingredient, (uint)data.IngredientID, data.Num);
                            newData.IsSave = false;

                            m_itemDataRC.Add(newData);
                            break;
                        }
                    default:
                        {
                            Debug.LogError("アイテムタイプが割り当て外のIDになっています : " + data.ItemTypeID.ToString());
                            break;
                        }
                }
            }
        }
#endif
    }


    /// <summary>
    /// アイテムリストに引数アイテムを追加する
    /// </summary>
    virtual public bool AddItem(ItemTypeID _itemTypeID, uint _itemID)
    {
        var data = ItemDataBaseManager.instance.GetItemData(_itemTypeID, _itemID);
        if (data == null) return false;

        // 保持
        PocketItemData targetData = null;

        // 現在所持しているアイテムから探す
        foreach (var itemData in m_itemDataRC)
        {
            if (itemData.ItemTypeID != _itemTypeID || itemData.ItemID != _itemID) continue;
            if (data.MaxNum <= itemData.Num) continue;

            targetData = itemData;
            break;
        }

        // もし検索時に存在しなければ新規作成
        if (targetData == null)
        {
            if (IsInList())
            {
                // アイテムデータを作成して新規追加
                targetData = PocketItemData.CreateItemData(_itemTypeID, _itemID, 0);
                m_itemDataRC.Add(targetData);
            }
            // 新規作成できない場合は終了
            else return false;
        }

        // 所持数を追加
        targetData.Num++;

        //イベントを送信
        GlobalAddItemEvent.PublishGlobalAddItemEven(this, targetData);
        GlobalChangePocketItemListEvent.PublishGlobalChangePocketItemListEvent(this, targetData);

        return true;
    }


    /// <summary>
    /// 引数アイテムデータの所持数を加算する
    /// </summary>
    virtual public bool AddItem(PocketItemData _pocketItemData)
    {
        if (_pocketItemData == null) return false;

        var data = ItemDataBaseManager.instance.GetItemData(_pocketItemData.ItemTypeID, _pocketItemData.ItemID);
        if (data == null) return false;

        PocketItemData targetData = null;

        // 検索
        foreach (var itemData in m_itemDataRC)
        {
            if (itemData == _pocketItemData)
            {
                targetData = itemData;
                break;
            }
        }

        if (targetData != null)
        {
            // 所持数を加算
            targetData.Num++;

            // イベント発信
            GlobalAddItemEvent.PublishGlobalAddItemEven(this, targetData);
            GlobalChangePocketItemListEvent.PublishGlobalChangePocketItemListEvent(this, targetData);

            return true;
        }
        else return false;
    }


    /// <summary>
    /// アイテムリストに存在する引数アイテムを減算/削除する
    /// </summary>
    virtual public bool RemoveItem(ItemTypeID _itemTypeID, uint _itemID)
    {
        // 保持
        PocketItemData targetData = null;

        // 検索
        foreach (var data in m_itemDataRC)
        {
            if (data.ItemTypeID != _itemTypeID || data.ItemID != _itemID) continue;

            if (targetData == null)
            {
                targetData = data;
            }
            else
            {
                // 一番所持数が少ないものから取り除く
                if (data.Num <= targetData.Num) targetData = data;
            }
        }

        // もうなければ終了
        if (targetData != null)
        {
            // 減算
            targetData.Num--;

            // もし手持ちが0になれば取り除く
            if (targetData.Num <= 0) m_itemDataRC.Remove(targetData);

            // リスト変化イベント送信
            GlobalRemoveItemEvent.PublishGlobalRemoveItemEven(this, targetData);
            GlobalChangePocketItemListEvent.PublishGlobalChangePocketItemListEvent(this, targetData);

            return true;
        }
        else return false;
    }

    /// <summary>
    /// 引数アイテムデータの所持数を減算する
    /// </summary>
    virtual public bool RemoveItem(PocketItemData _pocketItemData)
    {
        // 保持
        PocketItemData targetData = null;
        // 検索
        foreach (var data in m_itemDataRC)
        {
            if (data == _pocketItemData)
            {
                targetData = data;
                break;
            }
        }

        // ターゲットデータの所持数を減らす
        if (targetData != null)
        {
            // 減算
            targetData.Num--;

            // もし手持ちが0になれば取り除く
            if (targetData.Num <= 0) m_itemDataRC.Remove(targetData);

            // リスト変化イベント送信
            GlobalRemoveItemEvent.PublishGlobalRemoveItemEven(this, targetData);
            GlobalChangePocketItemListEvent.PublishGlobalChangePocketItemListEvent(this, targetData);

            return true;
        }
        else return false;
    }


    /// <summary>
    /// リストに直接追加する
    /// </summary>
    virtual public bool AddItemList(PocketItemData _pocketItemData)
    {
        if (IsInList() == false) return false;

        m_itemDataRC.Add(_pocketItemData);

        //イベントを送信
        GlobalAddItemEvent.PublishGlobalAddItemEven(this, _pocketItemData);
        GlobalChangePocketItemListEvent.PublishGlobalChangePocketItemListEvent(this, _pocketItemData);

        return true;
    }

    /// <summary>
    /// リストからアイテムを取り除く
    /// </summary>
    virtual public bool RemoveItemList(PocketItemData _pocketItemData)
    {

        if (m_itemDataRC.Remove(_pocketItemData))
        {
            //イベントを送信
            GlobalRemoveItemEvent.PublishGlobalRemoveItemEven(this, _pocketItemData);
            GlobalChangePocketItemListEvent.PublishGlobalChangePocketItemListEvent(this, _pocketItemData);

            return true;
        }
        else
        {
            return false;
        }
    }




    /// <summary>
    /// 整頓する
    /// </summary>
    public async UniTask Tidying()
    {

        // 新しくRCを初期化
        List<PocketItemData> currentList = m_itemDataRC.ToList();
        m_itemDataRC.Clear();

        // 一度同じ種類を全てまとめる
        List<PocketItemData> summaryList = new();
        foreach (var itemData in currentList)
        {
            // 存在しなければ
            if (itemData == null) continue;

            PocketItemData isAddingData = null;
            foreach (var data in summaryList)
            {
                // 一致するアイテムデータがあれば
                if (data.ItemTypeID == itemData.ItemTypeID && data.ItemID == itemData.ItemID)
                {
                    isAddingData = data;
                    break;
                }
            }

            // なければ追加
            if (isAddingData == null)
            {
                PocketItemData newItemData = new(itemData.ItemTypeID, itemData.ItemID, itemData.Num);
                summaryList.Add(newItemData);
            }
            // 既に存在すれば数を追加
            else
            {
                isAddingData.Num += itemData.Num;
            }
        }

        // 取り除きイベントを発信
        foreach (var itemData in currentList)
        {
            if (itemData == null) continue;
            GlobalRemoveItemEvent.PublishGlobalRemoveItemEven(this, itemData);
        }


        // ID順に並び替える
        summaryList = summaryList.OrderByDescending(_ => _.ItemTypeID).ThenBy(_ => _.ItemID).ToList();

        foreach (var itemData in summaryList)
        {
            var data = ItemDataBaseManager.instance.GetItemData(itemData.ItemTypeID, itemData.ItemID);
            if (data == null) continue;

            // 最大数から計算
            int num = (int)(itemData.Num / data.MaxNum);
            int remainder = (int)(itemData.Num % data.MaxNum);

            // 最大数の作成
            for (int i = 0; i < num; ++i)
            {
                PocketItemData newItemData = new(itemData.ItemTypeID, itemData.ItemID, (int)data.MaxNum);
                m_itemDataRC.Add(newItemData);
            }

            // 余りの数作成
            if (0 < remainder)
            {
                PocketItemData newItemData = new(itemData.ItemTypeID, itemData.ItemID, remainder);
                m_itemDataRC.Add(newItemData);
            }
        }

        await UniTask.CompletedTask;
    }


    //=====================================================================================
    //                               取得用メソッド
    //=====================================================================================

    #region メソッド説明
    ///--------------------------------------
    /// <summary>
    /// 引数IDのアイテム所持数を取得するメソッド
    /// </summary>
    /// -------------------------------------
    /// <param name="_itemTypeID">
    /// 取得したいアイテムの種類
    /// </param>
    /// -------------------------------------
    /// <param name="_itemID">
    /// 取得したいアイテムのID
    /// </param>
    /// --------------------------------------
    /// <returns>
    /// 引数IDのアイテム所持数
    /// </returns>
    /// --------------------------------------
    #endregion
    // 引数IDのアイテム所持数を取得
    virtual public int GetItemNum(ItemTypeID _itemTypeID, uint _itemID)
    {
        // 所持数
        int num = 0;

        // 検索
        foreach (var list in m_itemDataRC)
        {

            if (list.ItemTypeID != _itemTypeID)
            {
                continue;
            }

            if (list.ItemID != _itemID)
            {
                continue;
            }

            num += list.Num;

        }

        // 当てはまったデータを返す
        return num;

    }


    #region メソッド説明
    ///--------------------------------------
    /// <summary>
    /// ポケット空き容量が最大数に達しているか確認するメソッド
    /// </summary>
    /// --------------------------------------
    /// <returns>
    /// true - 達していない(取得可能)
    /// false - 達している(取得不可)
    /// </returns>
    /// --------------------------------------
    #endregion
    virtual public bool IsInList()
    {
        if (m_itemDataRC.Count < m_listMaxSize) return true;
        return false;
    }

    /// <summary>
    /// 引数アイテムが引数分追加できるか
    /// </summary>
    virtual public bool IsInList(ItemTypeID _itemTypeID, uint _itemID, int _inNum)
    {
        var data = ItemDataBaseManager.instance.GetItemData(_itemTypeID, _itemID);
        if (data == null) return false;

        // 追加可能数
        int canAddNum = 0;
        foreach (var itemData in m_itemDataRC)
        {
            if (itemData == null) continue;
            if (itemData.ItemTypeID == _itemTypeID && itemData.ItemID == _itemID)
            {
                canAddNum += (int)(data.MaxNum - itemData.Num);
            }
        }
        canAddNum += GetFreeSpaceNum() * (int)data.MaxNum;

        if (canAddNum < _inNum) return false;
        else return true;
    }

    /// <summary>
    /// 引数アイテムリストを全て追加できるか
    /// </summary>
    virtual public bool IsInList(PocketItemData _data)
    {
        if (_data == null) return false;

        var data = ItemDataBaseManager.instance.GetItemData(_data.ItemTypeID, _data.ItemID);
        if (data == null) return false;

        // 追加可能数
        int canAddNum = 0;
        foreach (var itemData in m_itemDataRC)
        {
            if (itemData == null) continue;
            if (itemData.ItemTypeID == _data.ItemTypeID && itemData.ItemID == _data.ItemID)
            {
                canAddNum += (int)(data.MaxNum - itemData.Num);
            }
        }
        canAddNum += GetFreeSpaceNum() * (int)data.MaxNum;


        // もし所持数を超えてしまえば
        if (canAddNum < _data.Num) return false;

        return true;

    }


    /// <summary>
    /// 引数アイテムリストを全て追加できるか
    /// </summary>
    virtual public bool IsInList(List<PocketItemData> _list)
    {
        // 同じデータを持つ新規データ作成
        List<PocketItemData> itemList = new();
        foreach (var data in m_itemDataRC)
        {
            if (data == null) continue;
            itemList.Add(new(data.ItemTypeID, data.ItemID, data.Num));
        }

        foreach (var inData in _list)
        {
            if (inData == null) continue;

            // アイテムデータ
            var data = ItemDataBaseManager.instance.GetItemData(inData.ItemTypeID, inData.ItemID);
            if (data == null) return false;

            // ターゲット
            PocketItemData targetData = null;
            // 所持数分
            for (int i = 0; i < inData.Num; ++i)
            {
                // リストから検索
                if (targetData == null)
                {
                    foreach (var itemData in itemList)
                    {
                        if (inData.ItemTypeID == itemData.ItemTypeID && inData.ItemID == itemData.ItemID)
                        {
                            if (itemData.Num < data.MaxNum)
                            {
                                targetData = itemData;
                            }
                        }
                    }
                }

                // 検索しても無ければ新規作成
                if (targetData == null)
                {
                    if (itemList.Count < m_listMaxSize)
                    {
                        targetData = new(inData.ItemTypeID, inData.ItemID, 0);
                        itemList.Add(targetData);
                    }
                }

                // 既存データがあれば
                if (targetData != null)
                {
                    targetData.Num++;
                    if (data.MaxNum <= targetData.Num)
                    {
                        targetData = null;
                    }
                }
                // 追加できなければ
                else
                {
                    return false;
                }
            }
        }

        // 問題なく追加出来れば
        return true;
    }


    virtual public List<PocketItemData> GetItemList(ItemTypeID _typeID)
    {

        List<PocketItemData> itemList = new();

        foreach (var list in m_itemDataRC)
        {
            if (_typeID == ItemTypeID.ALL || list.ItemTypeID == _typeID)
            {
                itemList.Add(list);
            }
        }

        return itemList;
    }


    /// <summary>
    /// 引数アイテムを所持しているかどうか
    /// </summary>
    virtual public bool IsHave(ItemTypeID _itemTypeID, uint _itemID)
    {
        foreach (var data in m_itemDataRC)
        {
            if (data == null) continue;
            if (data.ItemTypeID != _itemTypeID) continue;
            if (data.ItemID != _itemID) continue;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 引数アイテムを所持しているかどうか
    /// </summary>
    virtual public bool IsHave(PocketItemData _data)
    {
        foreach (var data in m_itemDataRC)
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
    /// 空き容量を取得
    /// </summary>
    virtual public int GetFreeSpaceNum()
    {
        return m_listMaxSize - m_itemDataRC.Count;
    }


    /// <summary>
    /// 全てのポケットデータを初期化
    /// </summary>
    public static void OnInitialize()
    {
        // コントローラーを全て取得
        var controllerList = FindObjectsByType<BasePocketItemDataController>();
        foreach (var controller in controllerList)
        {
            controller.ItemDataRC.Clear();
        }
    }

}

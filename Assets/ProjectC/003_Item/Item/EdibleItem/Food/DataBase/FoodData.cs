using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngredientInfo;
using FoodInfo;
using ItemInfo;
using ConditionInfo;
using PocketItemDataInfo;
using SaintsField;
using CI.QuickSave;
using System;

[CreateAssetMenu(fileName = "FoodData", menuName = "ScriptableObjects/Food/作成 FoodItemData")]
[System.Serializable]
public class FoodData : BaseItemData
{

    // 制作者(田内)

    //============================
    // 料理ID


    [Header("料理のID")]
    [SerializeField]
    private FoodID m_foodID = FoodID.Omelette;

    public enum FoodType
    {
        Heal,
        DebuffCondition,
        StrengtheningStatus
    }

    [Header("食べ物の分類")]
    [SerializeField] private FoodType m_foodType;
    public bool IsFoodType(FoodType _id)
    {
        if (m_foodType == _id) return true;
        return false;
    }
    public FoodType GetFoodType() { return m_foodType; }

    [Serializable]
    public class AddStatus
    {
        public enum AddStatusType
        {
            Attack = 0,
            Hp,
            Defence,
            Stamina
        }
        [SerializeField] private AddStatusType m_addStatusType;
        public AddStatusType AddType => m_addStatusType;

        [SerializeField] private float m_addValue;
        public float AddValue => m_addValue;

        public AddStatus(AddStatus status)
        {
            m_addStatusType = status.m_addStatusType;
            m_addValue = status.m_addValue;
        }
    }
    [ShowIf(nameof(m_foodType) + "==", FoodType.StrengtheningStatus)]
    [SerializeField] private AddStatus m_addPlayerStatus;
    public AddStatus AddPlayerStatus => m_addPlayerStatus;

    //============================
    // 初期ロック

    [Header("初期ロック")]
    [SerializeField]
    private bool m_initializeLock = false;

    public bool InitializeLock { get { return m_initializeLock; } }

    //============================
    // 経営部分で使用するプレハブ


    [Header("経営部分のプレハブ(※インスタンスを作成する際に使用します)")]
    [SerializeField]
    private GameObject m_managementFoodPrefab = null;

    public GameObject ManagementFoodPrefab
    {
        get
        {
            if (m_managementFoodPrefab == null)
            {
                Debug.LogError("料理:" + m_foodID + "の経営用プレハブが登録されていません");
            }

            return m_managementFoodPrefab;
        }
    }

    //====================================
    // この料理に必要な素材(レシピ)


    // 料理作成に必要な材料のデータをまとめたクラス
    [System.Serializable]
    public class NeedIngredientObject
    {

        // ID
        [Header("材料ID")]
        [SerializeField]
        private IngredientID m_ingredientID = IngredientID.SleepApple;


        public IngredientID IngredientID { get { return m_ingredientID; } }

        // 必要な数
        [Header("必要な数")]
        [SerializeField]
        [Range(1, 10)]
        private uint m_num;

        public uint Num { get { return m_num; } }

    }


    [Header("料理に必要な材料リスト(レシピ)")]
    [SerializeField]
    private List<NeedIngredientObject> m_needIngredientObjectList = new();


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理に必要な材料の種類をまとめたリストを返す、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理に必要な材料の種類をまとめたリスト
    /// </returns>
    /// --------------------------------------
    #endregion
    public List<NeedIngredientObject> NeedIngredientObjectList { get { return m_needIngredientObjectList; } }


    //===================================
    // 料理にかかる時間

    [Header("料理作成にかかる待機時間")]
    [SerializeField]
    [Range(0, 100)]
    private int m_createDelay = 2;

    public int CreateDelay { get { return m_createDelay; } }



    //===================================
    // 投げたときの範囲

    [Header("投げた際の効果範囲")]
    [SerializeField]
    [Range(1, 100)]
    private float m_throwRange = 4.0f;

    public float ThrowRange { get { return m_throwRange; } }


    //====================================
    // レシピ解禁情報

    private LockRecipeData m_lockRecipeData = null;

    public LockRecipeData LockRecipeData
    {
        get
        {
            if (m_lockRecipeData == null) m_lockRecipeData = new(m_foodID);
            return m_lockRecipeData;
        }
    }

    //=========================================================
    //                      実行処理
    //=========================================================


    public override void SetData()
    {
        m_itemTypeID = ItemTypeID.Food;
        m_itemID = (uint)m_foodID;
    }


    public void Load(RecipeSaveLoad _data)
    {
        if (_data == null) return;
        m_lockRecipeData = new(m_foodID);
        m_lockRecipeData.IsLock = _data.IsLock;
    }


    /// <summary>
    /// ロックを解除する
    /// </summary>
    public void OnUnLock()
    {
        if (m_lockRecipeData == null) m_lockRecipeData = new((FoodID)m_itemID);
        m_lockRecipeData.IsLock = false;
    }


    /// <summary>
    /// 量の色座種類IDを取得
    /// </summary>
    public IngredientTypeID GetIngredientTypeID()
    {
        IngredientTypeID id = IngredientTypeID.None;

        // 食材IDをセット
        foreach (var needIngredient in m_needIngredientObjectList)
        {
            if (needIngredient == null) continue;
            var data = ItemDataBaseManager.instance.GetItemData<IngredientData>(ItemTypeID.Ingredient, (uint)needIngredient.IngredientID);
            id |= data.IngredientTypeID;
        }

        return id;
    }

    /// <summary>
    /// 自身の価格計算
    /// </summary>
    public int Price()
    {
        float price = 0;
        foreach (var ingredient in NeedIngredientObjectList)
        {
            if (ingredient == null) continue;

            IngredientData data = ItemDataBaseManager.instance.GetItemData<IngredientData>(ItemTypeID.Ingredient, (uint)ingredient.IngredientID);
            if (data == null) continue;
            price += data.Price * ingredient.Num;
        }

        return (int)price;
    }



    /// <summary>
    /// 自身の満足値
    /// </summary>
    public int SatisfactionValue()
    {
        float satisfactionValue = 0;
        foreach (var ingredient in NeedIngredientObjectList)
        {
            if (ingredient == null) continue;

            IngredientData data = ItemDataBaseManager.instance.GetItemData<IngredientData>(ItemTypeID.Ingredient, (uint)ingredient.IngredientID);
            if (data == null) continue;
            satisfactionValue += data.SatisfactionValue * (int)ingredient.Num;
        }

        return (int)satisfactionValue;
    }

    /// <summary>
    /// 引数料理の価格計算
    /// </summary>
    public uint HealingValue()
    {
        uint healVal = 0;
        foreach (var ingredient in NeedIngredientObjectList)
        {
            if (ingredient == null) continue;

            IngredientData data = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Ingredient, (uint)ingredient.IngredientID) as IngredientData;
            if (data == null) continue;
            healVal += data.HealValue * ingredient.Num;
        }

        return healVal;
    }

    //=========================================================
    //                      static処理
    //=========================================================

    /// <summary>
    /// 引数料理を作成後、ポケットに追加する
    /// </summary>
    public static void CreateFood(PocketType _pocketType, FoodID _foodID, bool _removeFlg)
    {
        // 作成できるか確認
        if (_removeFlg == true)
        {
            if (IsCreate(_pocketType, _foodID) == false) return;
            RemoveNeedIngredient(_pocketType, _foodID);
        }

        // 料理を作成
        _pocketType.GetPocketItemDataManager().AddItem(ItemTypeID.Food, (uint)_foodID);

    }


    /// <summary>
    /// 引数料理が作成できるか確認するメソッド
    /// </summary>
    public static bool IsCreate(PocketType _pocketType, FoodID _id)
    {
        if (0 < GetCreateNum(_pocketType, _id)) return true;
        else return false;
    }


    /// <summary>
    /// 引数料理が提供できるか確認するメソッド
    /// </summary>
    public static bool IsProvide(PocketType _pocketType, FoodID _id)
    {
        if (0 < GetProvideNum(_pocketType, _id)) return true;
        else return false;
    }



    /// <summary>
    /// 引数料理が作成できる数を確認するメソッド
    /// </summary>
    public static int GetCreateNum(PocketType _pocketType, FoodID _id)
    {
        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)_id);
        if (data == null) return 0;

        // 作成可能数
        int num = 0;

        // ループ
        while (true)
        {
            // 必要な素材リスト
            foreach (var needIngredient in data.NeedIngredientObjectList)
            {
                if (needIngredient == null) continue;

                // 対応するポケットから素材所持数
                var itemNum = _pocketType.GetPocketItemDataManager().GetItemNum(ItemTypeID.Ingredient, (uint)needIngredient.IngredientID);

                // 現在の所持数が必要数を下回っていれば
                if (itemNum < needIngredient.Num * (num + 1)) return num;
            }

            // 作成可能数を加算
            num++;
        }
    }


    /// <summary>
    /// 引数料理が作成できるか確認するメソッド
    /// </summary>
    public static int GetProvideNum(PocketType _pocketType, FoodID _id)
    {
        // 提供可能数
        int num = 0;

        num += _pocketType.GetPocketItemDataManager().GetItemNum(ItemTypeID.Food, (uint)_id);
        num += GetCreateNum(_pocketType, _id);

        return num;
    }

    /// <summary>
    /// 必要食材をポケットから取り除く
    /// </summary>
    public static bool RemoveNeedIngredient(PocketType _pocketType, FoodID _foodID)
    {
        // 作成可能でなければ取り除かない
        if (IsCreate(_pocketType, _foodID) == false) return false;

        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)_foodID);

        // 必要な素材リスト
        foreach (var list in data.NeedIngredientObjectList)
        {
            if (list == null) continue;

            // 必要な素材数分
            for (int i = 0; i < list.Num; ++i)
            {
                // 必要な素材を取り除く
                _pocketType.GetPocketItemDataManager().
                    RemoveItem(ItemTypeID.Ingredient, (uint)list.IngredientID);
            }
        }

        return true;
    }


    /// <summary>
    /// 既存の料理をポケットから取り除く
    /// 取り除けなかった場合は食材を取り除く
    /// </summary>
    public static bool RemoveProvideNeedIngredient(PocketType _pocketType, FoodID _foodID)
    {
        // 既存の料理から先に除く
        if (_pocketType.GetPocketItemDataManager().RemoveItem(ItemTypeID.Food, (uint)_foodID) == false)
        {
            // 食材から取り除く
            if (RemoveNeedIngredient(_pocketType, _foodID) == false)
            {
                return false;
            }
        }

        return true;
    }
}


/// <summary>
/// 読み込み/書き込み用チャレンジデータ
/// </summary>
[System.Serializable]
public class RecipeSaveLoad
{
    public RecipeSaveLoad(FoodID _id)
    {
        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)_id);
        if (data == null) return;

        FoodID = _id;
        IsLock = data.InitializeLock;
    }

    public RecipeSaveLoad(FoodID _id, LockRecipeData _data)
    {
        if (_data == null) return;

        FoodID = _id;
        IsLock = _data.IsLock;
    }

    public FoodID FoodID = FoodID.Omelette;

    public bool IsLock = false;
}


[System.Serializable]
public class LockRecipeData
{
    public LockRecipeData(FoodID _id)
    {
        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)_id);
        if (data == null) return;

        IsLock = data.InitializeLock;
    }

    // ロックされているか
    public bool IsLock = false;
}
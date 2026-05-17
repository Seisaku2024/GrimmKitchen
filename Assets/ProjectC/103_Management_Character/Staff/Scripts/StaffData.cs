using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StaffInfo
{
    /// <summary>
    /// スタッフID
    /// </summary>
    public enum StaffID
    {
        Boy1 = 0,
        Boy2 = 2,
        Boy3 = 3,
        Boy4 = 4,
        Boy5 = 5,
        Boy6 = 6,

        Girl1 = 10,
        Girl2 = 11,
        Girl3 = 12,
        Girl4 = 13,
        Girl5 = 14,
        Girl6 = 15,

        BandMan = 50,
        ElderSister = 51,
        Maid = 52,




        Akazukin = 100,
        Hunter = 101,
        OldWoman = 102,

        Hensel = 110,
        Gretel = 111,

        BremenDonkey = 120,
        BremenDog = 121,
        BremenCat = 122,
        BremenChicken = 123,
    }

    /// <summary>
    /// スタッフのレアリティ
    /// </summary>
    public enum StaffRarityType
    {
        Normal,
        Rare,
        SuperRare,
    }

    /// <summary>
    /// スタッフの働き方タイプ
    /// </summary>
    public enum StaffType
    {
        Hall,
        Chef,
    }

    /// <summary>
    /// スタッフの性別
    /// </summary>
    public enum StaffGenderType
    {
        Man,
        Woman,
    }

    /// <summary>
    /// ホールスタッフの状態
    /// </summary>
    public enum HallStaffState
    {
        Default,        // 通常の状態
        MoveGetFood,    // 料理を取りに行く状態
        CarryFood,      // 料理を運んでいる状態
        MoveDefault,    // デフォルトの位置に移動する状態
        Stun,           // 攻撃されている状態
        Set,            // 料理をセットしている状態
    }

    /// <summary>
    /// シェフスタッフの状態
    /// </summary>
    public enum ChefStaffState
    {

        Find = 0,       // 料理探す
        Cooking = 1,    // 料理調理
    }


    /// <summary>
    /// スタッフ名ID
    /// </summary>
    public enum StaffNameID
    {
        None = 0,

        Takeo = 1,
        Lina = 2,
        Milia = 3,
        Hughley = 4,
        Kaeli = 5,
        Mina = 6,
        Marie = 7,
        MeiLin = 8,
        JianYue = 9,


        BandMan = 50,
        ElderSister = 51,
        Maid = 52,
        Cat = 53,
        Chicken = 54,
        Dog = 55,
        Donkey = 56,


        Littlered = 100,
        Gretel = 101,
        Hansel = 102,
        Hunter = 103,
        OldWoman = 104,

        Mother = 150,
        Father = 151,
    }

    public enum StaffStatusUpID
    {
        None = 0,
        StaffStatus_Provide = 1,
        StaffStatus_Cooking = 2,
        StaffStatus_Service = 3,
    }

}


public partial class StaffData : MonoBehaviour
{
    // 制作者　田内


    //================
    // 料理を持つ位置

    [Header("料理を保持する位置")]
    [SerializeField]
    private GameObject m_havePoint = null;

    public GameObject HavePoint { get { return m_havePoint; } }

    //============
    // 初期座標

    private Vector3 m_defaultPos = new();

    public Vector3 DefaultPos { get { return m_defaultPos; } }

    //=================
    // ターゲットの料理

    private OrderFoodData m_targetOrderFoodData = null;

    public OrderFoodData TargetOrderFoodData
    {
        get { return m_targetOrderFoodData; }
        set { m_targetOrderFoodData = value; }
    }

    //=========================================
    // ターゲットにしてきたオブジェクトのリスト

    private List<GameObject> m_beenTargetedObjectList = new();

    public List<GameObject> BeenTargetObjectList
    {
        get
        {
            m_beenTargetedObjectList.RemoveAll(_ => _ == null);
            return m_beenTargetedObjectList;
        }
        set { m_beenTargetedObjectList = value; }
    }


    //==============================================================
    //                  実行処理
    //==============================================================

    private void Start()
    {
        // マネージャーに登録
        StaffManager.instance.AddStaffData(this);

        SetInitializeData();
    }


    protected void SetInitializeData()
    {
        // 待つ座標をセット
        m_defaultPos = transform.position;

        // ステータスを更新
        SetStatus();

    }


}

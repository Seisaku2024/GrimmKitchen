

// 制作者(田内)

namespace IngredientInfo
{

    // 初期値入れとかないと全部ズレるんで消さないでください

    #region 名前空間説明
    /// -------------------------------------
    /// <summary>
    /// 材料のIDを示す列挙型
    /// </summary>
    /// -------------------------------------
    #endregion
    // 材料のIDを示す列挙型
    public enum IngredientID
    {
        //    NormalMushroom = 0,
        //    BigMushroom = 1,
        FireMushroom = 2,
        SleepMushroom = 3,
        ParalysisMushroom = 4,
        PoisonMushroom = 5,

        SleepApple = 7,

        Potato = 10,
        Carrot = 11,
        Onion = 12,
        Cabbage = 13,


        // 敵素材
        Egg = 500,

        BeastMeat = 501,
        BeastSpecialMeat = 502,
        BirdMeat = 510,
        OtherMeat = 520,
        FishMeat = 525,
        FishSpecialMeat = 526,

        PoisonFang = 530,

        SquidInk = 550,

        // ボス素材
        GoldenEgg = 1000,
        WolfBoneMeat = 1001,
    }


    [System.Flags]
    public enum IngredientTypeID
    {
        None = 0,            // 無し
        Egg = 1,             // タマゴ   
        Meat = 1 << 1,       // 肉
        Vegetable = 1 << 2,  // 野菜
        Fruit = 1 << 3,      // フルーツ
        Fish = 1 << 5,       // 魚
        Luxury = 1 << 6,     // 高級品
    }


}


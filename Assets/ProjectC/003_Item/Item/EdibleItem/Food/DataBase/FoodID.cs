
// 制作者(田内)

namespace FoodInfo
{

    // 初期値入れとかないと全部ズレるんで消さないでください

    #region 名前空間説明
    /// -------------------------------------
    /// <summary>
    /// 料理のIDを示す列挙型
    /// </summary>
    /// -------------------------------------
    #endregion
    // 料理のIDを示す列挙型
    public enum FoodID
    {
        None = 0,

        // オムレツ料理
        Omelette = 40,
        MushroomOmelette = 41,
        ParalysisMushroomOmelette = 42,
        SpicyMushroomOmelette = 43,

        // パイ料理
        SleepApplePie = 60,

        // 肉
        // 獣肉
        Steak = 80,
        ShabuShabu = 81,

        // 鳥肉
        ChickenSkewers = 90,

        // はてな肉
        Jerky = 100,
        Yukhoe = 101,

        // キノコ料理
        PoisonMushroomSoup = 103,
        ParalysisMushroomSoup = 105,
        SleepMushroomSoup = 106,
        FireMushroomSoup = 107,

        // 揚げ物
        FriedPotato = 120,
        OnionFry = 121,

        // 複合
        Potaufeu = 150,
        Curry = 151,

        // サラダ
        PotatoSalad = 200,
        Pickles = 201,

        // ドリンク
        VegetableJuice = 300,

        // アルコール飲料
        SnakeSake = 320,

        // ボス
        GoldenPudding = 1000,
        WolfRoastBeef = 1001,

    }
}

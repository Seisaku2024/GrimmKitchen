using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ManagementExtensions
{
    // 制作者　田内
    // 経営用の拡張クラス

    /// <summary>
    /// ランダムシャッフルしたリストを取得
    /// </summary>
    public static List<T> GetShuffleRandomList<T>(this List<T> _list)
    {
        // ランダムインスタンスを作成
        System.Random random = new System.Random();

        // Listの要素をランダムに並び替える
        List<T> shuffleList = _list.OrderBy(x => random.Next()).ToList();

        return shuffleList;
    }

    /// <summary>
    /// ランダムにシャッフルしたハッシュセットを取得
    /// </summary>
    public static HashSet<T> GetShuffleRandomHashSet<T>(this HashSet<T> _hashSet)
    {
        // ランダムインスタンスを作成
        System.Random random = new System.Random();

        // HashSetの要素をランダムに並び替える
        HashSet<T> shuffleHashSet = _hashSet.OrderBy(x => random.Next()).ToHashSet();

        return shuffleHashSet;
    }



    /// <summary>
    /// 配列からランダムに値を取得
    /// </summary>
    public static T GetRandomArray<T>(this System.Array _array)
    {
        if (_array == null || _array.Length <= 0)
        {
            return default;
        }

        // ランダムインスタンスを作成
        System.Random random = new System.Random();
        int index = random.Next(_array.Length);
        return (T)_array.GetValue(index);
    }

    /// <summary>
    /// リストからランダムで値を取得
    /// </summary>
    public static T GetRandom<T>(this List<T> _list)
    {
        if (_list == null || _list.Count <= 0)
        {
            return default;
        }

        System.Random random = new System.Random();
        int index = random.Next(_list.Count);
        return _list[index];
    }



    /// <summary>
    /// 引数transformのローカルを初期化する
    /// 引数のbool型にfalseを入れることで特定の行列を初期化しない事も可能
    /// </summary>
    public static void InitializeLocalTransform(this Transform _transform, bool _position = true, bool _rotation = true, bool _scale = true)
    {
        if (_position) _transform.localPosition = Vector3.zero;
        if (_rotation) _transform.rotation = Quaternion.identity;
        if (_scale) _transform.localScale = Vector3.one;
    }


    /// <summary>
    /// 列挙型をランダムに取得する
    /// </summary>
    public static T GetRandomEnumValue<T>() where T : System.Enum
    {
        System.Array values = System.Enum.GetValues(typeof(T));
        return values.GetRandomArray<T>();
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public static class UIExtensions
{
    // 制作者　田内
    // UI用の拡張クラス

    /// <summary>
    /// 引数のアクティブを基にListのアクティブを更新する
    /// </summary>
    // 拡張メソッドではないけどまとめておきたいのでここに書いてます
    public static void CheckToSetActiveGameObjectList<T>(T _component, List<GameObject> _list) where T : Behaviour
    {
        if (_component == null) return;

        bool isActive = _component.gameObject.activeSelf;

        foreach (var obj in _list)
        {
            obj.SetActive(isActive);
        }
    }

    /// <summary>
    /// UniRxのReactiveCollectionにRemoveAllを追加
    /// </summary>
    public static void RemoveAll<T>(this ReactiveCollection<T> collection, System.Func<T, bool> predicate)
    {
        var itemsToRemove = collection.Where(predicate).ToList();
        foreach (var item in itemsToRemove)
        {
            collection.Remove(item);
        }
    }

}

/*!
 * @file FoodLimitBarManager.cs
 * @brief フードの制限バーの表示/非表示を管理するクラス
 * @author 上甲
 */

using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;

public class FoodLimitBarManager : BaseManager<FoodLimitBarManager>
{
    List<OrderFoodData> m_foodData = new();
    List<OrderFoodData> m_addList = new();
    List<OrderFoodData> m_removeList = new();

    [SerializeField]
    GameObject m_limitBarPrefab = null;

    public void RegisterFoodList(OrderFoodData orderfood)
    {
        if (orderfood == null)
        {
            Debug.LogError("orderfood is null");
            return;
        }
        m_addList.Add(orderfood);

        if (m_limitBarPrefab == null)
        {
            Debug.LogError("m_limitBarPrefab is null");
            return;
        }
        var obj = Instantiate(m_limitBarPrefab);

        // 初期位置を指定しているオブジェクトが紐づけられていたらローカル座標を設定
        if (orderfood.gameObject.TryGetComponent(out GameObjectRegistry regisry))
        {
            GameObject posObj = regisry.GetObjectByKey("LimitBarPos");

            if (posObj != null)
            {
                obj.transform.localPosition = posObj.transform.localPosition;
            }
        }

        orderfood.m_limitBarUIObject = obj;

        SetParentAndKeepLocalTransform(obj.transform, orderfood.transform);

        var bar = obj.GetComponent<FoodLimitBarController>();

        if (bar != null)
        {
            bar.SetFoodData(orderfood, orderfood.CustomerData.AngryTime, orderfood.CustomerData.AngryTime);
        }
    }
    void SetParentAndKeepLocalTransform(Transform child, Transform newParent)
    {
        // 親を変更する前のワールド座標・回転・スケールを取得
        Vector3 worldPosition = child.position;
        Quaternion worldRotation = child.rotation;
        Vector3 worldScale = child.lossyScale;

        // 新しい親を設定（ワールド座標を維持しない）
        child.SetParent(newParent, false);

        // 取得したワールド座標・回転・スケールを適用
        child.localPosition = newParent.InverseTransformPoint(worldPosition);
        child.localRotation = Quaternion.Inverse(newParent.rotation) * worldRotation;

        // スケールの適用（親の影響を打ち消す）
        Vector3 parentScale = newParent.lossyScale;
        child.localScale = new Vector3(
            worldScale.x / parentScale.x,
            worldScale.y / parentScale.y,
            worldScale.z / parentScale.z
        );
    }


    void FoodCheck(OrderFoodData orderFood)
    {
        if (orderFood == null)
        {
            m_removeList.Add(orderFood);
            return;
        }

        if (IsShowLimitBar(orderFood.CurrentOrderFoodState))
        {
            // ここに処理を書く
            orderFood.m_limitBarUIObject.SetActive(true);
        }
        else
        {
            orderFood.m_limitBarUIObject.SetActive(false);
            m_removeList.Add(orderFood);
        }
    }

    private bool IsShowLimitBar(OrderFoodInfo.OrderFoodState state)
    {
        if (state == OrderFoodInfo.OrderFoodState.Set)
        {
            return false;
        }

        return true;
    }

    private void Update()
    {
        foreach (var food in m_addList)
        {
            m_foodData.Add(food);
        }

        foreach (var food in m_foodData)
        {
            FoodCheck(food);
        }

        foreach (var food in m_removeList)
        {
            m_foodData.Remove(food);
        }

        m_addList.Clear();
        m_removeList.Clear();
    }

}

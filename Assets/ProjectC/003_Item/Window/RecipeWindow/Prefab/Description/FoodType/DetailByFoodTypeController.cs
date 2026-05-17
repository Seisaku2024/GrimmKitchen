using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 制作者 吉田
/// 
/// FoodTypeによる仕様を持つ
/// FoodDataをFoodWindowUpdateBaseに渡してUIを更新
/// 
/// ChangeFoodItemDescription で使用する
/// 
/// </summary>
public class DetailByFoodTypeController : MonoBehaviour
{
    [System.Serializable]
    ///<summary>
    /// フードタイプによる仕様 
    /// ※マネージャーやデータベースにした方がいいかも
    ///</summary>
    public struct FoodTypeSystem
    {
        public FoodData.FoodType m_foodType;
        [Header("可能な動作")]
        public bool m_eat;
        public bool m_put;
        public bool m_throw;

        public FoodTypeSystem(
            FoodData.FoodType _foodType = FoodData.FoodType.Heal,
            bool _eat = false, bool _put = false, bool _throw = false)
        {
            m_foodType = _foodType;
            m_eat = _eat;
            m_put = _put;
            m_throw = _throw;
        }
    }
    [Header("FoodTypeの仕様")]
    [SerializeField]
    private List<FoodTypeSystem> m_foodTypeSystemList = new();

    [SerializeField]
    private List<FoodWindowUpdateBase> m_foodWindowUpdate = new();

    private FoodData m_foodData = null;


    /// <summary>
    /// <para> 現在のFoodDataの仕様を取得する </para>
    /// </summary>
    public FoodTypeSystem GetFoodTypeSystem()
    {
        return GetFoodTypeSystem(m_foodData);
    }

    /// <summary>
    /// <para> FoodTypeから仕様を取得する </para>
    /// FoodTypeを渡すと、そのFoodTypeに対応するFoodTypeSystemを返す
    /// </summary>
    public FoodTypeSystem GetFoodTypeSystem(FoodData.FoodType _foodType)
    {
        foreach (var item in m_foodTypeSystemList)
        {
            if (item.m_foodType == _foodType)
            {
                return item;
            }
        }

        Debug.Log("FoodTypeSystemが見つかりませんでした");
        return new FoodTypeSystem();
    }
    
    /// <summary>
    /// <para> FoodDataの仕様を取得する </para>
    /// </summary>
    public FoodTypeSystem GetFoodTypeSystem(FoodData _foodData)
    {
        if (_foodData == null) return new FoodTypeSystem();
        return GetFoodTypeSystem(_foodData.GetFoodType());
    }


    /// <summary>
    /// FoodDataを取得
    /// </summary>
    public FoodData GetFoodData() { return m_foodData; }

    public void Initialize()
    {
        m_foodData = null;
        foreach (var item in m_foodWindowUpdate)
        {
            item.OnInitialize();
        }
    }

    public void SetFoodData(FoodData _foodData)
    {
        if (m_foodData == _foodData) return;

        m_foodData = _foodData;

        foreach (var item in m_foodWindowUpdate)
        {
            item.OnUpdate(_foodData);
        }
    }



    /*
    [Header("表示/非表示用")]
    [SerializeField]
    private GameObject m_ableCondition = null;
    [SerializeField]
    private GameObject m_disableCondition = null;

    [SerializeField]
    private GameObject m_ableHealing = null;
    [SerializeField]
    private GameObject m_disableHealing = null;

    [SerializeField]
    private GameObject m_ableAddStatus = null;
    [SerializeField]
    private GameObject m_disableAddStatus = null;
    

    public void SetFoodType(FoodData.FoodType _foodType)
    {
        foreach (var item in m_foodTypeUIList)
        {
            if (item.m_foodType != _foodType) continue;

            Eat(item.m_condition);
            Put(item.m_healing);
            Throw(item.m_addStatus);

        }
    }

    private void Eat(bool _active)
    {
        if (m_ableCondition != null)
        {
            m_ableCondition.SetActive(_active);
        }

        if (m_disableCondition != null)
        {
            m_disableCondition.SetActive(!_active);
        }
    }

    private void Put(bool _active)
    {
        if (m_ableHealing != null)
        {
            m_ableHealing.SetActive(_active);
        }

        if (m_disableHealing != null)
        {
            m_disableHealing.SetActive(!_active);
        }
    }

    private void Throw(bool _active)
    {
        if (m_ableAddStatus != null)
        {
            m_ableAddStatus.SetActive(_active);
        }

        if (m_disableAddStatus != null)
        {
            m_disableAddStatus.SetActive(!_active);
        }
    }
*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 仕様を取得して
/// アクションの表示非表示を切り替える
/// </summary>
public class EnableActionUIByFoodType : FoodWindowUpdateBase
{
    [Header("仕様の情報")]
    [SerializeField]
    private DetailByFoodTypeController m_detailByFoodTypeController = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private GameObject m_ableEat = null;
    [SerializeField]
    private GameObject m_disableEat = null;

    [SerializeField]
    private GameObject m_ablePut = null;
    [SerializeField]
    private GameObject m_disablePut = null;

    [SerializeField]
    private GameObject m_ableThrow = null;
    [SerializeField]
    private GameObject m_disableThrow = null;

    public override void OnInitialize()
    {
        if (m_detailByFoodTypeController == null)
        {
            Debug.LogError("DetailByFoodTypeControllerがアタッチされていません \n"
                + "gameObject.name : " + gameObject.name
                );
            return;
        }

        gameObject.SetActive(false);
    }

    public override void OnUpdate(FoodData _foodData)
    {
        gameObject.SetActive(false);
        if (m_detailByFoodTypeController == null) return;
        if (_foodData == null) return;

        gameObject.SetActive(true);
        // FoodTypeによる仕様を取得
        var foodTypeSystem =
            m_detailByFoodTypeController.GetFoodTypeSystem(_foodData);

        // 仕様によってUIを表示する
        if(foodTypeSystem.m_eat)
        {
            Eat(true);
        }
        else
        {
            Eat(false);
        }
        if (foodTypeSystem.m_put)
        {
            Put(true);
        }
        else
        {
            Put(false);
        }
        if (foodTypeSystem.m_throw)
        {
            Throw(true);
        }
        else
        {
            Throw(false);
        }
    }

    private void Eat(bool _active)
    {
        if (m_ableEat != null)
        {
            m_ableEat.SetActive(_active);
        }

        if (m_disableEat != null)
        {
            m_disableEat.SetActive(!_active);
        }
    }

    private void Put(bool _active)
    {
        if (m_ablePut != null)
        {
            m_ablePut.SetActive(_active);
        }

        if (m_disablePut != null)
        {
            m_disablePut.SetActive(!_active);
        }
    }

    private void Throw(bool _active)
    {
        if (m_ableThrow != null)
        {
            m_ableThrow.SetActive(_active);
        }

        if (m_disableThrow != null)
        {
            m_disableThrow.SetActive(!_active);
        }
    }


}

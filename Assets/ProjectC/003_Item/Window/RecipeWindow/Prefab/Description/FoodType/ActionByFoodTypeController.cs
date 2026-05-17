using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// FoodTypeによってプレイヤーができる事UIに反映させる　（吉田）
/// ChangeFoodItemDescription で使用する
/// 
/// </summary>
public class ActionByFoodTypeController : MonoBehaviour
{
    [System.Serializable]
    struct FoodTypeUI
    {
        public FoodData.FoodType m_foodType;
        public bool m_eat;
        public bool m_put;
        public bool m_throw;
    }
    [Header("FoodTypeの仕様")]
    [SerializeField]
    private List<FoodTypeUI> m_foodTypeUIList = new();

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

    public enum FoodActionType
    {
        eatFood,
        putFood,
        throwFood
    }
    public bool CheckAbleFoodActionType(FoodActionType type)
    {
        switch (type)
        {
            case FoodActionType.eatFood:
                return m_ableEat.activeSelf;
            case FoodActionType.putFood:
                return m_ablePut.activeSelf;
            case FoodActionType.throwFood:
                return m_ableThrow.activeSelf;
        }
        return false;
    }

    public void SetFoodType(FoodData.FoodType _foodType)
    {
        foreach (var item in m_foodTypeUIList)
        {
            if (item.m_foodType != _foodType) continue;

            Eat(item.m_eat);
            Put(item.m_put);
            Throw(item.m_throw);

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

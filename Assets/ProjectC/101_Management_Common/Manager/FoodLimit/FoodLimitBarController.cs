using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLimitBarController : MonoBehaviour
{
    [SerializeField]
    HPBarController m_HPBarController;

    public OrderFoodData m_orderFoodData;

    public void SetFoodData(OrderFoodData orderFoodData, float maxAngryCount, float currentAngryCount)
    {
        m_orderFoodData = orderFoodData;
        m_HPBarController.SetHealth(currentAngryCount, maxAngryCount);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var limitTime = m_orderFoodData.CustomerData.AngryTime;
        var currentTime = m_orderFoodData.CustomerData.AngryCount;

        m_HPBarController.SetHealthValue(limitTime - currentTime);
    }
}

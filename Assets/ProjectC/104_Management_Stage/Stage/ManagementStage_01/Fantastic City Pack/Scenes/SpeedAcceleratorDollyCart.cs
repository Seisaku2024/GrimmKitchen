using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedAccelerator : MonoBehaviour
{
    [SerializeField] private CinemachineDollyCart dollyCart = null;

    [SerializeField] private float Min_Speed = 1.0f;
    [SerializeField] private float Max_Speed = 5.0f;
    [SerializeField] private float SpeedAdjustment = 0.005f;

    private void Update()
    {
        UpdateSpeed();
    }

    void UpdateSpeed()
    {
        float speed = dollyCart.m_Speed;

        // 加減速
        float acceleration = 1 + (SpeedAdjustment * -gameObject.transform.forward.normalized.y);
        speed *= acceleration;

        // 速度制限
        if (speed < Min_Speed) speed = Min_Speed;
        else if (speed > Max_Speed) speed = Max_Speed;

        dollyCart.m_Speed = speed;
    }
}

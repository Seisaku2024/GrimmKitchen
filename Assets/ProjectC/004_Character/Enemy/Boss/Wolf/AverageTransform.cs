/**
* @file AverageTransform.cs
* @brief TransformListの平均座標を自分の座標に更新
*/
using System.Collections.Generic;
using UnityEngine;

/**
* @brief TransformListの平均座標を計算　伊波
* @details 狼が岩掴む処理に使用　計算結果は自身のTransformに反映
*/
public class AverageTransform : MonoBehaviour
{
    [SerializeField] private List<Transform> m_transforms = new();

    [System.Flags]
    private enum AverageTransformType
    {
        None = 0,
        Transform = 1,
        Rotation = 1 << 1,
        All = ~0,
    }

    [Header("種類")]
    [SerializeField]
    private AverageTransformType m_transformType = AverageTransformType.All;



    void FixedUpdate()
    {
        Vector3 sumPos = Vector3.zero;
        Vector3 sumRot = Vector3.zero;
        foreach (Transform t in m_transforms)
        {
            sumPos += t.position;


            sumRot += t.eulerAngles;
        }

        if (m_transformType.HasFlag(AverageTransformType.Transform))
        {
            // 座標更新
            transform.position = sumPos / m_transforms.Count;
        }

        // 回転更新
        if (m_transformType.HasFlag(AverageTransformType.Rotation))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(sumRot / m_transforms.Count), 0.5f * Time.fixedDeltaTime);
        }
    }
}

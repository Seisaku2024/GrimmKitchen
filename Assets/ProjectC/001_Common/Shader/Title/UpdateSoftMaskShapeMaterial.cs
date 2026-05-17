using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MaskingShape内のUpdateで毎フレームマテリアルを更新させるかのチェックボックス用（山本）
//  MaskingShapeにシリアライズをセットできなかったため、代用として使用
public class UpdateSoftMaskShapeMaterial : MonoBehaviour
{
    [Header("毎フレームMaskingShapeのマテリアル更新を行うか")]
    [SerializeField] private bool m_bUpdateMaskingShapeMaterial = false;

    public bool UpdateMaskingShapeMaterial { get { return m_bUpdateMaskingShapeMaterial; } }


}

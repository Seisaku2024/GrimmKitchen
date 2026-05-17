using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class BaseStaffCalculator : Calculator
{
    // 制作者　田内

    //===========================

    [Header("スタッフ情報")]
    [SerializeField]
    protected FlexibleStaffDataVariable m_flexibleStaffDataVariable = null;


    //=========================================
    // 出力

    [Header("出力:座標")]
    [SerializeField]
    protected OutputSlotVector3 m_outputPos = new();

    //==============================================================

    #region メソッド説明
    /// <summary>
    /// スタッフ情報が存在するかのチェック
    /// </summary>
    #endregion
    protected StaffData GetStaffData()
    {

        // スタッフ情報が存在するか確認

        if (m_flexibleStaffDataVariable == null)
        {
            Debug.LogError("スタッフ情報がシリアライズされていません");
            return null;
        }

        if (m_flexibleStaffDataVariable.value.StaffData == null)
        {
            Debug.LogError("スタッフ情報が存在しません");
            return null;
        }

        // 存在する
        return m_flexibleStaffDataVariable.value.StaffData;

    }

    // スタッフオブジェクトを取得する
    protected GameObject GetStaffGameObject()
    {
        var data = GetStaffData();
        if (data == null) return null;

        // 客のゲームオブジェクトを取得
        return data.gameObject;
    }

}

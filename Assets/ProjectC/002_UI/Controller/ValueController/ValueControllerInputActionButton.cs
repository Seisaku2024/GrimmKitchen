using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueControllerInputActionButton : InputActionButton
{
    // 制作者 田内



    [Header("コントローラー")]
    [SerializeField]
    private ValueController m_valueController = null;

    private enum ValueControllerType
    {
        Increment,
        Decrement,
        Decision,
    }

    [Header("対応するキータイプ")]
    [SerializeField]
    private ValueControllerType m_valueControllerType = ValueControllerType.Decision;

    //================================================
    //                 実行処理
    //================================================


    protected override bool IsPress()
    {
        #region nullチェック
        if (m_valueController == null)
        {
            Debug.LogError("ValueControllerがシリアライズされていません");
            return false;
        }
        #endregion

        switch (m_valueControllerType)
        {

            case ValueControllerType.Increment:
                {
                    return m_valueController.IsIncrement();
                }
            case ValueControllerType.Decrement:
                {
                    return m_valueController.IsDecrement();
                }
            case ValueControllerType.Decision:
                {
                    return m_valueController.IsDecision();
                }
            default:
                {
                    return false;
                }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 制作者　吉田
/// EnumElementの移動を制御するクラス
/// 
/// 注意
/// ** WindowUpdateBaseを継承しているので、WindowBaseのシリアライズにセットする必要がある **
/// 
/// </summary>
public class EnumElementMoveController : WindowUpdateBase
{
    [Header("※WindowBaseのシリアライズにセットする必要がある")]
    [Space(10)]

    [SerializeField]
    private ButtonData m_buttonData = null;
    [SerializeField]
    private EnumElementController m_enumElementController = null;

    [Header("インクリメントするInputAction")]
    [SerializeField]
    private InputActionButton m_incrementInputAction = null;

    [Header("デクリメントするInputAction")]
    [SerializeField]
    private InputActionButton m_decrementInputAction = null;


    public override void OnInitialize()
    {
        if (m_buttonData == null)
        {
            if(TryGetComponent(out ButtonData buttonData))
            {
                m_buttonData = buttonData;
            }
            else
            {
                Debug.LogError("ButtonDataが見つかりませんでした");
            }
        }       
        if (m_enumElementController == null)
        {
            if(TryGetComponent(out EnumElementController enumElementController))
            {
                m_enumElementController = enumElementController;
            }
            else
            {
                Debug.LogError("EnumElementControllerが見つかりませんでした");
            }
        }
    }

    public override void OnUpdate()
    {
        if (m_buttonData == null) return;
        if (m_enumElementController == null) return;

        // 選択されていない場合は処理しない
        if (!m_buttonData.IsSelect) return;

        // インクリメント
        if (m_incrementInputAction != null)
        {
            if (m_incrementInputAction.IsInputActionTrriger())
            {
                m_enumElementController.Increment();
            }
        }

        // デクリメント
        if (m_decrementInputAction != null)
        {
            if (m_decrementInputAction.IsInputActionTrriger())
            {
                m_enumElementController.Decrement();
            }
        }
    }
}

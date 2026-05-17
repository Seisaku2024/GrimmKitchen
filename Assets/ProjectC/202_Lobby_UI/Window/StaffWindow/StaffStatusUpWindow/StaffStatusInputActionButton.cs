using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffStatusInputActionButton : InputActionButton
{
    // 制作者 田内

    [Header("コントローラー")]
    [SerializeField]
    private StaffStatusUpController m_staffStatusUpController = null;

    private enum StaffStatusUpType
    {
        Decision,
    }

    [Header("チュートリアルボタン種類")]
    [SerializeField]
    private StaffStatusUpType m_staffStatusUpType = StaffStatusUpType.Decision;


    //========================================
    //               実行処理
    //========================================

}

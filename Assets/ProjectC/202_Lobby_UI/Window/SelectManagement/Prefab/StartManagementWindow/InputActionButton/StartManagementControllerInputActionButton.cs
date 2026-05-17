using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManagementControllerInputActionButton : InputActionButton
{

    // 制作者 田内
    // 経営開始ボタン

    [Header("ボタンデータ")]// 追加：吉田
    [SerializeField]
    private ButtonData m_buttonData = null;

    [Header("経営開始コントローラー")]
    [SerializeField]
    private StartManagementController m_startManagementController = null;


    //================================================
    //                  実行処理
    //================================================

    protected override void Update()
    {
        if (m_buttonData != null)// 吉田
        {
            // スタートできる状態かどうか
            if (m_startManagementController.IsStart())
            {
                SetAble();
            }
            else
            {
                SetDisable();
            }

            if (!m_buttonData.IsSelect) return;
        }

        base.Update();

    }

    protected override bool IsPress()
    {
        #region nullチェック
        if (m_startManagementController == null)
        {
            Debug.LogError("StartManagementControllerがシリアライズされていません");
            return false;
        }
        #endregion

        // スタッフが設置されている && 料理がセットされている
        if (m_startManagementController.IsStart())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 押せる場合の処理
    /// </summary>
    public void SetAble()
    {
        if (m_buttonData != null)// 吉田
        {
            m_buttonData.SetAble();
        }
    }

    /// <summary>
    /// 押せない場合の処理
    /// <summary>
    public void SetDisable()
    {
        if (m_buttonData != null)// 吉田
        {
            m_buttonData.SetDisable();
        }
    }

}

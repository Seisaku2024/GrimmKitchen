using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputToCreateWindow : MonoBehaviour
{
    // 制作者 田内
    // Input

    [Header("ウィンドウコントローラー")]
    [SerializeField]
    private WindowController m_windowController = null;

    [Header("作成キー")]
    [SerializeField]
    protected InputActionReference m_inputActionReference = null;

    //===============================================
    //                  実行処理
    //===============================================

    private void Update()
    {
        UpdateInput();
    }

    virtual protected void UpdateInput()
    {
        // ゲームが停止中であれば作成しない
        if (Time.timeScale <= 0.0f) return;

        #region nullチェック
        if (m_inputActionReference == null)
        {
            Debug.LogError("PlayerInputが登録されていません");
            return;
        }
        if (m_windowController == null)
        {
            Debug.LogError("WindowControllerがシリアライズされていません");
            return;
        }
        #endregion


        // 既に作成されていれば作成しない
        if (m_windowController.IsCreateWindow()) return;

        // ウィンドウを開く
        m_inputActionReference.action.Enable();
        if (m_inputActionReference.action.triggered)
        {
            _ = m_windowController.CreateWindow();
        }
    }
}

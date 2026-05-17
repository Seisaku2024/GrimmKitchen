using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using NaughtyAttributes;

using StageInfo;
using Cysharp.Threading.Tasks;

public class SelectStageController : MonoBehaviour
{

    // 制作者　吉田
    // ステージを選択するコントローラー

    //=====================
    // 選択中のステージID

    private StageID m_currentSelectStageID = StageID.Stage01;

    public StageID CurrentSelectStageID { get { return m_currentSelectStageID; } }

    //==================
    // ボタン

    [Header("SelectUIController")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    //==================
    // ボタン

    [Header("スタートボタン")]
    [SerializeField]
    private InputActionButton m_startInputActionButton = null;

    //===================================================
    //                  実行処理
    //===================================================

    public bool IsStart()
    {
        if (IsGoStart())
        {
            return m_startInputActionButton.IsInputActionTrriger();
        }

        return false;
    }

    // スタートできるかどうか
    public bool IsGoStart()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerがシリアライズされていません");
            return false;
        }

        if (m_selectUIController.IsPress == false) return false;
        if (m_selectUIController.CurrentSelectUI == null) return false;

        // ステージボタンからIDを取得
        if (m_selectUIController.CurrentSelectUI.TryGetComponent<StageButton>(out var button))
        {
            m_currentSelectStageID = button.StageID;
            return button.IsUse;
        }

        return false;
    }
}

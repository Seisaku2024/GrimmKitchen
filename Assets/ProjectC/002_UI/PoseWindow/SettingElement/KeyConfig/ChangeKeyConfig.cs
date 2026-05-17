using CI.QuickSave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

/// <summary>
/// 制作者　吉田
/// キーコンフィグ変更する際の処理
/// </summary>
public class ChangeKeyConfig : MonoBehaviour
{

    [SerializeField]
    ButtonData m_buttonData = null;

    [Header("キーコンフィグ")]
    [SerializeField]
    private InputActionButton m_inputActionButton = null;

    [SerializeField] private GameObject m_maskObj;// リバインド中に表示するマスク

    /// <summary>
    /// リバインド非同期オペレーション
    /// 押すのを待つためのやつ
    /// </summary>
    RebindingOperation m_rebindingOperation;


    public void StartRebiding()
    {
        if (!this) return;

        // もしリバインド中なら、強制的にキャンセル
        // Cancelメソッドを実行すると、OnCancelイベントが発火する
        m_rebindingOperation?.Cancel();
        // リバインド前にActionを無効化する必要がある
        m_inputActionButton.InputActionReference.action.Disable();

        int bindingIndex = -1;// 全体
        switch (PlayerInputManager.instance.CurrentDeviceTypes)
        {
            case DeviceTypes.KeyboardMouse:
                bindingIndex = 1;
                break;
            case DeviceTypes.XBOX:
                bindingIndex = 0;
                break;
            case DeviceTypes.PlayStation:
                bindingIndex = 0;
                break;
            case DeviceTypes.Switch:
                bindingIndex = 0;
                break;
        }
        //int bindingIndex = m_inputActionButton.InputActionReference.action.
        //    GetBindingIndex(InputBinding.MaskByGroup(deviceType));
        ////bindingIndex = 1;

        //待ち受け開始用のゲームオブジェクトを表示
        // Imageがついているなら他のUIがキーの影響を受けない
        m_maskObj.SetActive(true);

        //リバインドオペレーションを開始
        m_rebindingOperation = m_inputActionButton.InputActionReference.action.
            PerformInteractiveRebinding(bindingIndex)
            // 完了時の処理
            .OnComplete((operation) =>
            {
                m_inputActionButton.InputActionReference.action.Enable();
                m_inputActionButton.UpdateButtonImage();// ボタンの画像を更新
                m_maskObj.SetActive(false);
            })
        // キャンセル時の処理
            .OnCancel((operation) =>
            {
                m_rebindingOperation.Dispose();// これしないとメモリリークする
                m_rebindingOperation = null;
                m_inputActionButton.InputActionReference.action.Enable();
                m_maskObj.SetActive(false);
            })
            //キャンセルキーを設定
            .WithCancelingThrough("<Keyboard>/escape")//escキーが割り当てられなくなる。
            .Start();

    }

    private void Start()
    {
        if (m_buttonData == null) return;

        // 変更できないようにする
        //m_buttonData.AddOnPressEvent(StartRebiding);
    }

    private void CheckForConflicts()
    {
    
    }


}

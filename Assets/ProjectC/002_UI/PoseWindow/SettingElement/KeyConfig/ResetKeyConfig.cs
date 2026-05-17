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
public class ResetKeyConfig : MonoBehaviour
{

    [SerializeField]
    ButtonData m_buttonData = null;

    [SerializeField]
    private InputActionAsset m_inputActionsAsset = null;

    public void ResetConfig()
    {
        if (!this) return;

        m_inputActionsAsset.RemoveAllBindingOverrides();

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
        foreach (var action in m_inputActionsAsset.actionMaps)
        {
            foreach(var inputAction in action.actions)
            {
                if (inputAction.bindings.Count <= bindingIndex) continue;
                inputAction.RemoveBindingOverride(bindingIndex);
            }
        }

        // Inputbutton の更新をしないと、画像が更新されない

    }

    private void Start()
    {
        if (m_buttonData == null) return;
        
        m_buttonData.AddOnPressEvent(ResetConfig);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using Cysharp.Threading.Tasks;

/// <summary>
/// 制作者 吉田
/// 設定で表示デバイスを切り替える時の処理
/// </summary>
public class ViewDeviceTypePresenter : MonoBehaviour
{
    [SerializeField]
    private ButtonData m_buttonData = null;

    [SerializeField]
    private EnumElementController m_enumElementController = null;

    private void Start()
    {
        if (m_buttonData == null)
        {
            if (!TryGetComponent(out m_buttonData))
            {
                Debug.LogError("ButtonDataがセットされていません");
            }
        }
        if (m_enumElementController == null)
        {
            Debug.LogError("m_enumElementControllerがセットされていません");
        }

        Initialized();

        // 値が変更された時の処理を追加
        m_enumElementController.AddOnChangeIndexEvent(SwitchViewDeviceType);
    }

    private void SwitchViewDeviceType()
    {
        if (!this) return;
        if (m_enumElementController == null) return;

        PlayerInputManager.instance.SetViewDeviceType(
            IndexToDiviceType(m_enumElementController.EnumIndex));
    }

    private void Initialized()
    {
        if (m_enumElementController == null) return;

        DeviceTypes deviceTypes = PlayerInputManager.instance.ViewDeviceType;
        int index = DiviceTypeToIndex(deviceTypes);
        m_enumElementController.SetEnumIndex(index);
    }

    // =================================================
    // DeviceType ⇔ Index

    private DeviceTypes IndexToDiviceType(int _index)
    {
        DeviceTypes deviceTypes = (DeviceTypes)_index;
        return deviceTypes;
    }

    private int DiviceTypeToIndex(DeviceTypes _deviceTypes)
    {
        int index = (int)_deviceTypes;
        return index;
    }

}

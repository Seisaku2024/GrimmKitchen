using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public enum DeviceTypes
{
    None,
    KeyboardMouse,
    XBOX,
    PlayStation,
    Switch,
}

public partial class PlayerInputManager : BaseManager<PlayerInputManager>
{
    // 制作者 田内
    // 入力デバイスによる変更


    [Header("データベース")]
    [SerializeField]
    InputActionButtonDataBase m_inputActionButtonDataBase = null;

    public InputActionButtonDataBase InputActionButtonDataBase
    {
        get { return m_inputActionButtonDataBase; }
    }

    //============================
    // 入力デバイスが変更された時

    private ReactiveProperty<DeviceTypes> m_currenrDevice = new(DeviceTypes.None);

    public System.IObservable<DeviceTypes> CurrentDevice
    {
        get { return m_currenrDevice; }
    }

    public DeviceTypes CurrentDeviceTypes
    {
        get { return m_currenrDevice.Value; }
    }

    //============================
    // 入力デバイス
    private DeviceTypes m_inputDeviceType = DeviceTypes.None;

    //============================
    // UI表示デバイス
    private DeviceTypes m_viewDeviceType = DeviceTypes.None;
    public DeviceTypes ViewDeviceType
    {
        get { return m_viewDeviceType; }
    }

    public void SetViewDeviceType(DeviceTypes _deviceType)
    {
        m_viewDeviceType = _deviceType;
        UpdateCurrentDevice();
    }

    //==================
    // 入力検知

    InputAction m_detrctionKeyboard = new InputAction(
        type: InputActionType.PassThrough,
        binding: "<Keyboard>/AnyKey",
        interactions: "Press");

    InputAction m_detrctionXBOX = new InputAction(
       type: InputActionType.PassThrough,
       binding: "<XInputController>/*",
       interactions: "Press");

    InputAction m_detrctionPlayStation = new InputAction(
       type: InputActionType.PassThrough,
       binding: "<DualShockGamepad>/*",
       interactions: "Press");

    InputAction m_detrctionSwitch = new InputAction(
              type: InputActionType.PassThrough,
                    binding: "<SwitchProControllerHID>/*",
                          interactions: "Press");

    //=====================================================
    //                      実行処理
    //=====================================================

    //入力デバイスの監視
    private void ObservationDevice()
    {
        m_detrctionKeyboard.Enable();
        m_detrctionXBOX.Enable();
        m_detrctionPlayStation.Enable();
        m_detrctionSwitch.Enable();

        //キーマウス
        if (m_detrctionKeyboard.triggered ||
        (Mouse.current != null && Mouse.current.delta.magnitude > 0))
        {
            m_inputDeviceType = DeviceTypes.KeyboardMouse;
        }
        //XBOX
        else if (m_detrctionXBOX.triggered)
        {
            m_inputDeviceType = DeviceTypes.XBOX;
        }
        //Playstation
        else if (m_detrctionPlayStation.triggered)
        {
            m_inputDeviceType = DeviceTypes.PlayStation;
        }
        //Switch
        else if (m_detrctionSwitch.triggered)
        {
            m_inputDeviceType = DeviceTypes.Switch;
        }

        // m_currenrDevice の更新（吉田）
        UpdateCurrentDevice();
    }

    /// <summary>
    /// m_currenrDevice の更新（吉田）
    /// 表示デバイスが設定されているかどうかで分岐
    /// </summary>
    private void UpdateCurrentDevice()
    {
        // 表示デバイスが設定されている場合
        if (m_viewDeviceType != DeviceTypes.None)
        {
            m_currenrDevice.Value = m_viewDeviceType;
            return;
        }
        // 表示がNoneの場合
        else
        {
            m_currenrDevice.Value = m_inputDeviceType;
        }
    }

   
}

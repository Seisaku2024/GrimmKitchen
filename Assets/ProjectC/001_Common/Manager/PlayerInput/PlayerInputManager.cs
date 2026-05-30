using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.SceneManagement;

public enum InputActionMapTypes
{
    Player,
    Camera,
    UI,
}

[RequireComponent(typeof(PlayerInput))]
public partial class PlayerInputManager : BaseManager<PlayerInputManager>
{

    // 制作者 田内(他)
    // PlayerInputを管理するマネージャー

    //=============
    // 入力感知
    PlayerInput m_playerInput = null;

    //=============
    // ActionMap

    private Dictionary<InputActionMapTypes, InputActionMap> m_inputActionMapDictionary = new();



    //==============================================
    //              実行処理
    //==============================================

    protected override void StartInstance()
    {
        // PlayerInputを取得
        gameObject.TryGetComponent(out m_playerInput);

        // InputActionMapの動作を開始
        {
            m_inputActionMapDictionary[InputActionMapTypes.Player] = m_playerInput.actions.FindActionMap("Player");
            m_inputActionMapDictionary[InputActionMapTypes.Player].Enable();

            m_inputActionMapDictionary[InputActionMapTypes.Camera] = m_playerInput.actions.FindActionMap("Camera");
            m_inputActionMapDictionary[InputActionMapTypes.Camera].Enable();

            m_inputActionMapDictionary[InputActionMapTypes.UI] = m_playerInput.actions.FindActionMap("UI");
            m_inputActionMapDictionary[InputActionMapTypes.UI].Enable();
        }

        // シーンが切り替わったら動作させる
        void SceneInput(Scene scene, LoadSceneMode mode)
        {
            GetInputActionMap(InputActionMapTypes.Player).Enable();
            GetInputActionMap(InputActionMapTypes.UI).Enable();
            GetInputActionMap(InputActionMapTypes.Camera).Enable();
        }

        SceneManager.sceneLoaded += SceneInput;
    }


    /// <summary>
    /// なにかキーが選択されれば
    /// </summary>
    public bool IsPressCurrentAnyKey()
    {
        if (m_detrctionKeyboard.triggered) return true;
        if (m_detrctionXBOX.triggered) return true;
        if (m_detrctionPlayStation.triggered) return true;
        if (m_detrctionSwitch.triggered) return true; ;

        return false;
    }


    private void Update()
    {
        // 入力デバイスの監視
        ObservationDevice();

        // プレイヤーのActionMapの制御
        SetActivePlayerActionMap();
    }


    private void OnDestroy()
    {
        // メモリリーク対策
        m_currenrDevice.Dispose();
    }



    /// <summary>
    /// 引数InputActionMapを取得
    /// </summary>
    public InputActionMap GetInputActionMap(InputActionMapTypes _type)
    {
        if (m_inputActionMapDictionary.TryGetValue(_type, out InputActionMap inputActionMap))
        {
            return inputActionMap;
        }
        else
        {
            Debug.LogError(_type.ToString() + "にはInputActionMapが存在しません");
            return null;
        }
    }


    /// <summary>
    /// 引数InputActionMapのアクションを取得
    /// </summary>
    public InputAction GetInputAction(InputActionMapTypes _type, string _action)
    {
        InputActionMap inputActionMap = GetInputActionMap(_type);
        if (inputActionMap == null) return null;

        InputAction inputAction = inputActionMap.FindAction(_action);

        if (inputAction == null)
        {
            Debug.LogError(_type.ToString() + "Mapには" + _action + "というInputActionは存在しません");
        }

        return inputAction;
    }


    /// <summary>
    /// 引数InputActionのトリガー 
    /// 連打等はこれで取得しなければならない
    /// </summary>
    public bool IsInputActionTrigger(InputActionMapTypes _type, string _action)
    {
        var action = GetInputAction(_type, _action);

        if (action == null)
        {
            Debug.LogError("このアクションは存在しません");
            return false;
        }

        return action.triggered;
    }


    /// <summary>
    /// 引数InputActionが押されたか
    /// 押された1フレームのみtrueを返す
    /// </summary>
    public bool IsInputActionWasPressed(InputActionMapTypes _type, string _action)
    {

        var action = GetInputAction(_type, _action);

        if (action == null)
        {
            Debug.LogError("このアクションは存在しません");
            return false;
        }

        return action.WasPressedThisFrame();

    }


    /// <summary>
    /// 引数InputActionが押され続けているか
    /// </summary>
    public bool IsInputActionPressed(InputActionMapTypes _type, string _action)
    {
        var action = GetInputAction(_type, _action);

        if (action == null)
        {
            Debug.LogError("このアクションは存在しません");
            return false;
        }

        return action.IsPressed();
    }



    /// <summary>
    /// 引数InputActionが離されたか
    /// 離された1フレームのみtrueを返す
    /// </summary>
    public bool IsInputActionWasReleased(InputActionMapTypes _type, string _action)
    {

        var action = GetInputAction(_type, _action);

        if (action == null)
        {
            Debug.LogError("このアクションは存在しません");
            return false;
        }

        return action.WasReleasedThisFrame();
    }


    // カメラ入力とプレイヤー入力を止める関数
    public void SetGameplayInputActive(bool isActive)
    {
        SetActionMapActive(InputActionMapTypes.Player, isActive);
        SetActionMapActive(InputActionMapTypes.Camera, isActive);
    }

    public void SetActionMapActive(InputActionMapTypes type, bool isActive)
    {
        InputActionMap actionMap = GetInputActionMap(type);
        if (actionMap == null) return;

        if (isActive)
        {
            actionMap.Enable();
        }
        else
        {
            actionMap.Disable();
        }
    }


}

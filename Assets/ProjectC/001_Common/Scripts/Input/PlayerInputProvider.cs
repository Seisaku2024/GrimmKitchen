using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// PlayerInputProvider (濱口)

// PlayerInputを使用した操作キャラクターへのInput処理
public class PlayerInputProvider : IInputProvider
{
    Camera m_camera;
    private bool m_waitForManagementAttackRelease = true;
    private int m_managementAttackReleaseFrame = -1;

    private bool IsManagementInputBlocked()
    {
        var managementState = ManagementStateUpdateManager.instance;
        if (managementState == null || managementState.gameObject.scene.name != "ManagementScene")
            return false;

        bool isStarting = managementState.IsState((int)ManagementStateUpdateInfo.ManagementState.Standby) ||
                          managementState.IsState((int)ManagementStateUpdateInfo.ManagementState.Start);
        if (isStarting) m_waitForManagementAttackRelease = true;
        return isStarting;
    }

    public Vector3 LookVector { get; } = Vector3.zero;

    //　カメラの向きに合った方向を返す(倉田)
    public Vector3 MoveVector
    {
        get
        {
            if (IsManagementInputBlocked()) return Vector3.zero;

            if (m_camera == null) m_camera = Camera.main;

            // カメラの向き
            Vector3 vForward = m_camera.transform.forward;
            vForward.y = 0;
            // 入力ベクトル
            var axis = PlayerInputManager.instance.GetInputAction(InputActionMapTypes.Player, "Move").ReadValue<Vector2>();

            //Normarizeしないように修正。（山本）
            return Quaternion.LookRotation(vForward) *　new Vector3(axis.x, 0, axis.y);
        }
    }

    // int型で、攻撃の種類を決定する
    public int AttackType
    {
        get
        {
            // 経営開始の決定入力を攻撃として扱わず、離した次のフレームから受け付ける。
            var managementState = ManagementStateUpdateManager.instance;
            if (managementState != null && managementState.gameObject.scene.name == "ManagementScene")
            {
                if (managementState.IsState((int)ManagementStateUpdateInfo.ManagementState.Standby) ||
                    managementState.IsState((int)ManagementStateUpdateInfo.ManagementState.Start))
                {
                    m_waitForManagementAttackRelease = true;
                    return 0;
                }

                if (m_waitForManagementAttackRelease)
                {
                    var attackAction = PlayerInputManager.instance.GetInputAction(InputActionMapTypes.Player, "Attack");
                    if (attackAction == null || !attackAction.enabled) return 0;

                    // Actionの再有効化直後にも、実際のボタンの押下状態を確認する。
                    foreach (var control in attackAction.controls)
                    {
                        if (control is UnityEngine.InputSystem.Controls.ButtonControl button && button.isPressed)
                            return 0;
                    }

                    m_waitForManagementAttackRelease = false;
                    m_managementAttackReleaseFrame = Time.frameCount;
                }

                if (Time.frameCount <= m_managementAttackReleaseFrame) return 0;
            }

            //IsPressed->triggeredに変更(山本)
            if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "Attack"))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        set { }
    }

    public bool DoDush { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionPressed(InputActionMapTypes.Player, "Dush"); } }
    public bool OnPressedDush { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.Player, "Dush"); } }
    public bool OnReleasedDush { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionWasReleased(InputActionMapTypes.Player, "Dush"); } }


    //ローリングトリガー（山本）
    //WasPressedThisFrameに変更（山本）
    public bool DoRolling { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.Player, "Rolling"); } }

    public bool SelectLeftItem { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "SelectLeftItem"); } }
    public bool SelectRightItem { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "SelectRightItem"); } }
    public bool UseItem { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "UseItem"); } }

    //童話スキル1使用トリガー
    //IsPressed->triggeredに変更(山本)
    public bool UseStorySkill_1 { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "UseStorySkill_1"); } }
    //童話スキル2使用トリガー
    public bool UseStorySkill_2 { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "UseStorySkill_2"); } }


    // アイテム採取
    public bool Gathering { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "Gathering"); } }

    // 掃除（経営パート）
    public bool Cleanning { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "Cleanning"); } }

    // アイテム設置
    public bool PutItem { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "PutItem"); } }

    // アイテム投げる準備
    public bool ReadyThrowItem { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "ReadyThrowItem"); } }

    // アイテム投げる
    public bool ThrowItem { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "ThrowItem"); } }

    // アイテム投げる準備時、投げる方向（マウス座標）
    public Vector2 ThrowAim
    {
        get
        {
            if (IsManagementInputBlocked()) return Vector2.zero;

            var mouseAction = PlayerInputManager.instance.GetInputAction(InputActionMapTypes.Player, "ThrowAim");
            return mouseAction.ReadValue<Vector2>();
        }
    }

    //キャンセル
    public bool Cancel { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.Player, "Cancel"); } }

    //食べる
    public bool Eat { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.Player, "Eat"); } }

    // 敵側
    public bool IsArrive { get; } = false;

    public Vector3 Destination { get; } = Vector3.zero;

    // 探索
    public bool Search { get { return !IsManagementInputBlocked() && PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.Player, "Search"); } }

}

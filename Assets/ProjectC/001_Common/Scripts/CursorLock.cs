using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLock : MonoBehaviour
{

    [SerializeField] PlayerInput m_input;
    bool m_isLock = false;

    // Start is called before the first frame update
    void Start()
    {
        m_isLock = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 指定されたキーが押されたらロックを解除してカーソルも目視できるようにする（山本）

        if (m_input)
        {
            if (m_input.actions["CursorLock"].WasPressedThisFrame())
            {
                if (m_isLock == false)
                {
                    m_isLock = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    m_isLock = false;
                }

            }
        }


        if (m_isLock == false)
        {
            return;
        }


        if (Mathf.Abs(Screen.width / 2 - Mouse.current.position.ReadValue().x) > 50
            || Mathf.Abs(Screen.height / 2 - Mouse.current.position.ReadValue().y) > 50)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {

            Cursor.lockState = CursorLockMode.None;
        }

    }
}

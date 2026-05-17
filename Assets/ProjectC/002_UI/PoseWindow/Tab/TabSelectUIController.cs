using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 制作者　吉田
/// TabRight　TabLeft に割り当てされているキーで
/// 左右のみ移動
/// </summary>
public class TabSelectUIController : SelectUIController
{

    public override async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_alwaysCreateType == AlwaysCreateType.Enable)
            {
                NullCheck();
            }

            // UIを選択する
            SelectUI();

            //// UIのキー操作判定
            //await OnUpdateInput();
            //cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    protected override void SelectUI()
    {
        if (Keyboard.current?.anyKey.wasPressedThisFrame == false && Gamepad.current?.wasUpdatedThisFrame == false) return;


        if (m_uiList.Count <= 0)
        {
            return;
        }

        bool flg = false;

        // 左に進む
        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, "TabLeft"))
        {
            flg = Left();
        }


        // 右に進む
        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, "TabRight"))
        {
            flg = Right();
        }


        // 上に進む
        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, "Up"))
        {
            //flg = Up();
        }


        // 下に進む
        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, "Down"))
        {
            //flg = Down();
        }

        if (flg)
        {
            m_currentSelectUIData = m_uiList[m_currentHeight].List[m_currentWidth];
            m_isSelectChangeFlg = true;

            //PlaySelectSound();

            ButtonDataUnselectUI();
            ButtonDataSelectUI();

        }
    }


}

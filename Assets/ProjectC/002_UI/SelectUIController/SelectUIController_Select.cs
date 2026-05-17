using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using SelectUIInfo;

namespace SelectUIInfo
{
    // UIの選択種類列挙型
    public enum SelectUIType
    {
        Press,
        Hold,
    }
}


public partial class SelectUIController : MonoBehaviour
{
    // 制作者 田内
    // UI選択の処理

    private string m_pressInputAction = "Select";

    public string PressInputAction
    {
        get { return m_pressInputAction; }
    }

    private string m_holdInputAction = "HoldSelect";

    public string HoldInputAction
    {
        get { return m_holdInputAction; }
    }


    //==========================================
    //                 実行処理
    //==========================================

    // 選択した場合
    private async UniTask OnUpdateInput()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            if (m_currentSelectUIData == null) return;

            SelectUIType type = m_currentSelectUIData.SelectUIType;
            switch (type)
            {
                // 単押し
                case SelectUIType.Press:
                    {
                        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, m_pressInputAction) == true)
                        {
                            await PressUpdate();
                            cancelToken.ThrowIfCancellationRequested();
                        }
                        break;
                    }
                // 長押し
                case SelectUIType.Hold:
                    {
                        if (PlayerInputManager.instance.IsInputActionTrigger(InputActionMapTypes.UI, m_holdInputAction) == true)
                        {
                            await PressUpdate();
                            cancelToken.ThrowIfCancellationRequested();
                        }
                        break;
                    }
            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // ButtonDataの実行処理
    private async UniTask OnUpdatePressButtonData()
    {
        if (m_currentSelectUIData == null) return;

        // ボタン情報を取得
        if (m_currentSelectUIData.ButtonData != null)
        {
            await m_currentSelectUIData.ButtonData.OnPressUpdate();
        }
    }


    private async UniTask PressUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 音声再生
            PlayPressSound();

            // サイズを変更
            if (m_isPressScale) DoScale();

            // ボタン実行処理
            await OnUpdatePressButtonData();
            cancelToken.ThrowIfCancellationRequested();

            // 選択処理
            m_isPress = true;
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

}

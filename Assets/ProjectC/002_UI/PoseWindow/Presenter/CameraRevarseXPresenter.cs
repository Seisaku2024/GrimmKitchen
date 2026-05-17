using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRevarseXPresenter : MonoBehaviour
{
    [SerializeField]
    private ButtonData m_buttonData = null;

    [SerializeField]
    private ToggleElementController m_toggleElementController = null;

    [SerializeField]
    private InputActionReference m_inputAction = null;


    // Start is called before the first frame update
    void Start()
    {
        if (m_buttonData == null)
        {
            if (!TryGetComponent(out m_buttonData))
            {
                Debug.LogError("ButtonDataがセットされていません");
            }
        }
        if (m_toggleElementController == null)
        {
            Debug.LogError("ToggleElementControllerがセットされていません");
        }

        Initialized();

        // 決定ボタンが押された時の処理を追加
        m_buttonData.AddOnPressEvent(Switch);
    }

    private void Initialized()
    {
        if (m_toggleElementController == null) return;
        if (m_inputAction == null) return;

        // InputBinding情報を取得
        var binding = m_inputAction.action.bindings[0];
        string processors = binding.overrideProcessors;
        if (string.IsNullOrEmpty(processors))
        {
            processors = "InvertVector2(invertX=true,invertY=true)";
        }

        // InvertYの値を取得
        bool? invertX = GetInvertValue(processors);
        if (invertX == null)
        {
            Debug.LogError("InvertYの値が取得できませんでした");
            return;
        }

        // ToggleElementControllerの値を設定 初期設定がtrueなので、表示は反転している
        m_toggleElementController.SetValue(!invertX.Value);
        ChangeCameraRevarse();
    }

    private void Switch()
    {
        if (!this) return;
        if (m_toggleElementController == null) return;

        m_toggleElementController.Switch();
        ChangeCameraRevarse();
    }

    private void ChangeCameraRevarse()
    {
        if (m_toggleElementController == null) return;
        if (m_inputAction == null) return;

        // InputBinding を変更
        for (int i = 0; i < m_inputAction.action.bindings.Count; i++)
        {
            var binding = m_inputAction.action.bindings[i];
            string processors = binding.overrideProcessors;
            if (string.IsNullOrEmpty(processors))
            {
                processors = "InvertVector2(invertX=true,invertY=true)";
            }

            //　初期設定がtrueなので、表示は反転している
            processors = SetInvertValues(processors, !m_toggleElementController.Value);
            // Bindingの変更
            m_inputAction.action.ApplyBindingOverride(i, new InputBinding
            {
                path = binding.path,
                interactions = binding.interactions,
                overrideProcessors = processors
            });

        }
    }

    // `invertY` の値を設定
    private string SetInvertValues(string input, bool invertX)
    {
        input = Regex.Replace(input, @"invertX=\w+", $"invertX={invertX.ToString().ToLower()}");
        //input = Regex.Replace(input, @"invertY=\w+", $"invertY={invertY.ToString().ToLower()}");
        return input;
    }

    // `key` に対応する `true` または `false` を取得
    // 値が見つからなかった場合はnullを返す
    static bool? GetInvertValue(string input)
    {
        Match match = Regex.Match(input, $@"invertX=(true|false)");
        if (match.Success)
        {
            return bool.Parse(match.Groups[1].Value);
        }
        return null;
    }
}

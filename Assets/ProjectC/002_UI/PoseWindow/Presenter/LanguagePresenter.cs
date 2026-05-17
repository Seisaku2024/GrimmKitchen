using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using Cysharp.Threading.Tasks;

/// <summary>
/// 制作者 吉田
/// 設定で言語を切り替える時の処理
/// </summary>
public class LanguagePresenter : MonoBehaviour
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
        m_enumElementController.AddOnChangeIndexEvent(SwitchLanguage);
    }

    private void SwitchLanguage()
    {
        if (!this) return;
        if (m_enumElementController == null) return;

        string localeCode = IndexToString(m_enumElementController.EnumIndex);
        ChangeSelectedLocale(localeCode);
    }

    private async UniTask ChangeSelectedLocale(string _localeCode)
    {
        // Set locale with locale code.
        LocalizationSettings.SelectedLocale = Locale.CreateLocale(_localeCode);

        // Wait initialization.
        await LocalizationSettings.InitializationOperation.Task;
    }

    private void Initialized()
    {
        if (m_enumElementController == null) return;

        string localeCode = LocalizationSettings.SelectedLocale.Identifier.Code;
        int index = StringToIndex(localeCode);
        m_enumElementController.SetEnumIndex(index);
    }

    // =================================================
    // String ⇔ Index

    private string IndexToString(int _index)
    {
        string localeCode;
        switch (m_enumElementController.EnumIndex)
        {
            case 0:
                localeCode = "ja";
                break;
            case 1:
                localeCode = "en";
                break;
            case 2:
                localeCode = "zh";
                break;

            default:
                localeCode = "ja";
                break;
        }
        return localeCode;
    }

    private int StringToIndex(string _localeCode)
    {
        int index;
        switch (_localeCode)
        {
            case "ja":
                index = 0;
                break;
            case "en":
                index = 1;
                break;
            case "zh":
                index = 2;
                break;

            default:
                index = 0;
                break;
        }
        return index;
    }

}

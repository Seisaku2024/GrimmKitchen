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
public class QualityTypePresenter : MonoBehaviour
{
    [SerializeField]
    private ButtonData m_buttonData = null;

    [SerializeField]
    private EnumElementController m_enumElementController = null;


    [Header("LOD補正値")]
    [SerializeField]
    private float[] m_lodBias = new float[5];

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

        QualitySettings.SetQualityLevel(
            IndexToQualityLevel(m_enumElementController.EnumIndex), true);
        QualitySettings.SetLODSettings(m_lodBias[m_enumElementController.EnumIndex], 0);

        Debug.Log("Change QualityLevel : " + QualitySettings.names[m_enumElementController.EnumIndex]);
    }

    private void Initialized()
    {
        if (m_enumElementController == null) return;

        int index = QualityLevelToIndex(QualitySettings.GetQualityLevel());
        m_enumElementController.SetEnumIndex(index);
        QualitySettings.SetLODSettings(m_lodBias[m_enumElementController.EnumIndex], 0);
    }

    // =================================================
    // QualityLevel ⇔ Index

    private int IndexToQualityLevel(int _index)
    {
        return _index + 2;
        //switch (_index)
        //{
        //    case 0:
        //        return DeviceTypes.PC;
        //    case 1:
        //        return DeviceTypes.SmartPhone;
        //    case 2:
        //        return DeviceTypes.Tablet;
        //    case 3:
        //        return DeviceTypes.VR;
        //    default:
        //        return DeviceTypes.PC;
        //}
    }

    private int QualityLevelToIndex(int _quality)
    {
        return _quality - 2;
    }

}

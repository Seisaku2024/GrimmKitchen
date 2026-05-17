using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 進捗率に応じて
/// ・imageのfillAmountを変更する
/// ・針オブジェクトを回す
/// 
/// </summary>

public class ManagementTimerController : MonoBehaviour, ISerializationCallbackReceiver
{

    [SerializeField]
    [Tooltip("値変えると長さ変わる Scene確認用")]
    [Range(0f, 1f)]
    private float m_value = 0f;
    public float Value { get { return m_value; } }

    /// <summary>
    /// 時間の最大値　（0より大きい値）
    /// </summary>
    [SerializeField]
    private float m_max = 1f;
    public float Max
    {
        get { return m_max; }
        set { m_max = value; }
    }

    /// <summary>
    /// maxから値を計算して返す
    /// </summary>
    public float GetCurrentValue()
    {
        return m_value * m_max;
    }

    [SerializeField]
    [Space(15)]
    [Header("FillAmount を動かすImage")]
    private Image m_updateFillAmountImage = null;

    [SerializeField]
    [Header("針")]
    private GameObject m_clockHands = null;


    // Start is called before the first frame update
    void Start()
    {
        m_max = ManagementGameDataManager.instance.TimeLimit;

        // 時間で0以下は存在しないため、max値は1を最小とする
        if (m_max <= 0)
        {
            m_max = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 経過時間
        var elapsedTime = ManagementGameDataManager.instance.CurrentElapsedTime;
        m_value = (m_max - elapsedTime) / m_max;

        UpdateFillAmount();
        UpdateClockHands();
    }

    private void UpdateFillAmount()
    {
        if (m_updateFillAmountImage == null) return;

        m_updateFillAmountImage.fillAmount = m_value;
    }

    private void UpdateClockHands()
    {
        if (m_clockHands == null) return;

        float angleZ = m_value * 360.0f;
        Vector3 rotateEuler = m_clockHands.transform.localEulerAngles;
        rotateEuler.z = angleZ;
        m_clockHands.transform.localEulerAngles = rotateEuler;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UpdateFillAmount();
        UpdateClockHands();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

public abstract class ValueController : MonoBehaviour
{
    // 制作者 田内
    // 値を操作するコントローラー(基底クラス)


    [Header("増やすボタン")]
    [SerializeField]
    protected InputActionButton m_incrementInputActionButton = null;

    [Header("減らすボタン")]
    [SerializeField]
    protected InputActionButton m_decrementInputActionButton = null;

    [Header("決定ボタン")]
    [SerializeField]
    protected InputActionButton m_decisionInputActionButton = null;

    [SerializeField]
    [Header("スライダー")]
    protected Slider m_valueSlider = null;

    [SerializeField]
    [Header("値を表示するテキスト")]
    protected TextMeshProUGUI m_text = null;


    [SerializeField]
    [Header("可能時に表示するオブジェクト")]
    protected GameObject m_possibleObject = null;

    [SerializeField]
    [Header("不可能時に表示するオブジェクト")]
    protected GameObject m_impossibleObject = null;


    //==================================

    // 現在値
    protected int m_currentValue = 0;
    public int CurrentValue
    {
        get { return m_currentValue; }
    }


    // 最小値
    protected int m_minValue = 1;
    public int MinValue
    {
        get { return m_minValue; }
    }


    // 最大値
    protected int m_maxValue = 1;
    public int MaxValue
    {
        get { return m_maxValue; }
    }

    //====================
    // 変更されたかどうか

    protected bool m_isSelectChangeFlg = false;

    public bool IsSelectChangeFlg { get { return m_isSelectChangeFlg; } }

    //=========================================
    //              実行処理
    //=========================================

    private void Start()
    {
        SetData();
        SetActiveObject();
    }

    /// <summary>
    /// 実行処理
    /// </summary>
    virtual public async UniTask OnUpdate()
    {
        // 選択
        Select();

        // アクティブセット
        SelectChangeToSetActiveObject();

        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 後実行処理
    /// </summary>
    virtual public void OnLateUpdate()
    {
        m_isSelectChangeFlg = false;
    }

    /// <summary>
    /// オブジェクトのアクティブをセット
    /// </summary>
    protected void SelectChangeToSetActiveObject()
    {
        if (m_isSelectChangeFlg == false) return;
        SetActiveObject();
    }

    protected void SetActiveObject()
    {
        // 選択可能時
        if (IsDecision())
        {
            if (m_possibleObject != null)
            {
                m_possibleObject.SetActive(true);
            }
            if (m_impossibleObject != null)
            {
                m_impossibleObject.SetActive(false);
            }
        }
        // 選択不可能時
        else
        {
            if (m_possibleObject != null)
            {
                m_possibleObject.SetActive(false);
            }
            if (m_impossibleObject != null)
            {
                m_impossibleObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 値を選択する
    /// </summary>
    protected void Select()
    {
        // スライダー
        Slider();

        // 足す
        Increment();

        // 減らす
        Decrement();
    }


    /// <summary>
    /// 値を加算
    /// </summary>
    protected void Increment()
    {
        #region nullチェック
        if (m_incrementInputActionButton == null)
        {
            Debug.LogError("IncrementInputActionButtonがシリアライズされていません");
            return;
        }
        #endregion

        if (m_incrementInputActionButton.IsInputActionTrriger())
        {
            if (IsIncrement())
            {
                m_currentValue++;
                m_isSelectChangeFlg = true;
                SetSliderValue();
            }
        }
    }


    /// <summary>
    /// 値を減算
    /// </summary>
    protected void Decrement()
    {
        #region nullチェック
        if (m_decrementInputActionButton == null)
        {
            Debug.LogError("DecrementInputActionButtonがシリアライズされていません");
            return;
        }
        #endregion

        if (m_decrementInputActionButton.IsInputActionTrriger())
        {
            if (IsDecrement())
            {
                m_currentValue--;
                m_isSelectChangeFlg = true;
                SetSliderValue();
            }
        }
    }


    /// <summary>
    /// スライダーを更新
    /// </summary>
    virtual protected void Slider()
    {
        // スライダーがある場合のみ
        if (m_valueSlider == null) return;

        if (m_currentValue != (int)m_valueSlider.value)
        {
            m_currentValue = (int)m_valueSlider.value;
            m_isSelectChangeFlg = true;
        }
    }



    /// <summary>
    /// 派生クラスでmaxValueなどを設定する
    /// </summary>
    abstract protected void SetData();


    /// <summary>
    /// 現在の値で決定することができるか
    /// </summary>
    public abstract bool IsDecision();


    /// <summary>
    /// スライダーの値を更新
    /// </summary>
    virtual protected void SetSliderValue()
    {
        if (m_valueSlider == null) return;

        m_valueSlider.minValue = m_minValue;
        m_valueSlider.maxValue = m_maxValue;
        m_valueSlider.value = m_currentValue;

        if (m_text != null)
        {
            m_text.text = m_currentValue.ToString();
        }
    }


    /// <summary>
    /// 足せるか
    /// </summary>
    public bool IsIncrement()
    {
        if (m_currentValue < m_maxValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 減らせるか
    /// </summary>
    public bool IsDecrement()
    {
        if (m_minValue < m_currentValue)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

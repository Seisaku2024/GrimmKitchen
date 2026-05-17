using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 制作者　吉田
/// 
/// SerializeField で設定したTMPの値と　 所持金を比べる
/// 
/// Updateでは
/// 所持金が足りない場合に自身のgameobjectを表示にする
/// 
/// </summary>
public class AttentionPrice : WindowUpdateBase
{
    [Header("比較するTMP nullの場合は自分のオブジェクトから探す")]
    [SerializeField]
    private TextMeshProUGUI m_comparePrice = null;

    [Header("TMPが0の時 比較結果を強制この値にする")]
    [SerializeField]
    private bool m_compareInZero = true;

    // Start is called before the first frame update
    public override void OnInitialize()
    {
        if (m_comparePrice == null)
        {
            if (!TryGetComponent(out m_comparePrice))
            {
                Debug.LogError("TMPがシリアライズされていません. オブジェクト名：" + gameObject.name +
                    "　スクリプト名：" + name);
                return;
            }
        }

        if (ComparePrice())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        if (m_comparePrice == null) return;

        if (ComparePrice())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 引数 <= 所持金 -> true
    /// 所持金が0の場合はtrue
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool ComparePrice(uint _price)
    {
        int total = ManagementDataManager.instance.TotalEarnedMoney;

        if (total <= 0)
        {
            return true;
        }

        if (_price <= total)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// シリアライズしたTMPの値 ＜= 所持金 -> true
    /// 上手く行かなかった場合はログを出力して,trueを返す
    /// </summary>
    /// <returns></returns>
    public bool ComparePrice()
    {
        if (m_comparePrice == null)
        {
            if (!TryGetComponent(out m_comparePrice))
            {
                Debug.Log("TMPがシリアライズされていません. オブジェクト名：" + gameObject.name +
                    "　スクリプト名：" + name);
                return true;
            }
        }

        uint price = uint.Parse(m_comparePrice.text);
        // 0の時はm_compareInZeroで返す
        if (price == 0)
        {
            return m_compareInZero;
        }

        return ComparePrice(price);
    }
}

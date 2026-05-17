using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using NaughtyAttributes;
using ManagementGameInfo;
using Cysharp.Threading.Tasks;

public class ChangeResultDescription : MonoBehaviour
{
    // 制作者 田内
    // 経営リザルトの説明文

    [Header("総提供数")]
    [SerializeField]
    private TextMeshProUGUI m_soldNumText = null;

    [Header("稼いだ金額")]
    [SerializeField]
    private TextMeshProUGUI m_earnedMoneyText = null;

    [Header("満足値")]
    [SerializeField]
    private TextMeshProUGUI m_satisfactionValueText = null;

    [Header("総額")]
    [SerializeField]
    private TextMeshProUGUI m_totalEarnedMoneyText = null;

    [Header("提供料理スロット作成")]
    [SerializeField]
    private CreateManagementProvideFoodDataSlotList m_createProvideFoodDataSlotList = null;

    [Header("使用食材スロット作成")]
    [SerializeField]
    private CreateProvideFoodUseIngredientSlotList m_createProvideFoodUseIngredientSlotList = null;

    //==============================================================

    [BoxGroup("来客数")]
    [Header("来客数")]
    [SerializeField]
    private TextMeshProUGUI m_cameCustomerNumText = null;

    [BoxGroup("来客数")]
    [Header("通常来客数")]
    [SerializeField]
    private TextMeshProUGUI m_cameNormalCustomerNumText = null;

    [BoxGroup("来客数")]
    [Header("怒った来客数")]
    [SerializeField]
    private TextMeshProUGUI m_cameAngryCustomerNumText = null;


    [BoxGroup("イベント数")]
    [Header("イベント解決数")]
    [SerializeField]
    private TextMeshProUGUI m_eventSolutionNumText = null;

    [BoxGroup("イベント数")]
    [Header("イベント未解決数")]
    [SerializeField]
    private TextMeshProUGUI m_eventUnSolutionNumText = null;


    //==============================================================
    //                      実行処理
    //==============================================================


    virtual public async UniTask OnInitialize()
    {
        var cancelToken = this.destroyCancellationToken;

        try
        {
            await SetDescription();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }



    protected async UniTask SetDescription()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await CreateProvideFoodSlot();
            cancelToken.ThrowIfCancellationRequested();

            await CreateProvideFoodUseIngredientSlot();
            cancelToken.ThrowIfCancellationRequested();

            SetEarnedMoneyText();

            SetSoldNumText();

            SetSatisfactionValueText();

            SetCameCustomerNumText();

            SetCameNormalCustomerNumText();

            SetCameAngryCustomerNumText();

            SetTotalEarnedMoneyText();

            SetEventSolutionNumText();

            SetEventUnSolutionNumText();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // 総提供数をセット
    private void SetSoldNumText()
    {
        if (m_soldNumText == null)
        {
            // SoldNumTextがシリアライズされていません
            return;
        }
        m_soldNumText.gameObject.SetActive(false);

        uint soldNum = 0;

        foreach (var data in ManagementGameDataManager.instance.ProvideFoodDataRPRC)
        {
            if (data == null || data.Value == null) continue;
            soldNum += data.Value.SoldNum;
        }

        m_soldNumText.text = soldNum.ToString("0");
        m_soldNumText.gameObject.SetActive(true);
    }


    // 提供料理スロットを作成
    private async UniTask CreateProvideFoodSlot()
    {
        if (m_createProvideFoodDataSlotList == null) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await m_createProvideFoodDataSlotList.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // 使用食材スロットを作成
    private async UniTask CreateProvideFoodUseIngredientSlot()
    {
        if (m_createProvideFoodUseIngredientSlotList == null) return;

        var cancelToken = this.GetCancellationTokenOnDestroy();
        try
        {
            await m_createProvideFoodUseIngredientSlotList.CreateSlot();
            cancelToken.ThrowIfCancellationRequested();
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    // 稼いだ金額
    private void SetEarnedMoneyText()
    {
        if (m_earnedMoneyText == null) return;


        m_earnedMoneyText.gameObject.SetActive(false);

        m_earnedMoneyText.text = ManagementGameDataManager.instance.EarnedMoney.ToString();
        m_earnedMoneyText.gameObject.SetActive(true);
    }

    private void SetSatisfactionValueText()
    {
        if (m_satisfactionValueText == null) return;

        m_satisfactionValueText.gameObject.SetActive(false);

        m_satisfactionValueText.text = ManagementGameDataManager.instance.CurrentSatisfactionValue.ToString();
        m_satisfactionValueText.gameObject.SetActive(true);
    }

    // 総額
    private void SetTotalEarnedMoneyText()
    {
        if (m_totalEarnedMoneyText == null) return;


        m_totalEarnedMoneyText.gameObject.SetActive(false);

        m_totalEarnedMoneyText.text = ManagementDataManager.instance.TotalEarnedMoney.ToString();
        m_totalEarnedMoneyText.gameObject.SetActive(true);

    }

    // 来客数テキストをセット
    private void SetCameCustomerNumText()
    {
        if (m_cameCustomerNumText == null) return;


        m_cameCustomerNumText.gameObject.SetActive(false);

        uint cameCustomerNum = 0;

        foreach (var num in ManagementGameDataManager.instance.CameCustomerNumDictionary)
        {
            cameCustomerNum += num.Value;
        }

        m_cameCustomerNumText.text = cameCustomerNum.ToString();
        m_cameCustomerNumText.gameObject.SetActive(true);

    }


    // 通常の来客数テキストをセット
    private void SetCameNormalCustomerNumText()
    {
        if (m_cameNormalCustomerNumText == null) return;


        m_cameNormalCustomerNumText.gameObject.SetActive(false);

        ManagementGameDataManager.instance.CameCustomerNumDictionary.TryGetValue(CameCustomerType.Normal, out var num);

        m_cameNormalCustomerNumText.text = num.ToString();
        m_cameNormalCustomerNumText.gameObject.SetActive(true);

    }

    // 怒って帰った来客数テキストをセット
    private void SetCameAngryCustomerNumText()
    {
        if (m_cameAngryCustomerNumText == null) return;


        m_cameNormalCustomerNumText.gameObject.SetActive(false);

        ManagementGameDataManager.instance.CameCustomerNumDictionary.TryGetValue(CameCustomerType.Angry, out var num);

        m_cameAngryCustomerNumText.text = num.ToString();
        m_cameNormalCustomerNumText.gameObject.SetActive(true);
    }

    // イベント解決数テキスト
    private void SetEventSolutionNumText()
    {
        if (m_eventSolutionNumText == null) return;


        m_eventSolutionNumText.gameObject.SetActive(false);

        ManagementGameDataManager.instance.EventSolutionNumDictionary.TryGetValue(EventSolutionType.Solution, out var num);

        m_eventSolutionNumText.text = num.ToString();
        m_eventSolutionNumText.gameObject.SetActive(true);
    }

    // イベント未解決数テキスト
    private void SetEventUnSolutionNumText()
    {
        if (m_eventUnSolutionNumText == null) return;

        m_eventUnSolutionNumText.gameObject.SetActive(false);

        ManagementGameDataManager.instance.EventSolutionNumDictionary.TryGetValue(EventSolutionType.UnSolution, out var num);

        m_eventUnSolutionNumText.text = num.ToString();
        m_eventUnSolutionNumText.gameObject.SetActive(true);
    }

}

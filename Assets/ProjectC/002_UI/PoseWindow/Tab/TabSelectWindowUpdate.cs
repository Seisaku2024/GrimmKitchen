using Cysharp.Threading.Tasks;
using SaintsField;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSelectWindowUpdate : WindowUpdateBase
{
    [Header("Tabコントローラー")]
    [SerializeField]
    [RichLabel("Tab SelectUIController")]
    private SelectUIController m_selectTab = null;

    [SerializeField]
    SerializableDictionary<ButtonInfo.ButtonID, SelectUIController> m_selectUIControllerDic = new();

    private ButtonInfo.ButtonID m_currentButtonID = ButtonInfo.ButtonID.None;

    public override void OnInitialize()
    {
        if (m_selectTab == null)
        {
            Debug.LogError("selectUIControllerがシリアライズされていません");
            return;
        }

        m_currentButtonID = ButtonInfo.ButtonID.OptionSound;

        // アクティブ状態じゃないとStartが呼ばれないので手動で呼ぶ
        foreach (var selectUIController in m_selectUIControllerDic)
        {
            selectUIController.Value.Start();
        }
    }

    public override void OnUpdate()
    {

    }

    public override async UniTask OnUpdateTask()
    {
        if (m_selectTab == null) return;
        if (m_selectTab.CurrentSelectUIData == null) return;
        if (m_selectTab.CurrentSelectUIData.ButtonData == null) return;
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // タブの切り替え
            if (m_currentButtonID != m_selectTab.CurrentSelectUIData.ButtonData.ButtonID)
            {
                ChangeTab(m_selectTab.CurrentSelectUIData.ButtonData.ButtonID);
            }

            // Update処理
            if (!m_selectUIControllerDic.ContainsKey(m_currentButtonID)) return;
            await m_selectUIControllerDic[m_currentButtonID].OnUpdate();
            cancelToken.ThrowIfCancellationRequested();

            // スライダーの更新
            m_selectUIControllerDic[m_currentButtonID].UpdateSlider();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    public override void OnLateUpdate()
    {
        // LateUpdate処理
        foreach (var selectController in m_selectUIControllerDic)
        {
            selectController.Value.OnLateUpdate();
        }
    }

    // タブの切り替え
    private void ChangeTab(ButtonInfo.ButtonID _next)
    {
        // 前のタブの選択解除
        if (m_selectUIControllerDic.ContainsKey(m_currentButtonID))
        {
            m_selectUIControllerDic[m_currentButtonID].ButtonDataUnselectAllUI();
        }

        // 次のタブの選択を0番目に
        m_currentButtonID = _next;
        if (!m_selectUIControllerDic.ContainsKey(_next)) return;
        m_selectUIControllerDic[_next].ResetCurrent();
    }

}

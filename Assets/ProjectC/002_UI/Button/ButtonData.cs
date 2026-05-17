using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ButtonInfo;
using Cysharp.Threading.Tasks;
using System;

namespace ButtonInfo
{
    public enum ButtonID
    {
        None = -1,
        Start = 0,
        Cancel = 1,



        // タイトル
        TitleNewGame = 100,
        TitleContinue = 101,
        TitleOption = 102,
        TitleExit = 103,
        TitleCredit=104,

        SelectProvideFood = 1000,
        SelectStaff = 1001,


        RandomStaff = 1100,
        StaffStorage = 1101,
        Challenge = 1102,

        Storage = 1200,
        ManagementStorage = 1201,

        MoveItem = 1300,

        ManagementStart = 2001,

        StageEasy = 4000,
        StageNormal = 4001,
        StageHard = 4002,

        // オプション(試遊会用)
        OptionBackTitleToInitializeTrialSession = 3001,

        OptionSound = 3101,
        OptionGame = 3102,
        OptionKeyConfig = 3103,
        OptionPose = 3104,

        // 童話スキル切り替え用（山本）
        ChangeStorySkill1 = 5000,
        ChangeStorySkill2 = 5001,

    }


    // 使用可能かどうか
    public enum IsUseButtonID
    {
        Disable,
        Enable,
    }
}



public class ButtonData : MonoBehaviour
{
    // ボタンデータをセットする
    // 制作者　田内

    [Header("ボタンID")]
    [SerializeField]
    protected ButtonID m_buttonID = ButtonID.None;

    public ButtonID ButtonID
    {
        get
        {
            return m_buttonID;
        }
    }


    //===========================================

    [Header("使用可能")]
    [SerializeField]
    protected bool m_isUse = true;

    public bool IsUse
    {
        get { return m_isUse; }
        set { m_isUse = value; }
    }

    //===========================================

    private bool m_isSelect = false;
    public bool IsSelect
    {
        get { return m_isSelect; }
    }

    [Header("使用不可時に見えるオブジェクト")]
    [SerializeField]
    private GameObject m_notUseGameObject = null;

    [Header("選択時に見えるオブジェクト")]
    [SerializeField]
    private GameObject m_selectGameObject = null;

    private List<Action> m_onPressEventList = new();
    public void AddOnPressEvent(Action _event)
    {
        m_onPressEventList.Add(_event);
    }


    //=================================================
    //                  実行処理
    //=================================================

    virtual protected void Start()
    {
        if (m_isUse == true)
        {
            SetAble();
        }
        else
        {
            SetDisable();
        }
    }

    //　使用不可時の処理
    public void SetDisable()
    {
        if (m_notUseGameObject == null) return;

        m_isUse = false;
        m_notUseGameObject.SetActive(true);
    }

    //　使用可能時の処理
    public void SetAble()
    {
        if (m_notUseGameObject == null) return;

        m_isUse = true;
        m_notUseGameObject.SetActive(false);
    }

    // 押されたときに行う処理
    virtual public async UniTask OnPressUpdate()
    {
        foreach (var item in m_onPressEventList)
        {
            item.Invoke();
        }
        await UniTask.CompletedTask;
    }


    // 選択されたときに行う処理(追加：吉田)
    virtual public void OnSelectUpdate()
    {
        m_isSelect = true;

        if (m_selectGameObject == null) return;
        m_selectGameObject.SetActive(true);
    }

    // 選択はずれたときに行う処理(追加：吉田)
    virtual public void OnUnselectUpdate()
    {
        m_isSelect = false;

        if (m_selectGameObject == null) return;
        m_selectGameObject.SetActive(false);
    }


}

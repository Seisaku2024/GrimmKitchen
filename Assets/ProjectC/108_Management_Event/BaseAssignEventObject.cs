/*
 * @file BaseAssignEventObject
 * @brief ImetaAIを用いてイベントオブジェクト群として管理するための基底クラス
 * Colliderに接触、接触終了した際のイベントと外部アクセス時のイベントを定義
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// @brief ImetaAIを用いてイベントオブジェクト群として管理するための外部アクセス前提イベントオブジェクトの基底クラス
/// 接触時イベント(自動),接触終了時イベント(自動)、アクセス時イベント(外部呼び出し)
/// </summary>
public class BaseAssignEventObject : MonoBehaviour
{
    //! @brief アクセス時に実行させたいアニメーショントリガー名
    [Header("アニメーショントリガー名")]
    [SerializeField] string m_animatorTriggerName;

    //! @brief 接触イベントを発生させるためのタグリスト
    [Header("接触イベントを発生させるためのタグリスト")]
    [Tag]
    [SerializeField] List<string> m_tagList = new();

    //! @brief 接触中かを管理するプロパティ
    private bool m_isCollided = false;

    //! @brief 接触中かを返すプロパティ
    public bool Collided { get { return m_isCollided; } }

    //! @brief イベント当たり判定が有効かを管理するプロパティ
    public bool isCollisionEnable = true;

    //! @brief UI表示用の参照
    [HideInInspector]
    public CharacterCore m_cCore;

    [SerializeField, Header("アクセス時のUI表示")]
    public ActionUIController.ActionUIState m_actionUIState;



    //====================================================================================================
    // virtual関数 要定義
    //====================================================================================================

    /// <summary>
    /// @brief 接触時のイベント
    /// m_tagListに登録されたタグオブジェクトとの接触時に自動で呼ばれる
    /// </summary>
    protected virtual void OnCollisionTriggerEvent()
    {
        Debug.LogError("BaseAssignEventObjectのOnCollisionTriggerEventが呼ばれました");
    }

    /// <summary>
    /// @brief 接触終了時のイベント
    /// </summary>
    protected virtual void OnCollisionTriggerExitEvent()
    {
        Debug.LogError("BaseAssignEventObjectのOnCollisionExitEventが呼ばれました");
    }

    /// <summary>
    /// @brief 何らかのアクセスがあった際のイベント
    /// アクセスの定義は継承先で行う
    /// </summary>
    public virtual void OnCollisionAccessEvent()
    {
        Debug.LogError("BaseAssignEventObjectのOnCollisionAccessEventが呼ばれました");
    }

    /// <summary>
    /// @brief アクセスされたかどうかを返す
    /// 主にキーアクセスを想定
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual bool IsAccessed(ref IInputProvider input, GameObject player)
    {
        Debug.LogError("BaseAssignEventObjectのIsAccessedが呼ばれました");
        return false;
    }

    protected virtual bool IsShowAbleActionUI()
    {
        return isCollisionEnable;
    }


    public virtual bool SetAnimationTrigger()
    {
        if (m_cCore == null || m_cCore.m_animator == null) return false;
        // 1フレームでリセットされるような拡張関数を使用
        m_cCore.m_animator.SetTriggerOneShot(m_animatorTriggerName);
        return true;
    }

    //====================================================================================================

    private void OnTriggerEnter(Collider other)
    {
        bool tagCheck = false;
        foreach (var tag in m_tagList)
        {
            if (other.CompareTag(tag))
            {
                tagCheck = true;
            }
        }

        if (tagCheck)
        {
            m_isCollided = true;
            if (!isCollisionEnable) return;
            ShowActionUI();
            OnCollisionTriggerEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool tagCheck = false;
        foreach (var tag in m_tagList)
        {
            if (other.CompareTag(tag))
            {
                tagCheck = true;
            }
        }

        if (tagCheck)
        {
            HideActionUI();
            m_isCollided = false;
            if (!isCollisionEnable) return;
            OnCollisionTriggerExitEvent();
        }
    }

    virtual protected void Start()
    {
        if (IMetaAI<BaseAssignEventObject>.Instance == null)
        {
            Debug.LogError("BaseAssignEventObjectのMetaAIが存在しません");
            return;
        }


        // MetaAIに登録
        IMetaAI<BaseAssignEventObject>.Instance.RegisterObject(this);
    }


    protected void ShowActionUI()
    {

        if (m_cCore == null || m_cCore.PlayerParameters == null) return;
        if (!IsShowAbleActionUI()) return;

        m_cCore.PlayerParameters.AddAnyActionUIState(m_actionUIState, 1);
        m_cCore.PlayerParameters.AddActionUIState(m_actionUIState);
    }

    public void HideActionUI()
    {
        if (m_cCore == null || m_cCore.PlayerParameters == null) return;

        m_cCore.PlayerParameters.RemoveAnyActionUIState(m_actionUIState);
        m_cCore.PlayerParameters.RemoveActionUIState(m_actionUIState);
    }

}

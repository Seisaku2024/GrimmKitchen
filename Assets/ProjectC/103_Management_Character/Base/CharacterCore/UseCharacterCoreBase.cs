using Arbor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// CharacterCoreを使うスクリプトのベースクラス
/// </summary>
public class UseCharacterCoreBase : StateBehaviour
{
    private CharacterCore m_CharacterCore;

    protected CharacterCore CharacterCore
    {
        get
        {
            if (m_CharacterCore == null)
            {
                SetOrAutoSetCCore();
            }
            return m_CharacterCore;
        }
    }

    private void Start()
    {
        if(m_CharacterCore == null)
        {
            SetOrAutoSetCCore();
        }
    }

    public void SetOrAutoSetCCore(CharacterCore core = null)
    {
        if (core != null)
        {
            m_CharacterCore = core;
        }
        else
        {
            m_CharacterCore = GetComponentInParent<CharacterCore>();
        }
    }
}

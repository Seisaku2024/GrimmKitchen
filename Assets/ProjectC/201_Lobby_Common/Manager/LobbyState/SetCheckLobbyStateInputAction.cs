using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class SetCheckLobbyStateInputAction : MonoBehaviour
{

    private CharacterCore m_playerCore = null;

    private bool m_bSearchObjFlg = false;
    public bool bSearchObjFlg => m_bSearchObjFlg;

    bool m_isNextChange = false;

    void Start()
    {

        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (core.GroupNo == CharacterGroupNumber.player)
            {
                m_playerCore = core;
            }
        }
    }

    private void Update()
    {
        if(m_isNextChange)
        {
            Click();
        }
    }

    // 当たり判定内に入った瞬間でも判定
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }
        m_isNextChange = true;
       // Click(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        m_isNextChange = false;
    }


    // 当たり判定内に入り、選択ボタンを押したら
    private void OnTriggerStay(Collider other)
    {
        //Click(other);
    }

    public void Click()
    {
        if (m_playerCore == null) return;

        PlayerInputManager.instance.GetInputAction(InputActionMapTypes.Player, "Attack").Disable();

        // 攻撃（選択）が入力されたら
        if (m_playerCore.InputProvider.Search
            && (m_bSearchObjFlg == false))
        {
           // m_playerCore.m_animator.SetTrigger("ShowCutScene");
            m_bSearchObjFlg = true;

            PlayerInputManager.instance.GetInputAction(InputActionMapTypes.Player, "Attack").Enable();
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class PlayerStatusPresenter : MonoBehaviour
{
    // プレイヤーのステータスに関するプレゼンタークラス(山本)
    private enum PlayerStatus
    {
        None,
        HP,
        Attack,
        Diffence,
        Stamina
    }

    [SerializeField] private PlayerStatus m_status = PlayerStatus.None;
    [SerializeField] private ParameterBarController m_controller;
    

    [Header("仮の最大値")]
    [SerializeField] private float m_maxHP = 1000;
    [SerializeField] private float m_maxAttack = 99;
    [SerializeField] private float m_maxDif = 99;
    [SerializeField] private float m_maxStamina = 100;



    private CharacterCore m_playerCore = null;
    private PlayerParameters m_playerParameters = null;
    private StrengtheningUpperLimit m_upperLimit = null;

    void Start()
    {
        if (IMetaAI<CharacterCore>.Instance == null)
        {
            return;
        }

        // プレイヤーコアセット
        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara.GroupNo == CharacterGroupNumber.player)
            {
                m_playerCore = chara;
                break;
            }
        }

        if (m_playerCore == null) { return; }


        m_playerParameters = m_playerCore.PlayerParameters;


        if (m_playerParameters == null)
        {
            return;
        }


        m_upperLimit = PlayerDataBaseManager.instance.DataBase.GetParameterLimit(PlayerStatusManager.instance.StrengtheningCap);

        if (m_upperLimit == null)
        {
            return; 
        }


        switch (m_status)
        {

            case PlayerStatus.HP:

                m_playerCore.Status.MaxHP.Subscribe(x => m_controller.SetParameter(x, m_upperLimit.HpLimit, m_maxHP));

                break;
            case PlayerStatus.Attack:


                m_playerCore.Status.m_attack.Subscribe(x => m_controller.SetParameter(x, m_upperLimit.AttackLimit, m_maxAttack));

                break;
            case PlayerStatus.Diffence:

                m_playerCore.Status.m_defense.Subscribe(x => m_controller.SetParameter(x, m_upperLimit.DefenceLimit, m_maxDif));

                break;
            case PlayerStatus.Stamina:

                m_playerCore.PlayerParameters.PlayerStatus.StaminaData.MaxStamina.Subscribe(x => m_controller.SetParameter(x, m_upperLimit.StaminaLimit, m_maxStamina));

                break;
            default:
                break;
        }

    }

   
}

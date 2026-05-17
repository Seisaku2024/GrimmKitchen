using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class PlayerDataBase : ScriptableObject
{
    [Header("基礎ステータス")]
    [SerializeField]
    private CharacterStatus m_characterStatus;
    public CharacterStatus CharacterStatus => m_characterStatus;

    [SerializeField]
    private StaminaDatabase m_staminaData;
    public StaminaDatabase Stamina { get { return m_staminaData; } }


    [SerializeField, Header("プレイヤーの各攻撃情報")]
    private List<AttackDamageData> m_attackData = new();

    public List<AttackDamageData> AttackData { get { return m_attackData; } }


    [SerializeField] private List<StrengtheningUpperLimit> m_parameterLimit = new();
    public StrengtheningUpperLimit GetParameterLimit(int limitStage)
    {
        if (m_parameterLimit.Count < limitStage)
        {
            Debug.LogError("プレイヤーの" + limitStage + "段階の強化パラメータは登録されてません");
            return null;
        }
        return m_parameterLimit[limitStage - 1];
    }

}


[Serializable]
public class StrengtheningUpperLimit
{
    [SerializeField] private float m_attackLimit;
    public float AttackLimit => m_attackLimit;

    [SerializeField] private float m_hpLimit;
    public float HpLimit => m_hpLimit;

    [SerializeField] private float m_defenceLimit;
    public float DefenceLimit => m_defenceLimit;

    [SerializeField] private float m_staminaLimit;
    public float StaminaLimit => m_staminaLimit;
}

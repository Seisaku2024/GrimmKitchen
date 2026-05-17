using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanselGretelAddTargetList : MonoBehaviour
{
    private List<CharacterCore> m_targetAddEnemyList = new List<CharacterCore>();
    public List<CharacterCore> TargetAddEnemyList { get { return m_targetAddEnemyList; } }
}

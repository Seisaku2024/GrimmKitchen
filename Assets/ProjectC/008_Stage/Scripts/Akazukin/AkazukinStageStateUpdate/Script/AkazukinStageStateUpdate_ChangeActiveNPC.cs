using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkazukinStageStateUpdate_ChangeActiveNPC : BaseAkazukinStageStateUpdate
{
    [Header("アクティブを操作するNPCリスト（Key:SpeakType,Value:アクティブにするかどうか）")]
    [SerializeField]
    private SerializableDictionary<SpeakerType, bool> m_changeNpcActiveList;

    [Header("アクティブを操作する敵のリスト（Key:GropeNo,Value:アクティブにするかどうか）")]
    [SerializeField]
    private SerializableDictionary<EnemyID, bool> m_changeNonNpcActiveList;

    public override UniTask OnInitialize()
    {
        if (m_changeNpcActiveList.Count != 0)
        {
            foreach (var character in m_changeNpcActiveList)
            {
                foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
                {
                    if (core.GroupNo == CharacterGroupNumber.NPC
                        && core.NPCParameters.SpeakerType == character.Key)
                    {
                        core.gameObject.SetActive(character.Value);
                        break;
                    }
                }
            }
        }



        if (m_changeNonNpcActiveList.Count != 0)
        {
            foreach (var character in m_changeNonNpcActiveList)
            {
                foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
                {
                    if (core.GroupNo == CharacterGroupNumber.enemy
                        && core.EnemyParameters.GetEnemyData().EnemyID == character.Key)
                    {
                        core.gameObject.SetActive(character.Value);
                        break;
                    }
                }
            }
        }


        SetEnd(m_nextAkazukinStageState);

        return base.OnInitialize();
    }
}

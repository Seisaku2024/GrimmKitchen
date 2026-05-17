using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetKinematicMotorCharacter : MonoBehaviour
{
    // 童話スキル召喚時にKinematicCharacterMotorコンポーネントを持つ
    //キャラクターを指定する場所へとセットする(山本)

    [Header("キャラクターコアとセットするTransform")]
    [SerializeField] SerializableDictionary<CharacterCore, Transform> m_nodesList;


    private void Start()
    {
        //CharacterSetPosition();
    }


    public void CharacterSetPosition()
    {
        // リストの登録したキャラクターを指定の場所にセット
        foreach (var node in m_nodesList)
        {
            node.Key.CharaCtrl.SetPositionMotor(node.Value.position, true);
        }
    }

}

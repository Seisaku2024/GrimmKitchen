using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStorySkillSpetialPosition : MonoBehaviour
{
    // 童話スキルごとに特殊な位置情報を登録し、他クラスからアクセスしやすくするための
    //コンポーネント（山本）

    [SerializeField]
    private SerializableDictionary<string,Transform>m_storySkillspecialTransformList = new SerializableDictionary<string,Transform>();
    public SerializableDictionary<string, Transform> StorySkillSpecialTransformList { get { return m_storySkillspecialTransformList; } }


}

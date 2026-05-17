using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StoryProgressDataBase : ScriptableObject
{
    [Header("ストーリー進捗度データのリスト")]
    [SerializeField]
    private List<StoryProgressData> m_storyDataBaseProgressList = new();

    public List<StoryProgressData> StoryDataBaseProgressList => m_storyDataBaseProgressList;
}

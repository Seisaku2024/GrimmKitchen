using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectInState : MonoBehaviour
{
    [Header("指定の進捗度達成してたらDeleteする")]
    [SerializeField]
    private StoryProgressType m_progressType = StoryProgressType.None;

    void Start()
    {
        if (StoryProgressManager.instance == null)
        {
            return;
        }

        if (StoryProgressManager.instance.GetStoryProgressData(m_progressType).AchieveFlg )
        {
            Destroy(gameObject);
        }
    }


}

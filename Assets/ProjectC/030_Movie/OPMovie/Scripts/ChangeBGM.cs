using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    public void PlayBGM(string _string)
    {
        SoundManager.Instance.StartBGM(_string);
    }
}

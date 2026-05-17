using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBGM : MonoBehaviour
{
    [Header("音量ロード")]
    [SerializeField]
    private VolumeSaveLoader m_volumeSaveLoader = null;
    


    //音再生（山本）
    [SerializeField] private string m_queName = null;
    // Start is called before the first frame update
    void Start()
    {
        if (m_volumeSaveLoader != null)
        {
            m_volumeSaveLoader.LoadVolume();
        }

        SoundManager.Instance.StopBGM();

        if (m_queName.Length != 0.0f)
            SoundManager.Instance.StartBGM(m_queName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : BaseManager<PlayerStatusManager>
{
    private CharacterStatus m_characterStatus;
    public CharacterStatus CharacterStatus()
    {
        if (m_characterStatus == null)
        {
            DataLoad();
        }
        return m_characterStatus;
    }

    private StrengtheningStatus m_nowStatus;
    public StrengtheningStatus NowStatus()
    {
        if (m_nowStatus == null)
        {
            DataLoad();
        }
        return m_nowStatus;
    }
    // 強化ステータスのキャップ
    // TODO:キャップの解放条件に合わせて保管場所を帰る
    private int m_strengtheningCap = 1;
    public int StrengtheningCap { get { return m_strengtheningCap; } }

    protected override void Awake()
    {
        base.Awake();

        DataLoad();
    }

    public void DataLoad()
    {
        if (m_nowStatus == null)
        {
            m_nowStatus = new();
        }
        PlayerStatusSaveLoader.Load(ref m_characterStatus, ref m_nowStatus, ref m_strengtheningCap);
    }

    //void Update()
    //{

    //}
}

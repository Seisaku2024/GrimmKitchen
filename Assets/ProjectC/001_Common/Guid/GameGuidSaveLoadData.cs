using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameGuidSaveLoadData
{
    // 制作者 田内
    // Guidセーブ用

    public GameGuidSaveLoadData()
    {
        m_gameGuid = GameGuid.NewGuid();
        m_guidString = m_gameGuid.ToString();
    }

    public GameGuidSaveLoadData(GameGuid _guid)
    {
        m_gameGuid = _guid;
        m_guidString = _guid.ToString();
    }

    // Guid
    private GameGuid m_gameGuid;
    public GameGuid GameGuid
    {
        get { return m_gameGuid; }
        set { m_gameGuid = value; }
    }

    // GuidString
    private string m_guidString = "";
    public string GuidString
    {
        get { return m_guidString; }
        set { m_guidString = value; }
    }
}

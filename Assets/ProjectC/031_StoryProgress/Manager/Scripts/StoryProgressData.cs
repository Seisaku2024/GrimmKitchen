using Arbor.StateMachine.StateBehaviours;
using JetBrains.Annotations;
using StageInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class StoryProgressData : ScriptableObject
{
    //ストーリー進捗度のデータ（山本）
    [Header("ストーリータイプ")]
    [SerializeField]
    private StoryProgressType m_progressType = StoryProgressType.None;

    public StoryProgressType StoryProgressType => m_progressType;

    [Header("達成フラグ")]
    [SerializeField]
    private bool m_achieveFlg = false;
    public bool AchieveFlg { get { return m_achieveFlg; }}


    //================================================================================
    //データ

    private AchieveStoryProgressData m_achieveStoryProgressData = null;

    public AchieveStoryProgressData AchieveStoryProgressData
    {
        get
        {
            if (m_achieveStoryProgressData == null) m_achieveStoryProgressData = new(m_progressType);
            return m_achieveStoryProgressData;
        }
    }
    //===============================================================================


    //========================================
    //              実行処理
    //========================================

    public void Load(AchieveStoryProgressDataSaveLoad _data)
    {
        if (_data == null) return;
        m_achieveStoryProgressData = new(m_progressType);
        m_achieveStoryProgressData.IsAchieve = _data.IsAchieve;
        m_achieveFlg = m_achieveStoryProgressData.IsAchieve;
    }

    public void SetFinish()
    {
        if (m_achieveStoryProgressData == null) m_achieveStoryProgressData = new(m_progressType);

        m_achieveStoryProgressData.IsAchieve = true;
        m_achieveFlg = true;
    }


   

}

[System.Serializable]
public class AchieveStoryProgressDataSaveLoad
{
    public AchieveStoryProgressDataSaveLoad(StoryProgressType _type)
    {
        var data = StoryProgressManager.instance.GetStoryProgressData(_type);
        if (data == null) return;
        IsAchieve = false;
    }


    public AchieveStoryProgressDataSaveLoad(StoryProgressType _type, AchieveStoryProgressData _data)
    {
        if (_data == null) return;
        StoryProgressType = _type;
        IsAchieve = _data.IsAchieve;
    }

    public StoryProgressType StoryProgressType = StoryProgressType.None;
    public bool IsAchieve = false;

}


[System.Serializable]
public class AchieveStoryProgressData
{
    public AchieveStoryProgressData(StoryProgressType _type)
    {
        var data = StoryProgressManager.instance.GetStoryProgressData(_type);
        if (data == null) return;

        IsAchieve = false;
    }

    public bool IsAchieve = false;
}

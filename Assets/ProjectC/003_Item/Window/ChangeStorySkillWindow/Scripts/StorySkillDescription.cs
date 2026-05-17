using ItemInfo;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Video;
using Cysharp.Threading.Tasks;

public class StorySkillDescription : MonoBehaviour
{
    //==============================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("スキル名Text")]
    [SerializeField]
    protected TextMeshProUGUI m_nameTextMeshPro = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_nameList = new();

    //==========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("説明文Text")]
    [SerializeField]
    protected TextMeshProUGUI m_descriptionTextMeshPro = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_descriptionList = new();

    //===========================================================

    [Foldout("共通")]
    [Header("-------------------------------------------------------")]
    [Header("スキル動画")]
    [SerializeField]
    protected VideoPlayer m_storySkillVideo = null;


    [Foldout("共通")]
    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_storySkillVideoList = new();

    //===========================================================


    private StorySkillData m_nowStorySkill = null;

    private void SetStorySkillActiveList()
    {
        // 名前
        UIExtensions.CheckToSetActiveGameObjectList(m_nameTextMeshPro, m_nameList);
        // 説明
        UIExtensions.CheckToSetActiveGameObjectList(m_descriptionTextMeshPro, m_descriptionList);
        // 動画
        UIExtensions.CheckToSetActiveGameObjectList(m_storySkillVideo, m_storySkillVideoList);
        
    }

    public async UniTask UpdateStorySkillDescription(StorySkillData _storySkillData)
    {
        m_nowStorySkill = _storySkillData;

        if (m_nowStorySkill == null) return;

        if (m_nameTextMeshPro)
        {
            var NameText = await m_nowStorySkill.StorySkillName.GetLocalizedStringAsync();
            m_nameTextMeshPro.text = NameText;
        }

        if (m_descriptionTextMeshPro)
        {
            var DescriptionText = await m_nowStorySkill.StorySkillDescriptionText.GetLocalizedStringAsync();
            m_descriptionTextMeshPro.text = DescriptionText;
        }

        if (m_storySkillVideo)
            m_storySkillVideo.clip = m_nowStorySkill.StorySkillClip;

        SetStorySkillActiveList();

    }


    /// <summary>
    /// 実行処理
    /// </summary>

    private void Start()
    {
       
        MessageBroker.Default.Receive<UpdateStorySkillDescription>().Subscribe(async x =>
        {
           await UpdateStorySkillDescription(x.m_storySkillData);
        }
         ).AddTo(this);

    }


}




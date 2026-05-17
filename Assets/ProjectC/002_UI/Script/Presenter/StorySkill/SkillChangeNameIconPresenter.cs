using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Settings;

public class SkillChangeNameIconPresenter : MonoBehaviour
{

    private enum SkillNo
    {
        Skill1,
        Skill2,
    }

    [SerializeField] SkillNo m_skillNo = SkillNo.Skill1;
    [SerializeField] private CharacterCore m_characterCore;
    [SerializeField] private TextMeshProUGUI m_skillName;
    [SerializeField] private Image m_skillImage;
    [SerializeField] private Image m_backTextImage;

    [Header("背景画像拡大のための文字数にかける倍率")]
    [SerializeField] private float m_localizeSizeMagnification = 30.0f;
    [Header("最小の背景画像の横の長さ")]
    [SerializeField] private float m_minBackImageWidth = 200.0f;
    [Header("最大の背景画像の横の長さ")]
    [SerializeField] private float m_maxBackImageWidth = 400.0f;



    void Start()
    {
        if (m_characterCore == null)
        {
            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (core.GroupNo == CharacterGroupNumber.player)
                {
                    m_characterCore = core;
                    break;
                }
            }
        }

        if (m_characterCore == null)
        {
            return;
        }


        PlayerParameters playerParameters = m_characterCore.PlayerParameters;

        if (m_skillNo == SkillNo.Skill1)
        {
            m_characterCore.PlayerParameters.StorySkill1_ID.Subscribe
                (x =>
                {
                    ChangeSkillNameIcon(playerParameters);
                }
                );
        }
        else
        {
            m_characterCore.PlayerParameters.StorySkill2_ID.Subscribe
                (x =>
                {
                    ChangeSkillNameIcon(playerParameters);
                }
                );
        }

        LocalizationSettings.SelectedLocaleChanged += OnLanguageChanged;

    }

    private void OnLanguageChanged(UnityEngine.Localization.Locale newlocal)
    {
        PlayerParameters playerParameters = m_characterCore.PlayerParameters;
        ChangeSkillNameIcon(playerParameters);
    }

    private async void ChangeSkillNameIcon(PlayerParameters _playerParameters)
    {
        StorySkillData skillData;

        if (m_skillNo == SkillNo.Skill1)
        {
            skillData = StorySkillDataBaseManager.instance.GetStorySkillData(_playerParameters.StorySkill1_ID.Value);
        }
        else
        {
            skillData = StorySkillDataBaseManager.instance.GetStorySkillData(_playerParameters.StorySkill2_ID.Value);
        }

        if (skillData == null) { return; }

        if (m_skillName)
        {
            var NameText = await skillData.StorySkillName.GetLocalizedStringAsync();
            m_skillName.text = NameText;
        }

        if (m_skillImage)
            m_skillImage.sprite = skillData.Sprite;

        if (m_backTextImage)
        {
            Vector2 origin;
            origin = m_backTextImage.rectTransform.sizeDelta;
            origin.x = m_skillName.text.Length * m_localizeSizeMagnification;
            origin.x = Mathf.Clamp(origin.x, m_minBackImageWidth, m_maxBackImageWidth);
            m_backTextImage.rectTransform.sizeDelta = origin;

        }

    }

}

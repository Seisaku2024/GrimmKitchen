using Cysharp.Threading.Tasks;
using StorySkillType;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StorySkillType
{
    public enum StorySkillNo
    {
        Skill1,
        Skill2,
    }
}

public class StorySkillSlotData : MonoBehaviour
{
    [Header("=================================")]
    [Header("童話スキル名")]
    [SerializeField]
    private TextMeshProUGUI m_storySkillNameText = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_storySkillNameTextList = new();

    [Header("=================================")]
    [Header("童話スキルアイコン画像")]
    [SerializeField]
    private Image m_iconImage = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_iconImageList = new();

    [Header("=================================")]
    [Header("童話スキル装備画像")]
    [SerializeField]
    private Image m_storySkillSetImage = null;

    [Header("表示/非表示用")]
    [SerializeField]
    private List<GameObject> m_storySkillSetImageList = new();

    // 童話スキルスデータ
    protected StorySkillData m_storySkillData = null;

    public StorySkillData StorySkillData
    {
        get { return m_storySkillData; }
    }


    private StorySkillNo m_storySkillNo = StorySkillNo.Skill1;

    //========================================
    //              実行処理
    //========================================


    /// <summary>
    /// データをセット/更新する
    /// </summary>
    public void SetStorySkillData(StorySkillData _data, bool IsSkill1)
    {
        // 情報をセット
        m_storySkillData = _data;

        if (IsSkill1 == true)
        {
            m_storySkillNo = StorySkillNo.Skill1;
        }
        else
        {
            m_storySkillNo = StorySkillNo.Skill2;
        }

        SetSlotData();
    }

    virtual protected void SetSlotData()
    {

        // 童話スキル名
        SetStorySkillNameText();

        // 童話スキルアイコンををセット
        SetStorySkillIcon();

        // 装備画像をセット
        SetStorySkillImage();

        // アクティブ更新
        SetActiveGameObjectList();
    }

    virtual protected void SetActiveGameObjectList()
    {
        UIExtensions.CheckToSetActiveGameObjectList(m_storySkillNameText, m_storySkillNameTextList);
        UIExtensions.CheckToSetActiveGameObjectList(m_iconImage, m_iconImageList);
        UIExtensions.CheckToSetActiveGameObjectList(m_storySkillSetImage, m_storySkillSetImageList);
    }

    private async void SetStorySkillNameText(bool _active = true)
    {
        if (m_storySkillNameText == null) return;

        m_storySkillNameText.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_storySkillData == null) return;

        // 童話スキル名をセット
        var NameText = await m_storySkillData.StorySkillName.GetLocalizedStringAsync();
        m_storySkillNameText.text = NameText;
        m_storySkillNameText.gameObject.SetActive(true);
    }

    private void SetStorySkillIcon(bool _active = true)
    {
        if (m_iconImage == null) return;

        m_iconImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (m_storySkillData == null) return;

        // 童話スキル名をセット
        m_iconImage.sprite = m_storySkillData.Sprite;
        m_iconImage.gameObject.SetActive(true);
    }


    // 装備中画像
    private void SetStorySkillImage(bool _active = true)
    {
        if (m_storySkillSetImage == null) return;

        m_storySkillSetImage.gameObject.SetActive(false);

        // 初期化用
        if (_active == false) return;
        if (IsSettingStorySkill() == false) return;

        // 童話スキル装備画像をセット
        m_storySkillSetImage.gameObject.SetActive(true);
    }

    //　装備中のアイコン切替
    public void ChangeSetStorySkillImage(bool _active = true)
    {
        if (m_storySkillSetImage == null) return;

        m_storySkillSetImage.gameObject.SetActive(_active);

    }


    // スタッフポイントにセットされているかどうか
    public bool IsSettingStorySkill()
    {
        bool flg = false;

        foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (chara.GroupNo == CharacterGroupNumber.player)
            {
                if (m_storySkillNo == StorySkillNo.Skill1)
                {
                    if (m_storySkillData.StorySkill_ID == chara.PlayerParameters.StorySkill1_ID.Value)
                    {
                        flg = true;
                        break;
                    }
                    else
                    {
                        flg = false;
                        break;
                    }


                }
                else
                {
                    if (m_storySkillData.StorySkill_ID == chara.PlayerParameters.StorySkill2_ID.Value)
                    {
                        flg = true;
                        break;
                    }
                    else
                    {
                        flg = false;
                        break;
                    }
                }

            }

        }

        return flg;
    }

}

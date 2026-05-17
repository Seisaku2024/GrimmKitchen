using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class MasterStorySkillWindow : BaseWindow
{
    [Header("何秒後に自動的にウィンドウが閉じるのか")]
    [SerializeField]
    private float m_closeSec = 3.0f;

    [Header("童話スキルのアイコン")]
    [SerializeField]
    private Image m_image;

    [Header("童話スキルの取得文章")]
    [SerializeField]
    private TextMeshProUGUI m_textMeshPro;


    private StorySkill_ID m_storySkill_ID = StorySkill_ID.None;
    public StorySkill_ID StorySkill_ID { set { m_storySkill_ID = value; } }

    private StorySkillData m_storySkillData;

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            if (StorySkillDataBaseManager.instance)
            {
                m_storySkillData = StorySkillDataBaseManager.instance.GetStorySkillData(m_storySkill_ID);
            }
            else
            {
                return;
            }

            if (m_image && m_storySkillData)
            {
                m_image.sprite = m_storySkillData.Sprite;
            }

            if (m_textMeshPro)
            {
                LocalizedString localizedString = new();

                localizedString = new LocalizedString(tableReference: "MasterStorySkill", entryReference: "GetStorySkill")
                {
                    {"SkillName",new StringVariable{Value=m_storySkillData.StorySkillName.GetLocalizedString()} }
                };

                if(localizedString.GetLocalizedString().Length>0)
                m_textMeshPro.text = localizedString.GetLocalizedString();

            }

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    public override async UniTask OnUpdate()
    {
        #region nullチェック

        #endregion


        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                // 閉じる
                if (IsClose()) return;

                // 指定秒数後にウィンドウ閉じる
                m_closeSec -= Time.deltaTime;
                if (m_closeSec <= 0.0f)
                {
                    return;
                }

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

}

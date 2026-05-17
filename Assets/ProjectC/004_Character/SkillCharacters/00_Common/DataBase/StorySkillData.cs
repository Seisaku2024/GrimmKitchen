using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Localization.Settings;

[CreateAssetMenu]
[System.Serializable]
public class StorySkillData : ScriptableObject
{
    //ストーリースキルのデータ（山本）

    [Header("ストーリースキルプレハブ")]
    [SerializeField] private GameObject m_storySkillPrefab = null;
    public GameObject StorySkillPrefab { get { return m_storySkillPrefab; } }

    [Header("召喚する際のエフェクト")]
    [SerializeField] private GameObject m_spellEffect = null;
    public GameObject SpellEffectPrefab { get { return m_spellEffect; } }

    [Header("召喚スキルのアイコン")]
    [SerializeField] private Sprite m_spellEffectSprite = null; 
    public Sprite Sprite { get { return m_spellEffectSprite; } }

    [Header("召喚する際に消費するBP")]
    [SerializeField] private float m_payBP = 0.0f;
    public float PayBP { get { return m_payBP; } }

    [Header("召喚にかかる時間(キャストタイム)")]
    [SerializeField] private float m_castTime = .0f;
    public float CastTime { get { return m_castTime; } }

    [Header("滞在時間(攻撃継続時間)")]
    [SerializeField] private float m_stayTime = .0f;
    public float StayTime { get { return m_stayTime; } }

    [Header("プレイヤーからどのくらい離れた位置に生成するか")]
    [SerializeField] private float m_distance = 0.0f;
    public float Distance { get { return m_distance; } }

    [Header("ストーリースキルのID")]
    [SerializeField] private StorySkill_ID m_storySkill_ID = StorySkill_ID.None;
    public StorySkill_ID StorySkill_ID { get { return m_storySkill_ID; } }

    [Header("ストーリースキル名")]
    [SerializeField] private UnityEngine.Localization.LocalizedString m_storySkillName;
    public UnityEngine.Localization.LocalizedString StorySkillName { get { return m_storySkillName; } }

    [Header("ストーリースキルの紹介文")]
    [SerializeField] private UnityEngine.Localization.LocalizedString m_storySkillDescriptionText;
    public UnityEngine.Localization.LocalizedString StorySkillDescriptionText { get {  return m_storySkillDescriptionText; } }

    [Header("スキル紹介動画")]
    [SerializeField]private VideoClip m_storySkillClip = null;
    public VideoClip StorySkillClip { get { return m_storySkillClip; } }

    [Header("童話スキル名の背景画像の横の長さ")]
    [SerializeField] private float m_backImageWidth = 0.0f;
    public float BackImageWidth { get { return m_backImageWidth; } }

    [Header("攻撃の詳細情報")]
    [SerializeField] private List<AttackDamageData> m_attackData;
    public List<AttackDamageData> AttackDamageDatas { get { return m_attackData; } }

    [Header("召喚時にSphereCast判定するレイヤーマスク")]
    [SerializeField] private LayerMask m_layerMask;
    public LayerMask LayerMask { get { return m_layerMask; } }

    [Header("SphereCastで除外するオブジェクトのレイヤーマスク")]
    [SerializeField] private LayerMask m_exclusionlayerMask;
    public LayerMask ExclusionLayerMask { get { return m_exclusionlayerMask; } }

    [Header("習得フラグ")]
    [SerializeField]
    private bool m_masterFlg = false;
    public bool MasterFlg { get { return m_masterFlg; } }

    [Header("デバック用取得フラグ")]
    [SerializeField]
    private bool m_debugMasterFlg = false;

    //================================================================================
    //データ

    private MasterStorySkillData m_masterStorySkillData = null;

    public MasterStorySkillData MasterStorySkillData
    {
        get
        {
            if (m_masterStorySkillData == null) m_masterStorySkillData = new(m_storySkill_ID);
            return m_masterStorySkillData;
        }
    }
    //===============================================================================


    //========================================
    //              実行処理
    //========================================

    public void Load(MasterStorySkillDataSaveLoad _data)
    {
        if (_data == null) return;
        m_masterStorySkillData = new(m_storySkill_ID);
        m_masterStorySkillData.IsMaster = _data.IsMaster;
        m_masterFlg = m_masterStorySkillData.IsMaster;

        // デバック用
        if(m_debugMasterFlg)
        {
            m_masterFlg = true;
        }

    }

    public void MasterSkill()
    {
        if (m_masterStorySkillData == null) m_masterStorySkillData = new(m_storySkill_ID);

        m_masterStorySkillData.IsMaster = true;
        m_masterFlg = true;
    }

}

[System.Serializable]
public class MasterStorySkillDataSaveLoad
{
    public MasterStorySkillDataSaveLoad(StorySkill_ID _id)
    {
        var data = StorySkillDataBaseManager.instance.GetStorySkillData(_id);
        if (data == null) return;
        IsMaster = false;
    }


    public MasterStorySkillDataSaveLoad(StorySkill_ID _id, MasterStorySkillData _data)
    {
        if (_data == null) return;
        StorySkill_ID = _id;
        IsMaster = _data.IsMaster;
    }

    public StorySkill_ID StorySkill_ID = StorySkill_ID.None;
    public bool IsMaster = false;

}


[System.Serializable]
public class MasterStorySkillData
{
    public MasterStorySkillData(StorySkill_ID _id)
    {
        var data = StorySkillDataBaseManager.instance.GetStorySkillData(_id);
        if (data == null) return;

        IsMaster = false;
    }

    public bool IsMaster = false;
}

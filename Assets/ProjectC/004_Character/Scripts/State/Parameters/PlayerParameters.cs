using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using NaughtyAttributes;
using UniRx;
using CI.QuickSave;
using static FoodData;
using SaintsField;
using Unity.VisualScripting;
using StorySkillType;

public class PlayerParameters : MonoBehaviour
{
    [Header("プレイヤー専用パラメータ")]
    [SerializeField] private PlayerStatus m_playerStatus = new();
    public PlayerStatus PlayerStatus { get { return m_playerStatus; } set { m_playerStatus = value; } }

    // PlayerStatusSaveLoaderに移動(セーブタイミングを他処理と揃えるため)
    //[SerializeField, SaintsField.ReadOnly]
    //private StrengtheningStatus m_nowStatus = new();

    public CharacterStatus CharaStatus() { return PlayerStatusManager.instance.CharacterStatus(); }

    // 料理を手に持っている可のフラグ 上甲
    public bool m_isFoodHold = false;

    //敵をサーチする範囲（山本）
    [Header("敵の方向を向く際のサーチ範囲")]
    [SerializeField] private float m_serchEnemyDist = 0.0f;
    public float SearchEnemyDist { get { return m_serchEnemyDist; } }

    //回避用変数（山本）
    [SerializeField] public float m_rollingPow = 2f;
    [SerializeField] public float m_rollingAnimeSpeed = 2f;

    //キー入力量の取得（山本）
    [SerializeField] private float m_speedStick = 0.0f;
    public float SpeedStick { get { return m_speedStick; } set { m_speedStick = value; } }

    // =====================
    // 設置アイテム用（吉田）
    // =====================
    #region
    public struct PutItemInfo
    {
        public ItemInfo.ItemTypeID m_itemTypeID;
        public uint m_itemID;
    }
    [HideInInspector]
    public PutItemInfo m_putItemInfo;

    public void SetPutItemInfo(ItemInfo.ItemTypeID _itemTypeID, uint _itemID)
    {
        m_putItemInfo.m_itemTypeID = _itemTypeID;
        m_putItemInfo.m_itemID = _itemID;
    }

    // =====================
    // アイテム採取用（吉田）
    // =====================

    // 取得可能状態にあるアイテム
    [HideInInspector]
    public AssignItemID m_ableGatheringItem = null;

    // ==========================
    // アイテム持ち状態用（山本）
    // ==========================
    [Header("アイテムを持つ場所")]
    [SerializeField] public Transform m_holdTrans = null;


    // =====================
    // アイテム投げる用（吉田）
    // =====================

    [Header("アイテム投げる用")]
    // 投げるときの初速度
    [HideInInspector]
    public Vector3 m_throwPower = new Vector3(0, 0, 0);

    // 投げるときの回転
    [SerializeField]
    [Tooltip("投げるときの回転")]
    public Vector3 m_throwTorque = new Vector3(0.3f, 0, 0.3f);

    // マウスの位置に表示するプレハブ
    [SerializeField]
    public GameObject m_mouseThrowAim;

    // 投げる位置
    [SerializeField]
    public Transform m_handTrans;

    [Header("カメラ切り替える用")]
    // AimCamera
    [SerializeField]
    public GameObject m_throwAimCamera = null;

    // プレイヤーの背後に追従するカメラ
    [SerializeField]
    public GameObject m_playerfollowCamera = null;

    [SerializeField]
    public Unity.Cinemachine.CinemachineBrain m_cinemachineBrain = null;
    #endregion

    //---------------------------------------------------------------------------------
    //========================
    //童話スキル関係（山本）
    //========================
    #region
    [Header("スキル関係")]
    [SerializeField] public GameObject m_skillPrefab;//緊急用
    [SerializeField] public GameObject m_spellEffect;//スキル詠唱用のエフェクト

    //IDで装備スキルを管理するように変更途中（山本）
    [Header("装備中の童話スキル1のID")]
    [SerializeField] SkillIDReactiveProperty m_storySkill1_ID;
    public SkillIDReactiveProperty StorySkill1_ID
    {
        get { return m_storySkill1_ID; }
        set { m_storySkill1_ID = value; }
    }


    [Header("装備中の童話スキル2のID")]
    [SerializeField] SkillIDReactiveProperty m_storySkill2_ID;
    public SkillIDReactiveProperty StorySkill2_ID
    {
        get { return m_storySkill2_ID; }
        set { m_storySkill2_ID = value; }
    }

    //スキル監視用
    //Rトリガー,第1スキル
    private GameObject m_obserbSkill1 = null;
    public GameObject ObserbSkill1 { set { m_obserbSkill1 = value; } get { return m_obserbSkill1; } }
    //Lトリガー,第2スキル
    private GameObject m_obserbSkill2 = null;
    public GameObject ObserbSkill2 { set { m_obserbSkill2 = value; } get { return m_obserbSkill2; } }

    //スキル使用できるかのフラグ
    //Right(第1スキル)
    private bool m_useSkill1Flg = false;
    public bool CanUseSkill1Flg
    {
        get { return m_useSkill1Flg; }
        set { m_useSkill1Flg = value; }
    }
    //Left（第2スキル）
    private bool m_useSkill2Flg = false;
    public bool CanUseSkill2Flg
    {
        get { return m_useSkill2Flg; }
        set { m_useSkill2Flg = value; }
    }

    //第1スキル,第2スキルどちらが使用されたか確認用フラグ
    private bool m_triggerStorySkill_1 = false;
    private bool m_triggerStorySkill_2 = false;
    public bool TriggerStorySkill_1
    {
        get { return m_triggerStorySkill_1; }
        set { m_triggerStorySkill_1 = value; }
    }
    public bool TriggerStorySkill_2
    {
        get { return m_triggerStorySkill_2; }
        set { m_triggerStorySkill_2 = value; }
    }

    //童話スキルのキャストタイムの進捗度(山本)------------------------------------------------
    private FloatReactiveProperty m_castTimeProgress = new(0.0f);
    public FloatReactiveProperty CastTimeProgress { get { return m_castTimeProgress; } set { m_castTimeProgress = value; } }
    #endregion

    //----------------------------------------------------------------------------------------
    //===================================
    //武器出現・消失エフェクト関係（山本）
    //===================================

    // 武器が消失しているかのフラグ（山本）
    [HideInInspector]
    public bool m_isVanishWeapon = false;
    [Header("武器出現関連")]        //武器の消失イベント（山本）
    [Header("武器消失のタイムラインイベント")]
    [SerializeField] private TimelineAsset m_vanishWeaponEvent;
    [Header("武器出現のタイムラインイベント")]
    [SerializeField] private TimelineAsset m_appearWeaponEvent;
    [Header("消失した武器が出現するまでの時間")]
    [SerializeField] private float m_appearEventTime = 5.0f;

    private float m_eventTime = 0.0f;

    //-------------------------------------------------------------------------------------

    // =====================
    // UI表示用（吉田）
    // =====================
    [Header("UI表示用")]
    // 関数でUIの表示設定する
    [SerializeField]
    private ActionUIController m_actionUIController;

    [Header("ステータス強化ログ")]
    [SerializeField]
    private StrengtheningLogController m_strengtheningLog;

    //====================================
    //ゲームオーバー時のリスタート（山本）
    //====================================
    private Vector3 m_playerRestartPosition = Vector3.zero;
    public Vector3 PlayerRestartPosition
    {
        get { return m_playerRestartPosition; }
        set { m_playerRestartPosition = value; }
    }

    private Vector3 m_playerRestartForward = Vector3.zero;
    public Vector3 PlayerRestartForward
    {
        get { return m_playerRestartForward; }
        set { m_playerRestartForward = value; }
    }

    //============================================
    // プレイヤー被弾時のダメージ表現(山本)
    //============================================
    [Header("プレイヤー被弾時のダメージスプライトの出現時間")]
    [SerializeField]
    private float m_hitDamagedTextureShowTime = 0.1f;
    public float HitTextureShowTime { get { return m_hitDamagedTextureShowTime; } }

    [Header("プレイヤー被弾時のダメージスプライトの消失時間")]
    [SerializeField]
    private float m_hitDamagedTextureVanishTime = 0.5f;
    public float HitTextureVanishTime { get { return m_hitDamagedTextureVanishTime; } }

    [Header("プレイヤーの入場するポータルのトランスフォーム")]
    [SerializeField]
    private Transform m_portalTrans = null;
    public Transform PortalTrans { get { return m_portalTrans; } set { m_portalTrans = value; } }

    //==================================================
    //ストーリー会話関係
    //==================================================
    [Header("会話用カメラのTransform")]
    [SerializeField]
    private Transform m_conversationCameraTransform = null;
    public Transform ConversationCameraTransform => m_conversationCameraTransform;

    private void Start()
    {
        m_playerStatus.Init(
            PlayerDataBaseManager.instance.DataBase.Stamina,
            PlayerStatusManager.instance.NowStatus());

        // 童話スキルロード
        LoadStorySkill();
    }

    public void StartVanishWeapon()
    {
        m_eventTime = m_appearEventTime;
    }

    public void UpdateVanishWeapon(Animator animator)
    {
        if (!m_isVanishWeapon) return;

        if (m_eventTime <= 0.0f)
        {
            m_isVanishWeapon = false;

            if (animator.TryGetComponent<PlayableDirector>(out PlayableDirector director))
            {
                director.Play(m_appearWeaponEvent);
                //アニメーター側に武器を所持していることを伝える
                animator.SetFloat("HasWeapon", 1.0f);
            }
        }
        else
        {
            m_eventTime -= Time.deltaTime;
        }
    }

    public void AppearWeapon(Animator animator)
    {
        if (!m_isVanishWeapon) return;
        if (animator.TryGetComponent<PlayableDirector>(out PlayableDirector director))
        {
            director.Play(m_appearWeaponEvent);
            m_isVanishWeapon = false;
            //アニメーター側に武器を所持していることを伝える
            animator.SetFloat("HasWeapon", 1.0f);
        }
    }

    public void HideWeapon(Animator animator)
    {
        if (m_isVanishWeapon) return;
        if (animator.TryGetComponent<PlayableDirector>(out PlayableDirector director))
        {
            director.Play(m_vanishWeaponEvent);
            m_isVanishWeapon = true;
            //アニメーター側に武器を所持してないことを伝える
            animator.SetFloat("HasWeapon", 0.0f);
        }
    }

    public void AddActionUIState(ActionUIController.ActionUIState _state)
    {
        if (m_actionUIController == null)
        {
            Debug.LogError("ActionUIControllerが設定されていません");
            return;
        }

        m_actionUIController.AddState(_state);
    }

    public void RemoveActionUIState(ActionUIController.ActionUIState _state)
    {
        if (m_actionUIController == null)
        {
            Debug.LogError("ActionUIControllerが設定されていません");
            return;
        }

        m_actionUIController.RemoveState(_state);
    }
    public void AddAnyActionUIState(ActionUIController.ActionUIState _state, float _distance = 1.0f)
    {
        if (m_actionUIController == null)
        {
            Debug.LogError("ActionUIControllerが設定されていません");
            return;
        }

        m_actionUIController.AddAnyActionState(_state, _distance);
    }
    public void RemoveAnyActionUIState(ActionUIController.ActionUIState _state)
    {
        if (m_actionUIController == null)
        {
            Debug.LogError("ActionUIControllerが設定されていません");
            return;
        }

        m_actionUIController.RemoveAnyActionState(_state);
    }

    //==================================================================
    //ActionItemWindow用（山本）
    //=================================================================
    [Header("ActionItemController登録")]
    [SerializeField]
    private GameObject m_actionItemWindowController = null;
    public GameObject ActionItemWindowController { get { return m_actionItemWindowController; } }

    // 童話スキルのセーブ
    public void SaveStorySkill()
    {
        var writer = QuickSaveWriter.Create("PlayerStorySkillStatus");

        //童話スキル関係セーブ(山本)
        writer.Write<StorySkill_ID>("StorySkill01", m_storySkill1_ID.Value);
        writer.Write<StorySkill_ID>("StorySkill02", m_storySkill2_ID.Value);

        writer.Commit();
    }


    // 童話スキルのロード
    public void LoadStorySkill()
    {
        if (!QuickSaveReader.RootExists("PlayerStorySkillStatus"))
        {
            //Debug.LogError("セーブデータがありません:PlayerStorySkillStatus");

            if (StorySkillDataBaseManager.instance)
            {
                m_storySkill1_ID.Value = StorySkillDataBaseManager.instance.StorySkill_ID1;
                m_storySkill2_ID.Value = StorySkillDataBaseManager.instance.StorySkill_ID2;
            }
            else
            {
                m_storySkill1_ID.Value = StorySkill_ID.None;
                m_storySkill2_ID.Value = StorySkill_ID.None;
            }

            SaveStorySkill();
        }

        QuickSaveReader reader = QuickSaveReader.Create("PlayerStorySkillStatus");

        //童話スキル関係ロード（山本）
        m_storySkill1_ID.Value = reader.Read<StorySkill_ID>("StorySkill01");
        m_storySkill2_ID.Value = reader.Read<StorySkill_ID>("StorySkill02");

    }

    public void Strengthening(AddStatus addStatus)
    {
        int addVal = PlayerStatusManager.instance.NowStatus().UseFood(CharaStatus(), addStatus, this);
        m_strengtheningLog.SetAddStatus(addStatus, addVal);
    }

}

[Serializable]
public class StrengtheningStatus
{
    [SerializeField, SaintsField.ReadOnly]
    private float m_nowAttack;
    public float NowAttack => m_nowAttack;

    [SerializeField, SaintsField.ReadOnly]
    private float m_nowHp;
    public float NowHp => m_nowHp;

    [SerializeField, SaintsField.ReadOnly]
    private float m_nowDefence;
    public float NowDefence => m_nowDefence;

    [SerializeField, SaintsField.ReadOnly]
    private float m_nowStamina;
    public float NowStamina => m_nowStamina;

    public void Reset(PlayerDataBase data)
    {
        m_nowAttack = data.CharacterStatus.m_attack.Value;
        m_nowHp = data.CharacterStatus.m_hp.Value;
        m_nowDefence = data.CharacterStatus.m_defense.Value;
        m_nowStamina = data.Stamina.MaxStamina.Value;
    }

    public void Save(QuickSaveWriter writer)
    {
        writer.Write<float>("Attack", m_nowAttack);
        writer.Write<float>("Hp", m_nowHp);
        writer.Write<float>("Defence", m_nowDefence);
        writer.Write<float>("MaxStamina", m_nowStamina);
    }

    public void Load(QuickSaveReader reader)
    {
        m_nowAttack = reader.Read<float>("Attack");
        m_nowHp = reader.Read<float>("Hp");
        m_nowDefence = reader.Read<float>("Defence");
        m_nowStamina = reader.Read<float>("MaxStamina");
    }

    // 少しも強化できなかったor既に上限ならfalse
    public int UseFood(CharacterStatus status, AddStatus addStatus, PlayerParameters playerParameters)
    {
        StrengtheningUpperLimit limit = PlayerDataBaseManager.instance.DataBase.GetParameterLimit(PlayerStatusManager.instance.StrengtheningCap);
        if (limit == null) return 0;
        float UpValue = addStatus.AddValue;
        switch (addStatus.AddType)
        {
            case AddStatus.AddStatusType.Attack:
                m_nowAttack += addStatus.AddValue;
                if (m_nowAttack >= limit.AttackLimit)
                {
                    UpValue = addStatus.AddValue + (limit.AttackLimit - m_nowAttack);
                    m_nowAttack = limit.AttackLimit;
                }
                status.m_attack.Value = m_nowAttack;
                break;
            case AddStatus.AddStatusType.Hp:
                m_nowHp += addStatus.AddValue;
                if (m_nowHp >= limit.HpLimit)
                {
                    // 最大値を超えないように回復するように修正（山本）
                    //playerParameters.CharaStatus.m_hp.Value += m_nowHp - limit.HpLimit;
                    UpValue = addStatus.AddValue + (limit.HpLimit - m_nowHp);
                    status.m_hp.Value = limit.HpLimit;
                    m_nowHp = limit.HpLimit;
                }
                else
                {
                    status.m_hp.Value += addStatus.AddValue;
                }
                status.MaxHP.Value = m_nowHp;
                break;
            case AddStatus.AddStatusType.Defence:
                m_nowDefence += addStatus.AddValue;
                if (m_nowDefence >= limit.DefenceLimit)
                {
                    UpValue = addStatus.AddValue + (limit.DefenceLimit - m_nowDefence);
                    m_nowDefence = limit.DefenceLimit;
                }
                status.m_defense.Value = m_nowDefence;
                break;
            case AddStatus.AddStatusType.Stamina:
                m_nowStamina += addStatus.AddValue;
                if (m_nowStamina >= limit.StaminaLimit)
                {
                    UpValue = addStatus.AddValue + (limit.StaminaLimit - m_nowStamina);
                    m_nowStamina = limit.StaminaLimit;
                }
                playerParameters.PlayerStatus.StaminaData.MaxStamina.Value = m_nowStamina;
                break;
        }
        return (int)UpValue;
    }

}
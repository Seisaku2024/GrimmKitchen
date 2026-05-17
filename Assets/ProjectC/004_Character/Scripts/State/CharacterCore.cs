using Arbor;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.VFX;
using SaintsField;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using CriWare;
using Unity.VisualScripting;


// キャラクターの基盤クラス
public partial class CharacterCore : MonoBehaviour, IDamageable
{
    // グループナンバー
    [SerializeField] private CharacterGroupNumber m_groupNo;
    public CharacterGroupNumber GroupNo { get { return m_groupNo; } set { m_groupNo = value; } }

    private bool m_doFriendlyFire = false;
    public bool DoFriendlyFire { get { return m_doFriendlyFire; } set { m_doFriendlyFire = value; } }

    [SerializeField] private MyCharacterController m_charaCtrl;
    public MyCharacterController CharaCtrl { get { return m_charaCtrl; } }
    [SerializeField] public Animator m_animator;

    // 入力インターフェース
    private IInputProvider m_inputProvider = NullCharacterIP.NullInstance;
    public IInputProvider InputProvider => m_inputProvider;//山本追加

    // どの入力インターフェースを使うか
    public enum InputTypes
    {
        None,
        [InspectorName("プレイヤー")] Player,
        [InspectorName("敵")] Enemy,
        [InspectorName("スキルキャラクター")] SkillChara,
        [InspectorName("経営")] Management,
        [InspectorName("NPC")] NPC,
    }
    [SerializeField] private InputTypes m_inputType;
    public InputTypes InputType { get { return m_inputType; } }

    // 移動の種類
    public enum MoveTypes
    {
        Loose,  // 緩やかに移動
        Sharp,  // びたびたに移動
    }

    [SerializeField]
    private MoveTypes m_moveType = MoveTypes.Loose;

    public MoveTypes MoveType { get { return m_moveType; } }

    [SerializeField] private bool m_isPushedOtherObj = true;

    // ヒットストップ用プロパティ
    public float HitStopRemainingTime
    {
        get; set;
    }

    //ヒットストップするためのプレイヤーの攻撃エフェクト保存用（山本）
    public VisualEffect TempPlayerAttackEffect
    {
        get; set;
    }


    //=====================
    // ヒット時演出用
    // ====================
    // マテリアルの色パラメーターのID 文字列は、使用シェーダーの詳細のPropertyから
    private static readonly int PROPERTY_COLOR = Shader.PropertyToID("_BaseColor");
    // モデルのRenderer
    [SerializeField] private Renderer m_renderer;
    // モデルのマテリアル複製
    private Material m_material;
    // DOTween用
    private DG.Tweening.Sequence m_seq;

    // 被弾時のビネット表現用（山本）
    //GrovalVolume
    private Volume m_globalVolume;
    private bool m_changeVinetColFlg = false;

    // ピンチ音
    CriAtomExPlayback m_pinchSE = new CriAtomExPlayback();

    // 移動用変数
    // 移動速度
    [SerializeField] private VisualEffect m_dashEffect; // ダッシュ用エフェクト

    private bool m_isRun = false;
    //ダッシュキー押しっぱなしを防ぐ用（山本）
    private bool m_isPush = false;
    private bool m_isNoKnockBack = false;
    private float m_knockBackMultiplier = 1;
    private Vector3 m_knockBackVec;
    bool m_isNoDamage = false;
    private bool m_isNoStamina = false; // スタミナなくて動けない状態

    [ShowIf(nameof(m_groupNo) + ">", CharacterGroupNumber.enemy)]
    [SerializeField] private CharacterStatus m_characterStatus;
    public CharacterStatus Status { get { return m_characterStatus; } }

    [Header("各キャラクター専用のパラメータ")]
    [Tooltip("プレイヤーだった場合のみ参照をいれる")]
    public PlayerParameters PlayerParameters;

    [Tooltip("敵だった場合のみ参照をいれる")]
    public EnemyParameters EnemyParameters;

    [Tooltip("スキルだった場合のみ参照をいれる")]
    public PlayerSkillsParameters PlayerSkillsParameters;

    [Tooltip("NPCだった場合のみ参照をいれる")]
    public NPCParameters NPCParameters;


    private Transform m_attackEnemyTrans;
    private Tweener m_damageTween;

    // TODO:リスタート時の初期化関連　伊波
    // 展示会終わったら消す
    private Vector3 m_initPos;
    public void EnemyResetPos()
    {
        if (m_inputType != InputTypes.Enemy) return;

        //NPCは初期位置に戻さないようにする（山本）
        if (m_groupNo == CharacterGroupNumber.NPC) return;

        m_charaCtrl.SetPositionMotor(m_initPos);
    }

    // FinishJumpFlgの有無（山本）
    private bool m_IsFinishJumpFlg = false;
    public bool IsFinishJumpFlg => m_IsFinishJumpFlg;

    // 該当名のパラメータがないかAnimatorControllerにないか確認する関数（山本）
    bool HasParameter(string _parameterName)
    {
        foreach (var param in m_animator.parameters)
        {
            if (param.name == _parameterName)
                return true;
        }
        return false;
    }

    // アニメーションをストップするフラグ
    private bool m_stopAnimationSpeedFlg = false;
    public bool StopAnimationSpeedFlg { get { return m_stopAnimationSpeedFlg; } set { m_stopAnimationSpeedFlg = value; } }


    private void Awake()
    {
        if (m_animator)
        {
            // 非アクティブにした時に、アニメーターステートが初期化されるのを防ぐ
            m_animator.keepAnimatorStateOnDisable = true;

            // FinishJumpFlgが存在しないか確認
            m_IsFinishJumpFlg = HasParameter("FinishJumpFlg");

        }

        if (m_renderer)
        {
            // materialにアクセスして、自動生成されるマテリアルを保持
            m_material = m_renderer.material;
        }

        //装備しているスキルから消費BPを取得する
        //SetStorySkillBP();

        //キャラクターを正面へ回転(山本)
        SetRotateToTarget(transform.forward, false);
    }

    void Start()
    {
        m_initPos = transform.position;

        IMetaAI<CharacterCore>.Instance.RegisterObject(this);

        // インスペクターで選択した入力インターフェースに応じて、選択
        if (m_inputType == InputTypes.Player)
        {
            m_inputProvider = new PlayerInputProvider();

            //スタート時にリスタート位置,向きを記録する
            if (gameObject.TryGetComponent(out PlayerParameters parameters))
            {
                parameters.PlayerRestartPosition = gameObject.transform.root.position;
                parameters.PlayerRestartForward = gameObject.transform.root.forward;
            }

            //GlovalVolumeをセット
            // オブジェクトを取得
            var data = GameObject.Find("Global Volume");
            if (data == null)
            {
                Debug.LogError("GlobalVolumeがヒエラルキーに存在していません");
                return;
            }

            // コンポーネントを取得
            m_globalVolume = data.GetComponent<Volume>();
            if (m_globalVolume == null)
            {
                Debug.LogError("Volumeコンポーネントがアタッチされていません");
                return;
            }

        }
        else if (m_inputType == InputTypes.Enemy || m_inputType == InputTypes.SkillChara || m_inputType == InputTypes.Management)
        {
            m_inputProvider = GetComponentInChildren<EnemyInputProvider>();
        }

        // ダッシュエフェクトがセットされていれば、最初は止めておく
        if (m_dashEffect != null)
        {
            m_dashEffect.Stop();
        }

        if (m_groupNo == CharacterGroupNumber.player && PlayerParameters)
        {
            m_characterStatus = PlayerParameters.CharaStatus();
            m_characterStatus.m_hp.Value = m_characterStatus.MaxHP.Value;

            // スキルがチェンジされたらスキルの値を変更する（山本）
            PlayerParameters.StorySkill1_ID.Subscribe
                (x =>
                {
                    PlayerParameters.StorySkill1_ID.Value = x;
                    PlayerStatus status = PlayerParameters.PlayerStatus;
                    var bp1 = StorySkillDataBaseManager.instance.GetStorySkillData(PlayerParameters.StorySkill1_ID.Value);
                    status.m_bpSkill_1.Value = 0.0f;
                    status.MaxBPSkill_1 = bp1.PayBP;
                }
                ).AddTo(this);

            PlayerParameters.StorySkill2_ID.Subscribe
                (x =>
                {
                    PlayerParameters.StorySkill2_ID.Value = x;
                    PlayerStatus status = PlayerParameters.PlayerStatus;
                    var bp2 = StorySkillDataBaseManager.instance.GetStorySkillData(PlayerParameters.StorySkill2_ID.Value);
                    status.m_bpSkill_2.Value = 0.0f;
                    status.MaxBPSkill_2 = bp2.PayBP;
                }
                );


            // 最初に装備したスキルは発動できるようにする（山本）
            SetInitializeStorySkillBP();

        }
        if (m_groupNo == CharacterGroupNumber.enemy && EnemyParameters)
        {
            m_characterStatus = new(EnemyParameters.GetEnemyData().StageCharaStatus);
        }

        // アップデート処理
        this.UpdateAsObservable()
            .Where(_ => enabled)
            .Subscribe(_ =>
            {
                // ヒットストップ処理 Animatorのスピードで再現
                if (HitStopRemainingTime <= 0)
                {
                    if (m_animator)
                    {
                        if (m_stopAnimationSpeedFlg)
                        {
                            m_animator.speed = 0.0f;
                        }
                        else
                        {
                            m_animator.speed = 1.0f;
                        }
                    }

                    //攻撃エフェクトが保存されていて、HitStop時間外なら（山本）
                    if (TempPlayerAttackEffect)
                    {
                        //if (TempPlayerAttackEffect.HasFloat("AddAngle"))
                        //{
                        //    if (TempPlayerAttackEffect.transform.TryGetComponent(out AttackEffectRegistration effct))
                        //    {
                        //        //AddAngleがあったら元に戻す
                        //        TempPlayerAttackEffect.SetFloat("AddAngle", effct.OriginAddAngle);
                        //    }
                        //}

                        if (TempPlayerAttackEffect.transform.TryGetComponent(out AttackEffectRegistration effct))
                        {
                            //変更したVFXのプロパティを元の値へと戻す
                            effct.ReturnVFXPropertiesValue();
                        }

                        //AttackEffectのスピードを元に戻す
                        TempPlayerAttackEffect.playRate = 1.0f;
                    }

                }
                else
                {
                    if (m_animator)
                    {
                        m_animator.speed = 0.0f;
                    }

                    //攻撃エフェクトが保存されていて、HitStop時間内なら（山本）
                    if (TempPlayerAttackEffect)
                    {


                        if (TempPlayerAttackEffect.transform.TryGetComponent(out AttackEffectRegistration effct))
                        {
                            //変更したVFXのプロパティを指定した値へと変更
                            effct.SetVFXPropertiesValue(0.0f);

                        }

                        //AttackEffectのスピードを止める
                        TempPlayerAttackEffect.playRate = 0.0f;

                    }

                }

                HitStopRemainingTime -= Time.deltaTime;
                if (HitStopRemainingTime < 0) HitStopRemainingTime = 0;
            }
            );
    }

    // ダメージ処理
    void IDamageable.Damaged(DamageNotification _dmgData, Collider _hitCol, float _knockBackMultiplier, bool _isStrongAttack, KnockBackType _knockBackType)
    {
        //ノーダメージフラグ中に早期リターン（山本）
        if (m_isNoDamage) { return; }


        // ダメージ受けたら画面を赤くする(山本)
        if (m_inputType == InputTypes.Player)
        {
            // カメラにセットされたダメージ用スプライトの
            // アルファ値を制御するコンポーネントにアクセス(山本)
            if (Camera.main.transform.TryGetComponent(out ChangeTextureAlphaNum changeTexture))
            {
                changeTexture.SetImageAlphaNum(1.0f);

                m_damageTween = DOVirtual.Float(1.0f, 0.0f, PlayerParameters.HitTextureVanishTime,
             value => changeTexture.SetImageAlphaNum(value)
             ).OnComplete
             (
               () =>
               {
                   changeTexture.SetImageAlphaNum(0.0f);
                   m_damageTween.Kill();
                   m_damageTween = null;
               }
               ).SetDelay(PlayerParameters.HitTextureShowTime);
            }
        }


        //死亡判定とスキル等による怯み判定がバッティングした際に強制的に死亡状態へと移行させる処理（山本）
        if (m_characterStatus.m_hp.Value <= 0.0f)
        {
            //死亡トリガーを逃れてしまったら強制的に死亡状態へと移行する（山本）
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            {
                m_characterStatus.m_hp.Value = 0.0f;
                m_animator.SetTrigger("IsDead");
                //HP0でもアタックエフェクトが表示されるようにする（山本）
                _dmgData.m_replyIsHit = true;
            }
            return;
        }

        if (m_characterStatus.m_hp.Value > 0)
        {
            // ダメージ処理
            Transform managerObj = transform.root.Find("ConditionManager");
            _dmgData.m_finalDamageVal = _dmgData.m_status.m_attack.Value * _dmgData.m_attackData.Attack * (_dmgData.m_status.m_attack.Value / Status.m_defense.Value);
            if (managerObj != null && managerObj.TryGetComponent(out ConditionManager manager))
            {
                _dmgData.m_finalDamageVal *= manager.DamageMulti();
            }
            if (_dmgData.m_finalDamageVal < 1.0f) _dmgData.m_finalDamageVal = 1.0f;
            m_characterStatus.m_hp.Value -= _dmgData.m_finalDamageVal;

            //!@todo :ヒットエフェクトが出来たらここは消す
            if (_hitCol)
            {
                SoundManager.Instance.Start3DPlayback("HitSoundBlow", _hitCol.transform.position);
            }

            // ヒットストップ処理
            HitStopRemainingTime = _dmgData.m_hitStopTime;
            // ヒット時演出
            HitFadeBlink(Color.red);
        }

        // 0以下で、倒れる
        if (m_characterStatus.m_hp.Value <= 0.0f)
        {
            m_characterStatus.m_hp.Value = 0.0f;
            m_animator.SetBool("IsDead", true);
            //HP0でもアタックエフェクトが表示されるようにする（山本）
            _dmgData.m_replyIsHit = true;
            return;
        }

        // プレイヤーのみピンチ音出す
        if (m_inputType == InputTypes.Player)
        {
            // ピンチ音　残りダメージが残り10％だった場合
            float pinchHP = m_characterStatus.MaxHP.Value * 0.1f;
            if (m_characterStatus.m_hp.Value <= pinchHP && m_inputType == InputTypes.Player)
            {
                if (m_pinchSE.status == CriAtomExPlayback.Status.Playing)
                {
                    m_pinchSE.Stop();
                }
                m_pinchSE = SoundManager.Instance.StartPlayback("se_Pinch_4");
            }
        }

        if (m_characterStatus.m_hp.Value > 0
            && m_isPushedOtherObj == true)
        {
            // ノックバック処理
            if (_hitCol)
            {
                switch (_knockBackType)
                {
                    case KnockBackType.forward:
                        m_knockBackVec = _hitCol.transform.forward;
                        m_knockBackVec.y = 0.0f;
                        m_knockBackVec.Normalize();
                        break;
                    case KnockBackType.radial:
                        m_knockBackVec = transform.position - _hitCol.transform.position;
                        m_knockBackVec.y = 0.0f;
                        m_knockBackVec.Normalize();
                        break;
                }

                if (_isStrongAttack)
                {
                    m_animator.SetTrigger("BlowAway");
                }
                else if (!m_isNoKnockBack)
                {
                    // 攻撃被弾時のHit方向を正確にするため敵の位置を覚えておく(山本)
                    m_attackEnemyTrans = _hitCol.transform.root.gameObject.transform;

                    if (m_characterStatus.m_knockBackDamage <= _dmgData.m_finalDamageVal)
                    {
                        if (_knockBackMultiplier > 0.0f)
                        {
                            m_animator.SetTrigger("KnockBack");
                            m_knockBackMultiplier = _knockBackMultiplier;

                            // 子オブジェクトまでたどるのはさすがに重いのでシーン制限 上甲 経営シーン払い戻し処理用
                            if (SceneNameManager.instance.CurrentSceneName == "ManagementScene")
                            {
                                var arbors = gameObject.GetComponentsInChildren<ArborFSM>();
                                foreach (var arv in arbors)
                                {
                                    arv.SendTrigger("Damage");
                                }
                            }
                        }
                    }

                    // 敵であれば索敵指示をだす
                    if (gameObject.TryGetComponent(out ArborFSM arborFSM))
                    {
                        arborFSM.SendTrigger("Damage");

                        if (EnemyParameters)
                        {
                            EnemyParameters.AttackedVec = -m_knockBackVec;
                        }
                    }
                }
            }

            // 返信用データ用意
            _dmgData.m_replyIsHit = true;
        }
    }
    // 毒のようなノックバックがいらないダメージ処理
    void IDamageable.Damaged(float _dmgData, Collider _hitCol)
    {
        //ノーダメージフラグ中に早期リターン（山本）
        if (m_isNoDamage) { return; }

        //死亡判定とスキル等による怯み判定がバッティングした際に強制的に死亡状態へと移行させる処理（山本）
        if (m_characterStatus.m_hp.Value <= 0.0f)
        {
            //死亡トリガーを逃れてしまったら強制的に死亡状態へと移行する（山本）
            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            {
                m_characterStatus.m_hp.Value = 0.0f;
                m_animator.SetTrigger("IsDead");
            }
            return;
        }

        if (m_characterStatus.m_hp.Value > 0)
        {
            // ダメージ処理
            Transform managerObj = transform.root.Find("ConditionManager");
            //if (_dmgData < 1.0f) _dmgData = 1.0f;
            m_characterStatus.m_hp.Value -= _dmgData;

            ////!@todo :ヒットエフェクトが出来たらここは消す
            //if (_hitCol)
            //{
            //    SoundManager.Instance.Start3DPlayback("HitSoundBlow", _hitCol.transform.position);
            //}
            // ヒット時演出
            HitFadeBlink(Color.red);
        }

        // 0以下で、倒れる
        if (m_characterStatus.m_hp.Value <= 0.0f)
        {
            m_characterStatus.m_hp.Value = 0.0f;
            m_animator.SetBool("IsDead", true);
            return;
        }
    }

    // HPが一定以下になったらHPを減らさない処理
    void IDamageable.Damaged(DamageNotification _dmgData, Collider _hitCol, float _limitHP)
    {
        //ノーダメージフラグ中に早期リターン（山本）
        if (m_isNoDamage) { return; }


        if (m_characterStatus.m_hp.Value > _limitHP)
        {
            // ダメージ処理
            _dmgData.m_finalDamageVal = _dmgData.m_attackData.Attack;

            m_characterStatus.m_hp.Value -= _dmgData.m_finalDamageVal;

            if (m_characterStatus.m_hp.Value <= _limitHP)
            {
                // 指定HP以下になったら1に強制する
                m_characterStatus.m_hp.Value = 1.0f;
            }

        }
        else
        {
            // 指定HP以下になったら1に強制する
            m_characterStatus.m_hp.Value = 1.0f;
        }

        if (_hitCol)
        {
            // ヒット時演出
            HitFadeBlink(Color.red);
        }


        // 返信用データ用意
        _dmgData.m_replyIsHit = true;

    }



    async private void HitStop(DamageNotification _dmgData)
    {
        if (m_animator == null) { return; }

        m_animator.speed = 0.0f;

        // ヒットストップを、指定の時間実行
        await UniTask.Delay((int)(_dmgData.m_hitStopTime * 1000));  //ミリ秒単位のため、1000倍

        m_animator.speed = 1.0f;
    }

    // カラー乗算によるダメージ演出再生
    void HitFadeBlink(Color color)
    {
        m_seq.Kill();
        m_seq = DOTween.Sequence();
        m_seq.Append(DOTween.To(() => Color.white, c => m_material.SetColor(PROPERTY_COLOR, c), color, 0.1f));
        m_seq.Append(DOTween.To(() => color, c => m_material.SetColor(PROPERTY_COLOR, c), Color.white, 0.1f));
        m_seq.Play();

    }




    public void Move(float _targetSpeed)
    {
        switch (m_moveType)
        {
            // 緩やか
            case MoveTypes.Loose:
                {
                    m_charaCtrl.MoveSpeed += (_targetSpeed - m_charaCtrl.MoveSpeed) * 0.5f;
                    m_animator.SetFloat("Speed", m_charaCtrl.MoveSpeed / Status.DushSpeed, 0.15f, Time.fixedDeltaTime);
                    break;
                }

            // びたびた
            case MoveTypes.Sharp:
                {
                    m_charaCtrl.MoveSpeed = _targetSpeed;
                    m_animator.SetFloat("Speed", m_charaCtrl.MoveSpeed / Status.DushSpeed, 0.15f, Time.fixedDeltaTime);
                    break;
                }
        }
    }

    // セットしたフレームのみこの値は反映される
    public void SetMoveVec(Vector3 _moveVec)
    {
        m_charaCtrl.MoveVec = _moveVec;
    }

    public void SetRotateToTarget(Vector3 _nextVec, bool _isMomentaryRot)
    {
        _nextVec.y = 0;

        m_charaCtrl.LookVector = _nextVec;
        m_charaCtrl.MomentaryRot = _isMomentaryRot;
    }

    public void Heal(float _healVal)
    {
        var hp = Status.m_hp.Value;

        //最大値以上回復するなら最大値にする
        if ((hp + _healVal) > Status.MaxHP.Value)
        {
            Status.m_hp.Value = Status.MaxHP.Value;
        }
        else
        {
            Status.m_hp.Value += _healVal;
        }

        Addressables.InstantiateAsync("HealEffect", transform);
    }

    //最初はBPをMaxにする為の処理
    public void SetInitializeStorySkillBP()
    {
        //格スキルIDから使用BPを取得する
        if (PlayerParameters)
        {
            PlayerStatus status = PlayerParameters.PlayerStatus;

            var bp1 = StorySkillDataBaseManager.instance.GetStorySkillData(PlayerParameters.StorySkill1_ID.Value);
            status.m_bpSkill_1.Value = bp1.PayBP;

            var bp2 = StorySkillDataBaseManager.instance.GetStorySkillData(PlayerParameters.StorySkill2_ID.Value);
            status.m_bpSkill_2.Value = bp2.PayBP;
        }
    }


    public void SetTabButtonIcon(StorySkill_ID iconName)
    {
        PlayerParameters.StorySkill1_ID.Value = iconName;
    }





    [System.Serializable]
    public class ActionState_Base : AnimatorStateMachine.ActionStateBase
    {
        [SerializeField] private bool m_isRootMotion = false;

        protected CharacterCore Core { get; private set; }
        public override void Initialize(AnimatorStateMachine stateMachine)
        {
            base.Initialize(stateMachine);

            Core = stateMachine.transform.parent.GetComponent<CharacterCore>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

        }

        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_charaCtrl.MoveSpeed = 0.0f;
            Core.m_charaCtrl.IsRootMotion = m_isRootMotion;
        }
    }
}


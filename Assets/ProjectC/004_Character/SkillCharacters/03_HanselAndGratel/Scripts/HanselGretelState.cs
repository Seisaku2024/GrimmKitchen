using SaintsField;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Playables;

//　ヘンゼルとグレーテルスキルのステート（山本）

public partial class CharacterCore : MonoBehaviour, IDamageable
{
    // 攻撃対象になったエネミーを記録するリスト
    private List<CharacterCore> m_targetEnemyList = new List<CharacterCore>();
    public List<CharacterCore> TargetEnemyList { get { return m_targetEnemyList; } set { m_targetEnemyList = value; } }

    // 釜のオブジェクト
    private GameObject m_cauldran = null;

    public enum HanselGretel
    {
        Hansel,
        Gretel,
    }



    // 登場時
    [System.Serializable]
    [AddTypeMenu("Player/SkillHansel/Falling")]
    public class ActionState_SkillHansel_Falling : ActionState_Base
    {



        [Header("ヘンゼルかどうか（チェック入れるとヘンゼル）")]
        [SerializeField] private HanselGretel m_type = HanselGretel.Hansel;


        private CaudronController m_controller = null;

        private Transform m_setTrans = null;
        public override void OnEnter()
        {
            base.OnEnter();

            Core.TargetEnemyList.Clear();

            if (Core.PlayerSkillsParameters)
            {
                Core.m_cauldran = Core.PlayerSkillsParameters.CauldronObj;

                if (Core.m_cauldran.TryGetComponent<CaudronController>(out var caudronController))
                {
                    m_controller = caudronController;
                }

            }

            if (m_type == HanselGretel.Hansel)
            {
                if (Core.transform.root.TryGetComponent(out ShareNodes shareNodes))
                {
                    m_setTrans = shareNodes.Nodes["HanselSitPosition"];
                }
            }
            else
            {
                if (Core.transform.root.TryGetComponent(out ShareNodes shareNodes))
                {
                    m_setTrans = shareNodes.Nodes["GretelSitPosition"];
                }
            }

            Core.CharaCtrl.transform.localScale = Vector3.one;

        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            Core.CharaCtrl.SetPositionMotor(m_setTrans.position);

        }


        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_controller.OnGroundFlg)
            {
                Core.m_animator.SetTrigger("Preparation");
            }
        }



    }


    // 敵を釜に吸収
    [System.Serializable]
    [AddTypeMenu("Player/SkillHansel/AbsorbEnemy")]
    public class ActionState_SkillHansel_AbsorbEnemy : ActionState_Base
    {
        [Header("攻撃の範囲")]
        [SerializeField]
        private float m_range = 10.0f;

        [Header("ヘンゼルかどうか（チェック入れるとヘンゼル）")]
        [SerializeField] private HanselGretel m_type = HanselGretel.Hansel;

        private List<CharacterCore> m_targetAddList = new List<CharacterCore>();

        public override void OnEnter()
        {
            base.OnEnter();

            Core.TargetEnemyList.Clear();

            List<CharacterCore> targetList = new List<CharacterCore>();


            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.GroupNo == CharacterGroupNumber.player)
                {

                    // すでにヘンゼルグレーテルスキルが出現していたら消す
                    if (chara.PlayerParameters.ObserbSkill1 != null)
                    {
                        if (chara.PlayerParameters.ObserbSkill1 != Core.transform.root.gameObject
                            && chara.PlayerParameters.StorySkill1_ID.Value == StorySkill_ID.HanselGretel)
                        {

                            Core.m_animator.SetTrigger("Vanish");
                            return;

                        }
                    }
                    else if (chara.PlayerParameters.ObserbSkill2 != null)
                    {
                        if (chara.PlayerParameters.ObserbSkill2 != Core.transform.root.gameObject
                            && chara.PlayerParameters.StorySkill2_ID.Value == StorySkill_ID.HanselGretel)
                        {

                            Core.m_animator.SetTrigger("Vanish");
                            return;

                        }
                    }
                }
            }


            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {


                if (chara.GroupNo == CharacterGroupNumber.enemy)
                {
                    var vec = chara.transform.position - Core.m_cauldran.transform.position;
                    var dist = vec.magnitude;

                    if (dist <= m_range)
                    {
                        if (chara.HasParameter("IsHanselSkill") == true &&
                            chara.HasParameter("Absorb") == true)
                        {
                            // HP０以下になっていたらリストに入れない
                            if (chara.Status.m_hp.Value <= 0.0f)
                            {
                                continue;
                            }

                            // すでにヘンゼルとグレーテルのスキルを受けている敵は対象外
                            if (chara.m_animator.GetBool("IsHanselSkill") == true)
                            {
                                continue;
                            }

                            chara.m_animator.SetBool("IsHanselSkill", true);
                            chara.m_animator.SetTrigger("Absorb");

                            targetList.Add(chara);

                            if (Core.m_cauldran.TryGetComponent(out HanselGretelAddTargetList hanselGretelAddTargetList))
                            {
                                hanselGretelAddTargetList.TargetAddEnemyList.Add(chara);
                            }


                        }


                    }

                }

            }

            if (Core.m_cauldran.TryGetComponent(out HanselGretelAddTargetList hanselGretelTargetList))
            {
                // 吸収できる敵いなければ撤退
                if (hanselGretelTargetList.TargetAddEnemyList.Count == 0)
                {
                    Core.m_animator.SetTrigger("Vanish");
                    return;
                }

                m_targetAddList = hanselGretelTargetList.TargetAddEnemyList;
                Core.TargetEnemyList = m_targetAddList;

            }

            Transform trans = null;

            if (m_type == HanselGretel.Hansel)
            {
                if (Core.transform.root.TryGetComponent(out ShareNodes shareNodes))
                {
                    trans = shareNodes.Nodes["HanselSitPosition"];
                }
            }
            else
            {
                if (Core.transform.root.TryGetComponent(out ShareNodes shareNodes))
                {
                    trans = shareNodes.Nodes["GretelSitPosition"];
                }
            }


            // キャラをそれぞれの場所へセット
            Core.CharaCtrl.SetPositionMotor(trans.position);

            Core.CharaCtrl.transform.localScale = Vector3.one;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            bool completeFlg = false;

            foreach (var core in m_targetAddList)
            {

                if (core == null)
                {
                    continue;
                }

                // HP０以下になっていたらリストから除外
                if (core.Status.m_hp.Value <= 0.0f)
                {
                    m_targetAddList.Remove(core);
                    continue;
                }

                if (core.m_finishAbsorbFlg == true)
                {
                    completeFlg = true;
                }
                else
                {
                    completeFlg = false;
                    break;
                }

            }


            if (completeFlg == false)
            {
                return;
            }

            // ターゲットリストの敵のステートを次に変更
            foreach (var core in m_targetAddList)
            {
                core.m_animator.SetTrigger("Mixed");
            }

            // 自身のステートも変更
            Core.m_animator.SetTrigger("MixEnemy");
        }


        public override void OnExit()
        {
            if (Core.PlayerSkillsParameters)
            {
                var cauldran = Core.PlayerSkillsParameters.CauldronObj;

                if (cauldran.TryGetComponent<CaudronController>(out var caudronController))
                {
                    caudronController.Ignition();
                }

            }
        }


    }

    // 敵をかき混ぜる
    [System.Serializable]
    [AddTypeMenu("Player/SkillHansel/MixEnemy")]
    public class ActionState_SkillHansel_MixEnemy : ActionState_Base
    {

        private float m_mixTime = 0.0f;
        private Vector3 m_targetVec = Vector3.zero;


        public override void OnEnter()
        {
            base.OnEnter();

            // かき混ぜ時間（童話スキルの滞在時間）
            m_mixTime = StorySkillDataBaseManager.instance.
                GetStorySkillData(StorySkill_ID.HanselGretel).StayTime;


            if (Core.transform.root.TryGetComponent(out ShareNodes shareNodes))
            {
                var targetposition = shareNodes.Nodes["SoupEndPoint"].position;
                m_targetVec = targetposition - Core.transform.position;
                m_targetVec.y = 0;
                m_targetVec.Normalize();

            }

        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            Core.transform.localScale = Vector3.one;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            Core.SetRotateToTarget(m_targetVec, false);

            if (m_mixTime <= 0.0f)
            {

                // ターゲットリストの敵のステートを次に変更
                foreach (var core in Core.m_targetEnemyList)
                {
                    core.m_animator.SetTrigger("FlyAway");
                }

                Core.m_animator.SetTrigger("Vanish");

            }
            else
            {
                m_mixTime -= Time.deltaTime;
            }

            Core.transform.localScale = Vector3.one;

        }


        public override void OnExit()
        {
            base.OnExit();

            if (Core.PlayerSkillsParameters)
            {
                var cauldran = Core.PlayerSkillsParameters.CauldronObj;

                if (cauldran.TryGetComponent<CaudronController>(out var caudronController))
                {
                    caudronController.StartLargeEruption();

                    // 噴火音再生
                    SoundManager.Instance.Start3DPlayback("Eruption",Core.transform.gameObject,0.5f);
                }

            }

        }


    }

    // 消滅
    [System.Serializable]
    [AddTypeMenu("Player/SkillHansel/Vanish")]
    public class ActionState_SkillHansel_Vanish : ActionState_Base
    {
        [Header("消失するまでの時間")]
        [SerializeField]
        private float m_vanishTime = 3.0f;
        private bool m_onceFlg = false;
        private Transform m_characterTrans = null;
        


        public override void OnEnter()
        {
            base.OnEnter();


            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (core.GroupNo == CharacterGroupNumber.player)
                {
                    m_characterTrans = core.transform;
                }
            }


        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_characterTrans != null)
            {
                var targetVec = m_characterTrans.position - Core.transform.position;
                targetVec.y = 0.0f;
                targetVec.Normalize();

                Core.SetRotateToTarget(targetVec, false);

            }


            if (m_vanishTime <= 0.0f && m_onceFlg == false)
            {
                Core.PlayerSkillsParameters?.StorySkillDisappear();

                var cauldran = Core.PlayerSkillsParameters.CauldronObj;

                if (cauldran.TryGetComponent<CaudronController>(out var caudronController))
                {
                    // 釜の消化
                    caudronController.Digestion();
                }

                m_onceFlg = true;

            }
            else
            {
                m_vanishTime -= Time.deltaTime;
            }

            Core.transform.localScale = Vector3.one;

        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Core.transform.localScale = Vector3.one;
        }

    }
}


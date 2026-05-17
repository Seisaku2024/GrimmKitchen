using Arbor;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine.UIElements;
using UnityEngine.VFX;

// キャラクターの基盤クラス
public partial class CharacterCore : MonoBehaviour, IDamageable
{
    //===================================
    // 敵側ステート
    //===================================
    // 各基本State（伊波）

    //タックル攻撃用のオブジェクトを指定（山本）
    [Header("タックル攻撃用のエフェクト")]
    [SerializeField] private GameObject m_rushAttackEffect = null;

    // 吸収移動完了フラグ(ヘンゼルとグレーテルスキル用)
    private bool m_finishAbsorbFlg = false;
    public bool FinishAbsorb => m_finishAbsorbFlg;

    // ヘンゼルとグレーテスキルにアクセスするための変数
    private GameObject m_HanselGretelSkillObj = null;


    [System.Serializable]
    [AddTypeMenu("Enemy/ReadyAttack")]
    public class ActionState_EnemyReadyAttack : ActionState_Base
    {
        [SerializeField] private bool m_isNotKnockBack;
        [SerializeField] private bool m_isShowDushEffect;
        public bool isTurnToward = true;


        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_isNoKnockBack = m_isNotKnockBack;

            //突進選択時、ダッシュエフェクトを発生させる（山本）
            if (m_isShowDushEffect)
            {
                if (Core.m_dashEffect)
                {
                    //ダッシュエフェクトを再生
                    Core.m_dashEffect?.Play();
                }

                if (Core.m_rushAttackEffect)
                {
                    //ラッシュエフェクトをアクティブ化(山本)
                    Core.m_rushAttackEffect.SetActive(true);
                    //数値を0.0fに
                    Core.m_rushAttackEffect.GetComponent<VisualEffect>()?.SetFloat("UVAlphaClippingValue", 0.0f);
                }

            }

            Core.m_inputProvider.AttackType = 0;
            Core.m_animator.SetInteger("DoAttackType", 0);
        }

        public override void OnFixedUpdate()
        {
            if (isTurnToward) Core.SetRotateToTarget(Core.m_inputProvider.Destination - Core.transform.position, false);
        }
    }


    [System.Serializable]
    [AddTypeMenu("Enemy/UseItem")]
    public class ActionState_EnemyUseItem : ActionState_Base
    {
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Core.Move(0.0f);
        }
    }

    [System.Serializable]
    [AddTypeMenu("Enemy/Rush")]
    public class ActionState_EnemyRush : ActionState_Base
    {
        [Header("突進時間")]
        [SerializeField] private float m_rushTime;
        [Header("追尾時間")]
        [SerializeField] private float m_maxHomingTime;
        [Header("追尾の強さ"), Range(0.0f, 1.0f)]
        [SerializeField] private float m_rateHoming;

        [Header("CharactorCoreのdushSpeed基準にしたspeed補正値")]
        [SerializeField] private float m_magnificationSpeed = 1f;
        private float m_remainingTime;
        private float m_homingTime;
        private Collider m_collider;

        ////ラッシュエフェクト関係（山本）
        //[Header("ラッシュエフェクトの消失スピード")]
        //[SerializeField] private float m_rushEffectAppearSpeed = 0.1f;
        //ラッシュエフェクトの現在の進捗状況
        private float m_rushEffectApearProgress = 0.0f;


        public override void OnEnter()
        {
            base.OnEnter();
            m_remainingTime = m_rushTime;
            m_homingTime = m_maxHomingTime;
            m_rushEffectApearProgress = 0.0f;

            Core.m_isNoKnockBack = true;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            m_remainingTime -= Time.deltaTime;
            m_homingTime -= Time.deltaTime;

            //ラッシュエフェクトの進捗度に合わせて表示（山本）
            if (Core.m_rushAttackEffect)
            {
                //ダッシュ時間に合わせて進捗度が0=>1=>0になるように調整
                m_rushEffectApearProgress = (float)Math.Sin((1.0f - (m_remainingTime / m_rushTime)) * Math.PI);
                //進捗度をセット
                Core.m_rushAttackEffect.GetComponent<VisualEffect>().SetFloat("UVAlphaClippingValue", m_rushEffectApearProgress);
            }


            if (m_remainingTime <= 0f)
            {
                Core.m_animator.SetTrigger("EndAttack");

                if (Core.m_dashEffect)
                {
                    //ダッシュエフェクトを止める（山本）
                    Core.m_dashEffect?.Stop();
                }

                if (Core.m_rushAttackEffect)
                {
                    //ラッシュ攻撃エフェクトを非アクティブにする
                    Core.m_rushAttackEffect.SetActive(false);
                }
            }
        }


        public override void OnFixedUpdate()
        {
            if (m_homingTime > 0.0f)
            {
                Vector3 rotVec = Core.m_inputProvider.Destination - Core.transform.position;
                rotVec.y = 0.0f;
                rotVec.Normalize();
                rotVec = Core.transform.forward + rotVec * m_rateHoming;
                Core.SetRotateToTarget(rotVec, false);
            }
            Core.Move(Core.Status.DushSpeed * m_magnificationSpeed);
        }

        public override void OnExit()
        {
            base.OnExit();

            //ダッシュエフェクトを止める（山本）
            if (Core.m_dashEffect)
            {
                Core.m_dashEffect.Stop();
            }


            if (Core.m_rushAttackEffect)
            {
                //ラッシュ攻撃エフェクトを非アクティブにする
                Core.m_rushAttackEffect.SetActive(false);
            }
        }
    }

    [System.Serializable]
    [AddTypeMenu("Enemy/ReadyThrowRock")]
    public class ActionState_ReadyThrowRock : ActionState_Base
    {
        [Header("追尾開始タイミング"), Range(0.0f, 1.0f)]
        [SerializeField] private float m_startHomingProgress;
        [Header("追尾の強さ"), Range(0.0f, 1.0f)]
        [SerializeField] private float m_rateHoming;

        Transform m_target;

        public override void OnEnter()
        {
            base.OnEnter();
            Core.m_isNoKnockBack = true;

            if (Core.transform.TryGetComponent(out ArborFSM arbor))
            {
                m_target = arbor.parameterContainer.GetTransform("Target");
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (Core.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > m_startHomingProgress)
            {
                if (!m_target) return;
                Vector3 rotVec = m_target.transform.position - Core.transform.position;
                rotVec.y = 0.0f;
                rotVec.Normalize();
                rotVec = Core.transform.forward + rotVec * m_rateHoming;
                Core.SetRotateToTarget(rotVec, false);
            }
            Core.Move(0.0f);
        }
    }


    [Serializable]
    [AddTypeMenu("Enemy/Discover")]
    public class ActionState_EnemyDiscover : ActionState_Base
    {
        public bool isTurnToward = true;
        // 以下の処理はEnemyIconElementに移行（伊波）
        //[SerializeField] private float m_iconShowTime = 2f;

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Core.Move(0.0f);
            if (Core.EnemyParameters?.Target && isTurnToward)
            {
                Core.SetRotateToTarget(Core.EnemyParameters.Target.position - Core.transform.position, false);
            }
        }


        //public override async void OnEnter()
        //{
        //    base.OnEnter();
        //    Transform canvas = Core.transform.Find("EnemyCanvas");
        //    if (!canvas) return;
        //    if (!canvas.TryGetComponent(out EnemyIconController iconController)) return;
        //    iconController.ShowIcon(EnemyIconController.EnemyIconType.DiscoverIcon);
        //    await UniTask.Delay(TimeSpan.FromSeconds(m_iconShowTime));
        //    if (iconController)
        //    {
        //        iconController.HideIcon(EnemyIconController.EnemyIconType.DiscoverIcon);
        //    }
        //}
    }

    [Serializable]
    [AddTypeMenu("Enemy/LostSight")]
    public class ActionState_EnemyLostSight : ActionState_Base
    {
        // 以下の処理はEnemyIconElementに移行（伊波）
        //[SerializeField] private float m_iconShowTime = 5f;
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Core.Move(0.0f);
        }

        //public override async void OnEnter()
        //{
        //    base.OnEnter();
        //    Transform canvas = Core.transform.Find("EnemyCanvas");
        //    if (!canvas) return;
        //    if (!canvas.TryGetComponent(out EnemyIconController iconController)) return;
        //    iconController.ShowIcon(EnemyIconController.EnemyIconType.LostSightIcon);
        //    await UniTask.Delay(TimeSpan.FromSeconds(m_iconShowTime));
        //    if (iconController)
        //    {
        //        iconController.HideIcon(EnemyIconController.EnemyIconType.LostSightIcon);
        //    }
        //}
    }


    [Serializable]
    [AddTypeMenu("Enemy/Absorb")]
    // 童話スキル（ヘンゼルとグレーテル）で敵が鍋に吸い込まれる際の挙動（山本）
    //（ベジェ曲線使用）
    public class ActionState_Absorb : ActionState_Base
    {
        // ベジェ曲線-------------------------------------------------
        [Header("ベジェ曲線の制御点の高さ")]
        [SerializeField]
        private float m_height = 3.0f;
        [Header("制御点を2直線のどの割合に位置するか")]
        [SerializeField, Range(0.0f, 1.0f)]
        private float m_persentage = 0.7f;
        [Header("ベジェ曲線で指定地点まで移動する時間")]
        [SerializeField]
        private float m_duration = 2.0f;
        [Header("ベジェ曲線の終点をずらすためのランダム値")]
        [SerializeField]
        private float m_randomNum = 0.3f;

        //ベジェ曲線の始点
        private Vector3 m_startPoint;
        //ベジェ曲線の制御点
        private Vector3 m_controlPoint;
        //ベジェ曲線の始点と終点のベクトル
        private Vector3 m_targetVec;
        //ベジェ曲線の終点
        private Vector3 m_endPosition;
        //------------------------------------------------------------

        // 釜--------------------------------------------------------
        [Header("落下するまでの時間")]
        [SerializeField]
        private float m_fallTime = 1.0f;
        [Header("落下後最終スケール値")]
        [SerializeField]
        private float m_targetScale = 0.5f;

        // スケール変更前の保存用変数
        private float m_preChangeScale = 1.0f;
        // 落下目標地点
        private Vector3 m_targetSoupPosition = Vector3.zero;

        //-------------------------------------------------------------

        private Tween m_tween = null;
        private Tween m_completeTween = null;

        public override void OnEnter()
        {
            base.OnEnter();

            Core.m_finishAbsorbFlg = false;

            // 初期化
            if (Core.m_HanselGretelSkillObj != null)
                Core.m_HanselGretelSkillObj = null;

            // プレイヤーのヘンゼルとグレーテルスキルからベジェ曲線の終点と最終移動点を取得
            foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (chara.m_groupNo == CharacterGroupNumber.player)
                {
                    // ヘンゼルとグレーテルスキルがないとこのStateにはならないはずだが一応確認

                    if (chara.PlayerParameters.StorySkill1_ID.Value == StorySkill_ID.HanselGretel
                        && chara.PlayerParameters.ObserbSkill1)
                    {
                        Core.m_HanselGretelSkillObj = chara.PlayerParameters.ObserbSkill1;
                    }
                    else if (chara.PlayerParameters.StorySkill2_ID.Value == StorySkill_ID.HanselGretel
                        && chara.PlayerParameters.ObserbSkill2)
                    {
                        Core.m_HanselGretelSkillObj = chara.PlayerParameters.ObserbSkill2;
                    }

                    if (Core.m_HanselGretelSkillObj == null) break;

                    // スキルからベジェ曲線の終点と到達地点を取得
                    if (Core.m_HanselGretelSkillObj.TryGetComponent(out ShareNodes shareNode))
                    {
                        m_endPosition = shareNode.Nodes["BezierEndPoint"].position;
                        m_targetSoupPosition = shareNode.Nodes["SoupEndPoint"].position;
                    }

                    break;

                }

            }


            // ベジェ曲線関係-------------------------------------------------------------
            m_startPoint = Core.transform.position;
            // ベジェ曲線の終点にランダム値を加える
            m_endPosition = m_endPosition
                + new Vector3(UnityEngine.Random.Range(-m_randomNum, m_randomNum),
                              UnityEngine.Random.Range(-m_randomNum, m_randomNum),
                              UnityEngine.Random.Range(-m_randomNum, m_randomNum));

            // 交点を求める
            Vector3 point = Vector3.Lerp(m_startPoint, m_endPosition, m_persentage);
            m_controlPoint = point + new Vector3(0, m_height, 0);
            //------------------------------------------------------------------------------

            // パスファインディングを動かないようにする
            if (Core.EnemyParameters.PathFinding)
            {
                Core.EnemyParameters.PathFinding.enabled = false;
            }

            // Arbor停止
            if (Core.gameObject.TryGetComponent(out ArborFSM arbor))
            {
                arbor.Pause();
            }

            // 重力働かないようにする
            Core.CharaCtrl.Gravity.y = 0.0f;

            // レイヤーを一時的に変更
            Core.gameObject.layer = LayerMask.NameToLayer("AbsorbEnemy");

            if (Core.TryGetComponent(out MyCharacterController myCharacterController))
            {
                myCharacterController.AddNoHitTag("Player");
            }

            // スタン、麻痺状態でも解除してスキルが通じるようにする

            if (Core.HasParameter("IsStun"))
            {
                Core.m_animator.SetBool("IsStun", false);
            }

            if (Core.HasParameter("IsParalysis"))
            {
                Core.m_animator.SetBool("IsParalysis", false);
            }


            // Dotweenを使用してベジェ曲線でエネミーを移動させる
            //鍋の上へと移動
            m_tween = DOVirtual.Float(0.0f, 1.0f, m_duration,
                 value =>
                 {
                     // ベジェ曲線の計算
                     Vector3 position = CalculateBezierPoint
                     (value,
                     m_startPoint,
                     m_controlPoint,
                     m_endPosition);

                     Core.CharaCtrl.SetPositionMotor(position);

                 }
                 ).OnComplete(FallUpdate).SetEase(Ease.InQuad);//鍋の上に移動した後に鍋の中へと移動


            // Core.m_animator.gameObject.layer= LayerMask.NameToLayer("AbsorbEnemy");

        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

        }

        public override void OnExit()
        {
            base.OnExit();
            Core.m_finishAbsorbFlg = false;
            m_tween.Kill();
            m_tween = null;
            m_completeTween.Kill();
            m_completeTween = null;

        }

        // ベジェ曲線の計算式
        Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            Vector3 point = (uu * p0) + (2 * u * t * p1) + (tt * p2);
            return point;
        }

        private void FallUpdate()
        {
            m_targetSoupPosition = new Vector3(m_endPosition.x, m_targetSoupPosition.y, m_endPosition.z);

            //下へ落下とスケール値調整
            m_completeTween = DOVirtual.Vector3(m_endPosition, m_targetSoupPosition, m_fallTime,
                 value =>
                 {

                     Core.CharaCtrl.SetPositionMotor(value);


                 }).
                 OnComplete
                 (
                 () =>
                 {
                     Core.m_finishAbsorbFlg = true;
                 }
                 )
                 .SetEase(Ease.InQuad);

            Core.m_animator.transform.DOScale(m_targetScale, m_fallTime).SetEase(Ease.InBounce);

        }




    }

    // ヘンゼルとグレーテルスキルで釜でかき混ぜられる処理（山本）
    [Serializable]
    [AddTypeMenu("Enemy/Mixed")]
    public class ActionState_MixedEnemy : ActionState_Base
    {

        [Header("回転速度")]
        [SerializeField] private float m_speed = 5.0f;

        // スープの落下点
        private Vector3 m_centerPosition = new Vector3();
        // 落下点（スープの中心）から現在のエネミーの距離
        private float m_dist = 0.0f;
        // 落下点と現在の敵座標の角度
        private float m_nowAngle = 0.0f;


        public override void OnEnter()
        {
            base.OnEnter();

            if (this == null) return;


            if (Core.m_HanselGretelSkillObj.TryGetComponent(out ShareNodes shareNodes))
            {
                m_centerPosition = shareNodes.Nodes["SoupEndPoint"].position;
            }

            m_dist = (Core.transform.position - m_centerPosition).magnitude;

            m_nowAngle = Vector3.SignedAngle(m_centerPosition, Core.transform.position, Vector3.up);

        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            m_nowAngle += Time.fixedDeltaTime * m_speed;

            if (m_nowAngle >= 360.0f)
            {
                m_nowAngle -= 360.0f;
            }

            var position = new Vector3(m_centerPosition.x + Mathf.Cos(m_nowAngle) * m_dist, Core.transform.position.y, m_centerPosition.z + Mathf.Sin(m_nowAngle) * m_dist);

            Core.CharaCtrl.SetPositionMotor(position);


        }

    }

    // ヘンゼルとグレーテルスキルで釜から吹き飛ばされる処理（山本）
    [Serializable]
    [AddTypeMenu("Enemy/FlyAway")]
    public class ActionState_FlyAway : ActionState_Base
    {
        [Header("Jump力")]
        [SerializeField]
        private float m_jumpPow = 10.0f;
        [Header("吹き飛ばす力")]
        [SerializeField]
        private float m_pow = 3.0f;
        [Header("元のスケール値")]
        [SerializeField]
        private float m_originScale = 1.0f;
        [Header("スケール値変更までの時間")]
        [SerializeField]
        private float m_changeTime = 1.0f;

        private bool m_changeFinishScaleFlg = false;

        public override void OnEnter()
        {
            base.OnEnter();

            m_changeFinishScaleFlg = false;

            Vector3 forwardDirection = Core.transform.forward;
            float randomYaw = UnityEngine.Random.Range(0, 360.0f);
            // ランダム回転のQuaternionを生成
            Quaternion randomRotation = Quaternion.Euler(0f, randomYaw, 0f);
            // 回転後のベクトルを計算
            Vector3 rotatedDirection = randomRotation * forwardDirection;
            rotatedDirection.Normalize();

            Core.CharaCtrl.Jump(m_jumpPow);
            Core.m_charaCtrl.AddVelocity(rotatedDirection * m_pow * -Core.m_knockBackMultiplier);
            Core.SetRotateToTarget(rotatedDirection, true);

            // 重力戻す
            Core.CharaCtrl.Gravity.y = -30.0f;

            // スケール値元に戻す
            Core.m_animator.transform.DOScale(m_originScale, m_changeTime)
                .OnComplete
                (() =>
                {
                    m_changeFinishScaleFlg = true;
                }
                )
                .SetEase(Ease.InBounce);

        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();


            // 地面に着地した際の処理
            if (Core.m_charaCtrl.Motor.GroundingStatus.IsStableOnGround &&
                m_changeFinishScaleFlg)
            {
                // 強制的に1ダメージ
                Core.Status.m_hp.Value -= 1.0f;

                if (Core.Status.m_hp.Value <= 0.0f)
                {
                    Core.Status.m_hp.Value = 0.0f;
                    Core.m_animator.SetTrigger("IsDead");
                    return;
                }

                Core.m_animator.SetTrigger("FinishFlyAway");
            }

        }

        public override void OnExit()
        {
            base.OnExit();

            // パスファインディングを動くようにする
            if (Core.EnemyParameters.PathFinding)
            {
                Core.EnemyParameters.PathFinding.enabled = true;

                // 目的地を自身の現在座標にセット
                Core.EnemyParameters.PathFinding.Stop();
            }

            // エージェントのパスをリセット
            if (Core.transform.TryGetComponent(out NavMeshAgent meshAgent))
            {
                meshAgent.ResetPath();
            }

            // Arbor再開
            if (Core.gameObject.TryGetComponent(out ArborFSM arbor))
            {
                arbor.Resume();
            }

            // レイヤーを戻す
            Core.gameObject.layer = LayerMask.NameToLayer("Enemy");

            if (Core.TryGetComponent(out MyCharacterController myCharacterController))
            {
                myCharacterController.RemoveNoHItTagList("Player");
            }


            if (Core.HasParameter("IsHanselSkill") == true)
            {
                Core.m_animator.SetBool("IsHanselSkill", false);
            }



        }

    }



}
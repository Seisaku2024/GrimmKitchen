using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChallengeInfo;
using UniRx;
using DG.Tweening;

public class ChallengeGage : MonoBehaviour
{
    // 制作者 田内　（修正 吉田）
    // 満足度用UI



    [Header("ゲージのコントローラー")]
    [SerializeField]
    private HPBarController m_barController = null;


    [Header("満足値の位置を示すためのコントローラー")]
    [SerializeField]
    private BarSignController m_barSignController = null;


    //===================================================
    // Target（大会のクリア値、イベントの発生値）のList

    [Header("イベントマークプレハブ")]
    [SerializeField]
    private FixedChallengeEventController m_targetControllerPrefab = null;

    private List<FixedChallengeEventController> m_fixedChallengeEventController = new List<FixedChallengeEventController>();

    //=======================
    // 実行中tween
    private Tween m_tween = null;

    //======================
    // 現在の割合(0～1)
    private float m_ratio = 0.0f;

    //==========================================
    //                実行処理
    //==========================================

    private void Start()
    {
        var data = ChallengeDataBaseManager.instance.GetData(ChallengeManager.instance.ChallengeID);

        // チャレンジが設定されていなければ削除
        if (data == null || data.ChallengeID == ChallengeID.None)
        {
            Destroy(gameObject);
            return;
        }

        // ゲージの最大値や初期値を設定
        InitializeGage();

        // TargetController作成
        CreateTargetController();

        // クリア条件が更新されるたびに更新
        MessageBroker.Default.Receive<BaseClearConditionChallengeData.GlobalChangeClearConditionChallenge>().Subscribe(_ =>
        {
            // 既に動いていれば仕切り直し
            if (m_tween != null)
            {
                m_tween.Kill();
                m_tween = null;
            }

            // チャレンジ進行割合を取得
            float currentRatio = ManagementGameDataManager.instance.GetCurrentClearConditionAdvanceRatio();

            // DOTweenでアニメーション
            m_tween = DOVirtual.Float(m_ratio, currentRatio, 0.8f,
                value =>//アニメーション中の更新処理
                {
                    m_ratio = value;

                    // 目標達成しているか確認
                    foreach (var target in m_fixedChallengeEventController)
                    {
                        target.CheckOver(m_ratio);
                    }

                    // ゲージの長さを変更
                    SetGageLength();

                    // 座標更新
                    SetSignPos();
                }).
                SetEase(Ease.OutCubic).
                SetLink(gameObject);
        }).AddTo(this);

    }

    private void OnDestroy()
    {
        if (m_tween != null)
        {
            m_tween.Kill();
            m_tween = null;
        }
    }


    // 位置を調整する　追加：吉田
    private void SetSignPos()
    {
        if (m_barSignController == null) return;

        m_barSignController.SetPos(m_ratio);
    }


    // ゲージの最大値や初期値を設定　追加：吉田
    public void InitializeGage()
    {
        #region nullチェック
        if (m_barController == null)
        {
            Debug.LogError("HPBarControllerがシリアライズされていません");
            return;
        }
        #endregion

        var data = ChallengeDataBaseManager.instance.GetData(ChallengeManager.instance.ChallengeID);
        if (data == null) return;

        // ゲージの最大値や初期値を設定
        // リスト変更の度に更新
        ManagementGameDataManager.instance.ClearConditionChallengeDataRPRC.ObserveCountChanged().Subscribe(_ =>
        {
            float max = (float)ManagementGameDataManager.instance.ClearConditionChallengeDataRPRC.Count;
            float current = ManagementGameDataManager.instance.GetCurrentClearConditionAdvanceValue();
            m_barController.SetHealth(current, max);
        }).AddTo(this);
    }


    private void CreateTargetController()
    {
        #region nullチェック
        if (m_targetControllerPrefab == null)
        {
            Debug.LogError("TargetControllerPrefabがシリアライズされていません");
            return;
        }
        if (m_barController == null)
        {
            Debug.LogError("BarControllerがシリアライズされていません");
            return;
        }
        #endregion

        var data = ChallengeDataBaseManager.instance.GetData(ChallengeManager.instance.ChallengeID);
        if (data == null) return;

        foreach (var eve in data.FixedChallengeEventList)
        {
            if (eve == null) continue;

            // 子オブジェクトに作成 初期座標をセット
            var fixedChallengeEventController = Instantiate(m_targetControllerPrefab, m_barController.gameObject.transform);
            fixedChallengeEventController.SetTargetFixedChallengeEvent(eve);

            void UpdateBarSignControllerData(FixedChallengeEventController _controller,ChallengeData.FixedChallengeEvent _data)
            {
                float max = (float)ManagementGameDataManager.instance.ClearConditionChallengeDataRPRC.Count;
                float current = max * _data.StartValue;
                _controller.SetBarSignControllerData(max, current);
            }


            // 更新
            UpdateBarSignControllerData(fixedChallengeEventController,eve);


            // リスト変更の度に座標を更新
            ManagementGameDataManager.instance.ClearConditionChallengeDataRPRC.ObserveCountChanged().Subscribe(_ =>
            {
                // 更新
                UpdateBarSignControllerData(fixedChallengeEventController, eve);

            }).AddTo(this);

            m_fixedChallengeEventController.Add(fixedChallengeEventController);
        }

    }



    // ゲージの長さを変更　追加：吉田
    private void SetGageLength()
    {
        if (m_barController == null) return;

        // 計算モードをオフにする
        m_barController.IsCalc = false;
        m_barController.SetHealthValue(m_ratio);
    }


}
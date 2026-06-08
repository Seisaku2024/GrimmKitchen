using Unity.Cinemachine;
using Cysharp.Threading.Tasks;
using Speaker;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class ConversationWindow : BaseWindow
{
    // 会話用のウィンドウ（山本）

    [Header("名前文")]
    [SerializeField]
    private TextMeshProUGUI m_speakerNameText = null;

    [Header("名前表示ウィンドウ")]
    [SerializeField]
    private GameObject m_speakerWindow = null;

    [Header("会話文")]
    [SerializeField]
    private TextMeshProUGUI m_conversationText = null;

    [Header("話者タイプ")]
    [SerializeField]
    private SpeakerType m_speakerType = SpeakerType.None;

    [Header("ストーリータイプ")]
    [SerializeField]
    private StoryType m_storyType = StoryType.None;

    [Header("進むボタン")]
    [SerializeField]
    protected InputActionButton m_nextInputActionButton = null;

    [Header("カメラのInputAction")]
    [SerializeField]
    private InputActionReference m_cameraInputActionReference = null;

    [Header("文章のエフェクト")]
    [SerializeField]
    private TypeWriteEffectText m_effectText = null;

    private ConversationData m_conversationData = null;
    private int m_listCount = 0;
    private List<ConversationInformaton> m_conversationList = new List<ConversationInformaton>();
    //会話カメラのLookAtのTransform
    private Transform m_conversationCameraLookAtTransform = null;
    //会話用のカメラのFollowのTransform
    private Transform m_conversationCameraFollowTransform = null;
    //スキップフラグ
    private bool m_bSkipFlg = false;

    // キャラクターとカメラとの距離
    private float m_cameraDist = 2f;

    // カメラを変更するかどうかフラグ
    private bool m_bChangeCamFlg = true;
    public bool ChangeCameFlg { get { return m_bChangeCamFlg; } set { m_bChangeCamFlg = value; } }

    public SpeakerType SpeakerType { get { return m_speakerType; } set { m_speakerType = value; } }
    public StoryType StoryType { get { return m_storyType; } set { m_storyType = value; } }
    public Transform SetConversationCameraLookAtTransform { set { m_conversationCameraLookAtTransform = value; } }
    public Transform SetConversationCameraFollowTransform { set { m_conversationCameraFollowTransform = value; } }

    private CharacterCore m_characterCore = null;

    private float m_originBlendTime = 0f;

    public override async UniTask OnInitialize()
    {
        #region nullチェック

        if (m_conversationText == null)
        {
            Debug.LogError("会話テキストがセットされてません");
            return;
        }

        if (m_speakerNameText == null)
        {
            Debug.LogError("話者テキストがセットされてません");
            return;
        }

        if (m_speakerWindow == null)
        {
            Debug.LogError("話者ウィンドウがセットされてません");
            return;
        }


        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            m_conversationData = ConversationDataBaseManager.instance.GetConvaersationData(m_storyType);

            var fase = m_conversationData.TalkingFase;

            m_conversationList.Clear();

            m_conversationList = m_conversationData.GetLocalizedStringList2(fase);
            m_listCount = 0;

            // 話者のウィンドウ表示するかどうか
            if (m_conversationData.IsSpeakerNameWindow == true)
            {
                if (m_conversationData.SpeakerName != null)
                {
                    var NameText = await m_conversationList[m_listCount].SpeakerName.GetLocalizedStringAsync();
                    m_speakerNameText.text = NameText;
                }
            }
            else
            {
                m_speakerWindow.SetActive(false);
            }

            // 会話更新
            var localizedText = await m_conversationList[m_listCount].LocalizedString.GetLocalizedStringAsync();
            m_conversationText.text = localizedText;

            if (m_effectText)
            {
                _ = m_effectText.PlayEffect();
            }



            // カメラターゲットを検索（メタリストから探索）
            foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
            {
                if (m_conversationList[m_listCount].SpeakerType == SpeakerType.Player &&
                    core.GroupNo == CharacterGroupNumber.player)
                {
                    m_conversationCameraLookAtTransform = core.PlayerParameters.ConversationCameraTransform;
                    m_conversationCameraFollowTransform = core.transform;
                    m_characterCore = core;
                    break;
                }

                if ((core.GroupNo == CharacterGroupNumber.NPC)
                && (core.NPCParameters.SpeakerType == m_conversationList[m_listCount].SpeakerType))
                {
                    m_conversationCameraLookAtTransform = core.NPCParameters.CameraTrans;
                    m_conversationCameraFollowTransform = core.transform;
                    m_characterCore = core;
                    m_cameraDist = m_conversationList[m_listCount].DistantCamera;
                    break;
                }

            }

            // アニメーション再生
            if (m_characterCore)
            {
                m_characterCore.m_animator.ResetTrigger("IsStand");
                string animeTrigger = m_conversationList[m_listCount].AnimeTrigger;
                m_characterCore.m_animator.SetBool(animeTrigger, true);
            }

            // カメラ切り替えるかどうか
            if (m_bChangeCamFlg == false)
            {
                CutSceneManager.instance.PlayCutScene(CutSceneNumber.CutSceneNoCamera);
            }
            else
            {
                // 会話用のカメラに切り替える
                if (m_conversationCameraLookAtTransform && m_conversationCameraFollowTransform)
                {
                    if (CutSceneManager.instance == null)
                    {
                        Debug.LogError("CutSceneManagerが存在しません");
                        return;
                    }

                    CutSceneManager.instance.SetConversationCammeraLookAt(m_conversationCameraLookAtTransform);
                    CutSceneManager.instance.SetConversationCammeraFollow(m_conversationCameraFollowTransform);
                    CutSceneManager.instance.SetConversationCammeraDiastance(m_cameraDist);

                    CutSceneManager.instance.PlayCutScene(CutSceneNumber.CutSceneCamera);

                }
            }

            StopAnimation();
            StopCameraAngle();

            // 会話時にアイテム表示するかどうか
            if (m_conversationList[m_listCount].ActiveItemName.Length > 0)
            {
                m_characterCore.NPCParameters?.SetActiveListObj(m_conversationList[m_listCount].ActiveItemName);
            }

            // 会話時にむくキャラクターの方向を変化するか
            if(m_conversationList[m_listCount].FaceSpeakeTypeInChange != SpeakerType.None)
            {
                m_characterCore.NPCParameters.NPCFaceSpeakerType = 
                    m_conversationList[m_listCount].FaceSpeakeTypeInChange;
                m_characterCore.NPCParameters.m_bChangetFaceSpeaker.Value = true;
            }

            await UniTask.DelayFrame(1);
            cancelToken.ThrowIfCancellationRequested();

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }


    public override async UniTask OnUpdate()
    {
        #region nullチェック
        // Nullチェック
        if (m_conversationText == null)
        {
            return;
        }
        #endregion

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();


                // 会話更新
                if (IsNextConversationText() == false)
                {
                    CutSceneManager.instance.ChangeExtentionMode(DirectorWrapMode.None);

                    ReStartAnimation();
                    RestartCameraAngle();



                    m_characterCore?.m_animator.SetTrigger("IsFinish");

                    // 報酬があるなら取得する
                    if (m_conversationData.RewardList.Count != 0)
                    {
                        var fase = m_conversationData.TalkingFase;
                        if (m_conversationData.RewardList.ContainsKey(fase))
                        {
                            var rewardChallenge = Instantiate(m_conversationData.RewardList[fase]);
                            rewardChallenge.UpdateRewardChallenge();
                            if (rewardChallenge != null) Destroy(rewardChallenge.gameObject);
                        }
                    }

                    //// トランジション開始
                    //TransitionController transitionController = CutSceneManager.instance.TransitionController;
                    //if (transitionController)
                    //{
                    //    transitionController.StartTransition(0.0f, 2.0f);
                    //}

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

    private bool IsNextConversationText()
    {
        bool nextTextFlg = true;

        if (m_nextInputActionButton.IsInputActionTrriger())
        {
            string animeTrigger = m_conversationList[m_listCount].AnimeTrigger;

            if (m_bSkipFlg == false && (m_effectText?.IsSkip == false))
            {
                // キャンセル処理
                m_effectText?.CancelAnimation();
                m_bSkipFlg = true;

                return nextTextFlg;
            }

            animeTrigger = m_conversationList[m_listCount].AnimeTrigger;
            if (animeTrigger.Length != 0)
                m_characterCore.m_animator.SetBool(animeTrigger, false);

            // 前回で表示にしたアイテム名を取得
            string showItemName = m_conversationList[m_listCount].ActiveItemName;
            // 前回のキャラクターコアを保存
            var preCharacterCore = m_characterCore;

            m_listCount++;
            m_bSkipFlg = false;

            // 会話終了
            if (m_conversationList.Count <= m_listCount)
            {
                // 会話時を終了する際にはアイテムを非表示に
                if(showItemName.Length>0)
                {
                    m_characterCore.NPCParameters?.SetNotActiveAllListObj();
                }

                nextTextFlg = false;
                return nextTextFlg;
            }

            //名前変更
            m_conversationList[m_listCount].SpeakerName.StringChanged += (text) =>
            {
                m_speakerNameText.text = null;
                m_speakerNameText.text = text;
            };

            //会話更新
            m_conversationList[m_listCount].LocalizedString.StringChanged += (text) =>
            {
                m_conversationText.text = null;
                m_conversationText.text = text;
            };

            if (m_effectText)
            {
                _ = m_effectText.PlayEffect();
            }

            if (m_bChangeCamFlg)
            {
                // カメラターゲットを変更（メタリストから探索）
                foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
                {
                    if (m_conversationList[m_listCount].SpeakerType == SpeakerType.Player &&
                        core.GroupNo == CharacterGroupNumber.player)
                    {
                        m_conversationCameraLookAtTransform = core.PlayerParameters.ConversationCameraTransform;
                        m_conversationCameraFollowTransform = core.transform;
                        m_characterCore = core;
                        break;
                    }

                    if ((core.GroupNo == CharacterGroupNumber.NPC)
                    && (core.NPCParameters.SpeakerType == m_conversationList[m_listCount].SpeakerType))
                    {
                        m_conversationCameraLookAtTransform = core.NPCParameters.CameraTrans;
                        m_conversationCameraFollowTransform = core.transform;
                        m_characterCore = core;
                        m_cameraDist = m_conversationList[m_listCount].DistantCamera;
                        break;
                    }

                }

                // アニメーション再生
                if (m_characterCore)
                {
                    m_characterCore?.m_animator.SetTriggerOneShot("IsFinishArmLayer");
                    animeTrigger = m_conversationList[m_listCount].AnimeTrigger;
                    if (animeTrigger.Length != 0)
                        m_characterCore.m_animator.SetBool(animeTrigger, true);
                }

                // 会話用のカメラに情報伝達
                if (m_conversationCameraLookAtTransform && m_conversationCameraFollowTransform)
                {
                    CutSceneManager.instance.SetConversationCammeraLookAt(m_conversationCameraLookAtTransform);
                    CutSceneManager.instance.SetConversationCammeraFollow(m_conversationCameraFollowTransform);
                    CutSceneManager.instance.SetConversationCammeraDiastance(m_cameraDist);
                }
            }

            // 会話時にアイテムを非表示にするかどうか

            // 次の会話で表示するアイテムがあるなら表示
            if (m_conversationList[m_listCount].ActiveItemName.Length>0)
            {
                m_characterCore.NPCParameters?.SetActiveListObj(m_conversationList[m_listCount].ActiveItemName);

            }

            // 会話時にむくキャラクターの方向を変化するか
            if (m_conversationList[m_listCount].FaceSpeakeTypeInChange != SpeakerType.None)
            {
                m_characterCore.NPCParameters.NPCFaceSpeakerType =
                   m_conversationList[m_listCount].FaceSpeakeTypeInChange;
                m_characterCore.NPCParameters.m_bChangetFaceSpeaker.Value = true;
               
            }

        }

        return nextTextFlg;

    }

    // プレイヤーとNPC以外のアニメーションを停止
    private void StopAnimation()
    {
        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {

            if (core.GroupNo == CharacterGroupNumber.player || core.GroupNo == CharacterGroupNumber.NPC)
            {
                //　会話相手の方にプレイヤーを向ける処理
                if (core.GroupNo == CharacterGroupNumber.player && m_conversationCameraFollowTransform)
                {
                    core.ConversationObjTrans = m_conversationCameraFollowTransform;
                    var targetVec = core.ConversationObjTrans.position 
                                        - core.transform.position;
                    targetVec.y = 0.0f;
                    targetVec.Normalize();

                    core.SetRotateToTarget(targetVec, false);
                }

                // 武器を消す
                if (core.PlayerParameters != null)
                {
                    core.PlayerParameters.HideWeapon(core.m_animator);
                    core.PlayerParameters.IsNotAutoAppearWepon = true;
                }

                continue;
            }

            core.StopAnimationSpeedFlg = true;
            core.m_animator.speed = 0.0f;
            core.CharaCtrl.MoveSpeed = 0.0f;
            core.EnemyParameters?.Arbor.Pause();

        }
    }

    private void StopCameraAngle()
    {
        PlayerInputManager.instance.GetInputActionMap(InputActionMapTypes.Camera).Disable();


        var freelookCamera = FindObjectOfType<CinemachineFreeLook>();
        if (freelookCamera)
            freelookCamera.GetComponent<CinemachineInputProvider>().enabled = false;

    }

    private void RestartCameraAngle()
    {
        PlayerInputManager.instance.GetInputActionMap(InputActionMapTypes.Camera).Enable();
        var freelookCamera = FindObjectOfType<CinemachineFreeLook>();
        if (freelookCamera == null) return;
        freelookCamera.GetComponent<CinemachineInputProvider>().enabled = true;



    }

    // 再開
    private void ReStartAnimation()
    {
        foreach (var core in IMetaAI<CharacterCore>.Instance.ObjectList)
        {
            if (core.GroupNo == CharacterGroupNumber.player || core.GroupNo == CharacterGroupNumber.NPC)
            {
                // 武器出現
                if (core.PlayerParameters != null)
                {
                    core.PlayerParameters.IsNotAutoAppearWepon = false;
                }

                continue;
            }

            core.StopAnimationSpeedFlg = false;
            core.m_animator.speed = 1.0f;
            core.EnemyParameters?.Arbor.Resume();

        }
    }





}

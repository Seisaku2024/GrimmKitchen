using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Timeline;


public enum CutSceneNumber
{
    none,
    gameOver,
    reStart,
    StartMovie,
    BossBark,
    CutSceneCamera,
    InTheBook,
    OPMovie,
    CutSceneNoCamera,
    max,
}


public class CutSceneManager : BaseManager<CutSceneManager>
{
    //カットシーンにアクセスするためのマネージャークラス（山本）
    [SerializeField] private PlayableDirector m_playableDirector = null;
    public PlayableDirector PlayableDirector => m_playableDirector;


    [SerializeField] private SerializableDictionary<CutSceneNumber, TimelineAsset> m_dictionaryTimeline;

    [Header("会話用のカメラセット")]
    [SerializeField] private CinemachineCamera m_conversationCamera;
    public CinemachineCamera conversationcamera => m_conversationCamera;

    [Header("トランジションコントローラー")]
    [SerializeField]
    private TransitionController m_transitionController = null;
    public TransitionController TransitionController => m_transitionController;

    public bool PlayCutScene(CutSceneNumber _cutSceneNumber)
    {
        if (!m_playableDirector)
        {
            Debug.LogError("PlayableDirectorが登録されてない");
            return false;
        }

        switch (_cutSceneNumber)
        {
            case CutSceneNumber.gameOver:
            case CutSceneNumber.CutSceneNoCamera:
                {
                    if (m_dictionaryTimeline.TryGetValue(_cutSceneNumber, out TimelineAsset asset))
                    {
                        m_playableDirector.Play(asset);
                        //WrapModeをHoldに変更
                        m_playableDirector.extrapolationMode = DirectorWrapMode.Hold;

                        return true;
                    }
                    else
                    {
                        Debug.LogError("タイムラインアセットが登録されてない");
                    }
                }
                break;

            case CutSceneNumber.reStart:
                {
                    if (m_dictionaryTimeline.TryGetValue(_cutSceneNumber, out TimelineAsset asset))
                    {
                        m_playableDirector.Play(asset);
                        //WrapModeをNoneに変更
                        m_playableDirector.extrapolationMode = DirectorWrapMode.None;

                        return true;
                    }
                    else
                    {
                        Debug.LogError("タイムラインアセットが登録されてない");
                    }
                }
                break;

            case CutSceneNumber.StartMovie:
            case CutSceneNumber.InTheBook:
            case CutSceneNumber.OPMovie:
            {
                    if (m_dictionaryTimeline.TryGetValue(_cutSceneNumber, out TimelineAsset asset))
                    {
                        m_playableDirector.Play(asset);
                        //WrapModeをNoneに変更
                        m_playableDirector.extrapolationMode = DirectorWrapMode.None;

                        return true;
                    }
                    else
                    {
                        Debug.LogError("タイムラインアセットが登録されてない");
                    }
                }
                break;
            case CutSceneNumber.BossBark:
                {
                    if (m_dictionaryTimeline.TryGetValue(_cutSceneNumber, out TimelineAsset asset))
                    {
                        m_playableDirector.Play(asset);
                        //WrapModeをNoneに変更
                        m_playableDirector.extrapolationMode = DirectorWrapMode.None;

                        return true;
                    }
                    else
                    {
                        Debug.LogError("タイムラインアセットが登録されてない");
                    }
                }
                break;
            case CutSceneNumber.CutSceneCamera:
                {
                    if (m_dictionaryTimeline.TryGetValue(_cutSceneNumber, out TimelineAsset asset))
                    {
                        m_playableDirector.Play(asset);
                        //WrapModeをNoneに変更
                        m_playableDirector.extrapolationMode = DirectorWrapMode.Hold;

                        if(m_conversationCamera)
                        m_conversationCamera.Priority = 10;

                        return true;
                    }
                    else
                    {
                        Debug.LogError("タイムラインアセットが登録されてない");
                    }
                }
                break;
            default:
                break;

        }

        return false;
    }

    public void StopCutScene()
    {
        if (IsCutScenePlay() && m_playableDirector)
        {
            m_playableDirector.Stop();
        }
    }

    public void ChangeExtentionMode(DirectorWrapMode _wrapMode)
    {
        if(m_playableDirector)
        {
            m_playableDirector.extrapolationMode = _wrapMode;
        }

        if(m_conversationCamera)
        {
            m_conversationCamera.Priority = 1;
        }

    }
    

    public bool IsCutScenePlay()
    {
        if (!m_playableDirector)
        {

            Debug.LogError("PlayableDirectorがセットされてないです。");
            return false;
        }

        //シーンマネージャーにセットされたディレクターが再生中ならTrue
        if (m_playableDirector.state == PlayState.Playing)
        {
            return true;
        }
        else { return false; }

    }

    public void SetConversationCammeraLookAt(Transform _transform)
    {
        if (m_conversationCamera == null) return;
        m_conversationCamera.LookAt = _transform;
    }

    public void SetConversationCammeraFollow(Transform _transform)
    {
        if (m_conversationCamera == null) return;
        m_conversationCamera.Follow = _transform;
    }

    public void SetConversationCammeraDiastance(float dist)
    {
        if (m_conversationCamera == null) return;
        // CinemachineFramingTransposerは非推奨のためCinemachinePositionComposerに置き換え
        var composer = m_conversationCamera.GetComponent<CinemachineOrbitalFollow>();
        if (composer != null)
        {
            composer.Radius = dist;
        }
    }


    public void SetCinamachineBrainBlendTime(float value)
    {
        if (Camera.main.TryGetComponent(out CinemachineBrain cinemachineBrain))
        {
            cinemachineBrain.DefaultBlend.Time = value;
        }
    }



}

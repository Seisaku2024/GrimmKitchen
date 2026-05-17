using Cysharp.Threading.Tasks;
using MackySoft.Navigathena;
using MackySoft.Navigathena.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class TestNotingEntryPoint : EntryPointBase
{
    [SerializeField] private CaptureScreenToImage m_capture;

    [Header("設定すれば遷移時カメラを無効にする")]
    [SerializeField] private Camera m_camera = null;

    [Header("遷移時削除するオブジェクト")]
    [SerializeField, NonReorderable] private List<UnityEngine.Object> m_deleteObjects = new List<UnityEngine.Object>();


    [Header("アクティブをtrueにするリスト")]
    [Header("※シリアライズするオブジェクトはアクティブをfalseにすること")]
    [SerializeField] private List<GameObject> m_objList = new();

    [SerializeField] private GameObject pagePrefab = null;


    [Header("入場イベント再生するか")]
    [SerializeField] private bool m_bEntryMovie = true;

    [Header("OPSceneでOPムービー再生するか")]
    [SerializeField] private bool m_bStartOPMovie = false;


    // 遷移演出の終了後に呼び出されます。
    protected override async UniTask OnEnter(ISceneDataReader reader, CancellationToken cancellationToken)
    {
        // リストで更新(田内)
        foreach (var obj in m_objList)
        {
            // nullチェック追加(根冝)
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        if (CutSceneManager.instance != null)
        {
            if (m_bEntryMovie)
            {
                //入場イベント再生
                CutSceneManager.instance.PlayCutScene(CutSceneNumber.StartMovie);
            }
            else if (m_bStartOPMovie)
            {
                //OPムービー再生
                CutSceneManager.instance.PlayCutScene(CutSceneNumber.OPMovie);
            }
            else
            {
                if (IMetaAI<CharacterCore>.Instance == null)
                {
                    return;
                }

                foreach (var chara in IMetaAI<CharacterCore>.Instance.ObjectList)
                {
                    if (chara.GroupNo == CharacterGroupNumber.player)
                    {
                        if (chara.IsFinishJumpFlg)
                            chara.m_animator.SetBool("FinishJumpFlg", true);
                    }
                }
            }
        }

        // 1フレーム目は必ずスキップ
        if(Time.frameCount==1)
        {
            return;
        }


        if (pagePrefab)
        {
            GameObject instance = Instantiate(pagePrefab);

            NextPageTransitionBehaviour nextPageTransitionBehaviour = instance.GetComponentInChildren<NextPageTransitionBehaviour>();

            if (nextPageTransitionBehaviour)
            {
                await nextPageTransitionBehaviour.NextPage();



            }
            Destroy(instance);
        }




    }

    // タイトル画面は一番最初に実行されるものとして、ビルド時にも同様の必須初期化処理を実行させるための処理

#if UNITY_EDITOR

#else
    protected override async UniTask OnInitialize(ISceneDataReader reader, IProgress<IProgressDataStore> transitionProgress, CancellationToken cancellationToken)
    {
    //await OnFirstPreInitializeFunc();
    }

#endif


    // 遷移演出の開始前に呼び出されます。
    protected override async UniTask OnExit(ISceneDataWriter writer, CancellationToken cancellationToken)
    {

        

        await m_capture.Capture();
        if (m_camera)
        {
            m_camera.enabled = false;
        }
        foreach (var obj in m_deleteObjects)
        {
            Destroy(obj);
        }
    }
}

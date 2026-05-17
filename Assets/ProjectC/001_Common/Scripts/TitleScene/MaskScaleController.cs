using DG.Tweening;
using JetBrains.Annotations;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class MaskScaleController : MonoBehaviour
{
    // タイトルのマスクテクスチャのスケール値を変更する（山本）

    [Header("操作する逆マスクのリスト")]
    [SerializeField] List<Transform> m_substractMaskTexTransformList = new List<Transform>();
    [Header("操作するマスクのリスト")]
    [SerializeField] List<Transform> m_additiveMaskTexTransformList = new List<Transform>();

    //スケール値の変更が完了するとまでの時間
    [Header("マスクのスケールの変化が完了するまでの時間")]
    [SerializeField] private float m_changeFinishScaleTime = 0.0f;
    //スクリーンショットの変更が起きるまでの時間
    [Header("スクリーンショットを表示している時間(逆マスクの表示時間)")]
    [SerializeField] private float m_changeScreenshotTime = 5.0f;
    //マスクアニメーションを表示するまでの時間
    [Header("マスク画像を表示する時間（スクリーンショットが表示されない時間）")]
    [SerializeField] private float m_changeMaskAnimationTime = 0.5f;

    [Header("乱数の最大値")]
    [SerializeField] private float m_maxNum = 0.1f;

    [Header("乱数の最小値")]
    [SerializeField] private float m_minNum = -0.1f;

    [Header("切り替えるスクリーンショットリスト")]
    [SerializeField] private List<Sprite> m_screenShotList = new List<Sprite>();

    [Header("変更するスクリーンショット")]
    [SerializeField] private UnityEngine.UI.Image m_titleBackTexture;

    [Header("マスクテクスチャの拡縮の最大倍率")]
    [SerializeField] private float m_maxScaleNum = 1.2f;

    [Header("マスクテクスチャの拡縮の最小倍率")]
    [SerializeField] private float m_minScaleNum = 0.8f;

    [Header("マスクテクスチャの拡縮アニメーションの最大時間")]
    [SerializeField] private float m_scaleAnimationMaxTime = 2.0f;

    [Header("マスクテクスチャの拡縮アニメーションの最小時間")]
    [SerializeField] private float m_scaleAnimationMinTime = 1.0f;




    //逆ステンシルマスクのリスト
    private Dictionary<Transform, TitleMask> m_substractMaskList = new Dictionary<Transform, TitleMask>();
    //ステンシルマスクのリスト
    private Dictionary<Transform, TitleMask> m_additiveMaskList = new Dictionary<Transform, TitleMask>();


    //　逆マスクのスケール値を記録するリスト
    List<Vector3> m_substractMaskScaleList = new List<Vector3>();
    //　マスクのスケール値を記録するリスト
    List<Vector3> m_additiveMaskScaleList = new List<Vector3>();

    // スクリーンショットリストのアクセスする要素番号
    int m_listNum = 0;

    // MaskControllerの状態
    public enum MaskState
    {
        stay,
        substractMaskAnimation,
        additiveMaskAnimation,
        changeScreenShot,
        none,

    }
    private MaskState m_maskState = MaskState.stay;


    void Awake()
    {
        foreach (var mask in m_substractMaskTexTransformList)
        {

            m_substractMaskScaleList.Add(mask.localScale);

            //ステートマシーン----------------------------------------------------------------
            TitleMask titleMask = new TitleMask();

            if (mask.gameObject.TryGetComponent(out TitleMask title))
            {
                titleMask = title;
            }
            else
            {
                break;
            }


            titleMask.MaskStateMachine.m_scale = mask.localScale;
            titleMask.MaskStateMachine.m_bFinish = false;
            titleMask.MaskStateMachine.m_maskStateInfo = MaskStateInfo.maskOpen;
            titleMask.MaskStateMachine.m_changeScaleTime = m_changeFinishScaleTime
                + UnityEngine.Random.Range(m_minNum, m_maxNum);
            titleMask.MaskStateMachine.m_changeStateTime = m_changeScreenshotTime;
            titleMask.MaskStateMachine.m_transform = mask;
            titleMask.MaskStateMachine.MaskBaseState.SceneChangeFlg = true;

            titleMask.MaskStateMachine.m_maxScaleNum = m_maxScaleNum;
            titleMask.MaskStateMachine.m_minScaleNum = m_minScaleNum;   
            titleMask.MaskStateMachine.m_scaleAnimationMaxTime = m_scaleAnimationMaxTime;
            titleMask.MaskStateMachine.m_scaleAnimationMinTime = m_scaleAnimationMinTime;   


            m_substractMaskList.Add(mask, titleMask);

            mask.localScale = Vector3.zero;

        }


        foreach (var mask in m_additiveMaskTexTransformList)
        {
            //ステートマシーン----------------------------------------------------------------
            TitleMask titleMask = new TitleMask();

            if (mask.gameObject.TryGetComponent(out TitleMask title))
            {
                titleMask = title;
            }
            else
            {
                break;
            }


            m_additiveMaskScaleList.Add(mask.localScale);


            titleMask.MaskStateMachine.m_scale = mask.localScale;
            titleMask.MaskStateMachine.m_bFinish = false;
            titleMask.MaskStateMachine.m_maskStateInfo = MaskStateInfo.maskOpen;
            titleMask.MaskStateMachine.m_changeScaleTime = m_changeFinishScaleTime
                + UnityEngine.Random.Range(m_minNum, m_maxNum);
            titleMask.MaskStateMachine.m_changeStateTime = m_changeMaskAnimationTime;
            titleMask.MaskStateMachine.m_transform = mask;
            titleMask.MaskStateMachine.MaskBaseState.SceneChangeFlg = true;

            m_additiveMaskList.Add(mask, titleMask);

            //0で初期化
            mask.localScale = Vector3.zero;

        }

        //最初は逆マスクのアニメーション
        m_maskState = MaskState.substractMaskAnimation;

        //スクリーンショットをリストの一番に変更
        if(m_titleBackTexture)
        m_titleBackTexture.sprite = m_screenShotList[m_listNum];

    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();
    }


    private void OnStateUpdate()
    {
        switch (m_maskState)
        {
            case MaskState.substractMaskAnimation:
                OnSubstractMaskAimation();
                break;

            case MaskState.additiveMaskAnimation:
                OnAdditiveMaskAnimation();
                break;

            case MaskState.changeScreenShot:
                ChangeScreenshot();
                break;
            default:
                Debug.Log("何の更新もしてない");
                break;

        }

    }

    //　逆マスクのアニメーション
    private void OnSubstractMaskAimation()
    {
        bool m_bAllMaskFinishFlg = true;

        foreach (var mask in m_substractMaskTexTransformList)
        {
            m_substractMaskList[mask].OnUpdate();

            //　処理が終了してないMask画像があるならfalseにする（次に進まないように）
            if (m_substractMaskList[mask].GetFinishFlg() == false)
            {
                m_bAllMaskFinishFlg = false;
            }

        }


        // 逆Mask画像の処理が全て終了しているなら再度初期化を行う
        if (m_bAllMaskFinishFlg)
        {
            //State変更
            m_maskState = MaskState.additiveMaskAnimation;

            //初期化
            foreach (var mask in m_substractMaskTexTransformList)
            {

                m_substractMaskList[mask].MaskStateMachine.m_bFinish = false;
                m_substractMaskList[mask].MaskStateMachine.m_maskStateInfo = MaskStateInfo.maskOpen;
                m_substractMaskList[mask].MaskStateMachine.m_changeStateTime = m_changeScreenshotTime;
                m_substractMaskList[mask].MaskStateMachine.m_changeScaleTime = m_changeFinishScaleTime
                     + UnityEngine.Random.Range(m_minNum, m_maxNum);
                m_substractMaskList[mask].MaskStateMachine.MaskBaseState.SceneChangeFlg = true;


            }

            Debug.Log("逆マスクのアニメーション終了");
        }
    }

    // マスクのアニメーション
    private void OnAdditiveMaskAnimation()
    {
        bool m_bAllMaskFinishFlg = true;

        foreach (var mask in m_additiveMaskTexTransformList)
        {
            m_additiveMaskList[mask].OnUpdate();

            //　処理が終了してないMask画像があるならfalseにする（次に進まないように）
            if (m_additiveMaskList[mask].GetFinishFlg() == false)
            {
                m_bAllMaskFinishFlg = false;
            }

        }

        // Mask画像の処理が全て終了しているならStateを変更と初期化
        if (m_bAllMaskFinishFlg)
        {
            //State変更
            m_maskState = MaskState.changeScreenShot;

            //初期化

            foreach (var mask in m_additiveMaskTexTransformList)
            {

                m_additiveMaskList[mask].MaskStateMachine.m_bFinish = false;
                m_additiveMaskList[mask].MaskStateMachine.m_maskStateInfo = MaskStateInfo.maskOpen;
                m_additiveMaskList[mask].MaskStateMachine.m_changeStateTime = m_changeMaskAnimationTime;
                m_additiveMaskList[mask].MaskStateMachine.m_changeScaleTime = m_changeFinishScaleTime
                     + UnityEngine.Random.Range(m_minNum, m_maxNum);
                m_additiveMaskList[mask].MaskStateMachine.MaskBaseState.SceneChangeFlg = true;

            }

            Debug.Log("マスクのアニメーション終了");
        }

    }

    // スクリーンショットの変更
    private void ChangeScreenshot()
    {
        //スケール値を0に変更
        foreach (var mask in m_substractMaskTexTransformList)
        {
            mask.localScale = Vector3.zero;
        }

        foreach (var mask in m_additiveMaskTexTransformList)
        {
            mask.localScale = Vector3.zero;
        }


        //スクリーンショットの変更
        if (m_titleBackTexture)
        {

            m_listNum++;
            //m_listNumの値がm_screenShotListの登録数より大きければ最初に戻す
            if (m_listNum > m_screenShotList.Count - 1)
            {
                m_listNum = 0;
            }

            //リストから変更するテクスチャを指定
            m_titleBackTexture.sprite = m_screenShotList[m_listNum];


        }

        //State変更
        m_maskState = MaskState.substractMaskAnimation;

        Debug.Log("スクリーンショットの変更");
    }

}

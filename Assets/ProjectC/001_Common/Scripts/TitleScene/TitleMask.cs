using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMask : MonoBehaviour
{
    // Mask画像のStateMachine(山本)

    public MaskStateMachine MaskStateMachine { get { return m_maskStateMachine; } set { m_maskStateMachine = value; } }
    //　マスク画像のStateMachine
    private MaskStateMachine m_maskStateMachine = new MaskStateMachine();

    private void Start()
    {
        m_maskStateMachine.Start();
        //m_maskStateMachine.ChangeMaskState(new WaitState());
    }

    // アニメーションの一連の流れ（開く→停止→閉じる）が終了しているかを確認
    public bool GetFinishFlg()
    {
        return m_maskStateMachine.m_bFinish;
    }

    public void OnUpdate()
    {

        m_maskStateMachine.Update();
    }

}

public enum MaskStateInfo
{
    wait,
    maskCloth,
    maskOpen,
    None
}

public class MaskStateMachine
{
    public MaskBaseState MaskBaseState { get { return m_maskState; } set { m_maskState = value; } }

    private MaskBaseState m_maskState = new MaskBaseState();

    public Transform m_transform;
    public Vector3 m_scale = Vector3.zero;
    public bool m_bFinish = false;
    public float m_changeScaleTime = 0.0f;
    public float m_changeStateTime = 0.0f;
    public MaskStateInfo m_maskStateInfo = MaskStateInfo.wait;
    public bool m_sceneChangeFlg = false;
    public Vector3 m_waitAnimeScale = Vector3.zero;
    public float m_changeWaitScaleTime = 0.0f;

    //ScaleAnimation
    public float m_maxScaleNum = 0.0f;
    public float m_minScaleNum = 0.0f;
    public float m_scaleAnimationMaxTime = 0.0f;
    public float m_scaleAnimationMinTime = 0.0f;

    public void Start()
    {
        m_maskState.SetStateMacine = this;
    }


    public void Update()
    {
        if (m_maskState.SceneChangeFlg)
        {
            ChangeState();
            m_maskState.SceneChangeFlg = false;
        }

        m_maskState.Update();
    }
    public void ChangeMaskState(MaskBaseState maskBase)
    {
        if (m_maskState != null)
        {
            m_maskState.Exit();
        }

        m_maskState = maskBase;

        m_maskState.Enter(this);
    }

    private void ChangeState()
    {
        switch (m_maskStateInfo)
        {
            case MaskStateInfo.wait:

                ChangeMaskState(new WaitState());

                break;

            case MaskStateInfo.maskOpen:

                ChangeMaskState(new MaskOpenState());

                break;

            case MaskStateInfo.maskCloth:

                ChangeMaskState(new MaskCloseState());

                break;

            default:
                break;

        }

    }


}

public class MaskBaseState
{
    public MaskBaseState() { }
    ~MaskBaseState() { }

    //仮想関数----------------------------------------------------------------

    //初期
    public virtual void Enter(MaskStateMachine maskState)
    {
        m_maskBaseState = maskState;
    }
    //更新
    public virtual void Update() { }
    //終了
    public virtual void Exit() { }

    //------------------------------------------------------------------------

    //public Transform Transform { get { return m_transform; } set { m_transform = value; } }
    //public Vector3 Scale { get { return m_scale; } set { m_scale = value; } }
    //public bool FinishFlg { get { return m_bFinish; } set { m_bFinish = value; } }
    //public float ChangeScaleTime { get { return m_changeScaleTime; } set { m_changeScaleTime = value; } }
    //public float ChangeStateTime { get { return m_changeStateTime; } set { m_changeStateTime = value; } }
    //public MaskStateInfo GetMaskState { get { return m_maskState; } set { m_maskState = value; } }
    public bool SceneChangeFlg { get { return m_sceneChangeFlg; } set { m_sceneChangeFlg = value; } }

    public MaskStateMachine SetStateMacine { get { return m_maskBaseState; } set { m_maskBaseState = value; } }


    protected MaskStateMachine m_maskBaseState;

    //protected Transform m_transform;
    //protected Vector3 m_scale = Vector3.zero;
    //protected bool m_bFinish = false;
    //protected float m_changeScaleTime = 0.0f;
    //protected float m_changeStateTime = 0.0f;
    //protected MaskStateInfo m_maskState = MaskStateInfo.wait;
    protected bool m_sceneChangeFlg = false;

}

//待機状態
public class WaitState : MaskBaseState
{
    private bool m_changeFinish = false;
    private DOTween m_tween;

    public override void Enter(MaskStateMachine maskState)
    {
        base.Enter(maskState);
    }

    public override void Update()
    {
        base.Update();

        if (m_maskBaseState.m_changeStateTime <= 0.0f)
        {
            m_maskBaseState.m_maskStateInfo = MaskStateInfo.None;
            m_sceneChangeFlg = true;
            m_maskBaseState.m_bFinish = true;
        }
        else
        {
            m_maskBaseState.m_changeStateTime -= Time.deltaTime;


            if (m_changeFinish == false)
            {
                m_maskBaseState.m_changeWaitScaleTime 
                    = UnityEngine.Random.Range
                    (
                    m_maskBaseState.m_scaleAnimationMinTime,
                    m_maskBaseState.m_scaleAnimationMaxTime
                    );

                Vector3 rundomNum = new Vector3
                            (
                            UnityEngine.Random.Range(m_maskBaseState.m_minScaleNum, m_maskBaseState.m_maxScaleNum),
                            UnityEngine.Random.Range(m_maskBaseState.m_minScaleNum, m_maskBaseState.m_maxScaleNum),
                            1.0f
                            );

                m_maskBaseState.m_waitAnimeScale = m_maskBaseState.m_scale;
                m_maskBaseState.m_waitAnimeScale.x *= rundomNum.x;
                m_maskBaseState.m_waitAnimeScale.y *= rundomNum.y;



                m_maskBaseState.m_transform.DOScale(m_maskBaseState.m_waitAnimeScale,
                m_maskBaseState.m_changeWaitScaleTime)
                .OnComplete
                (
                () =>
                {
                    Vector3 rundomNum = new Vector3
                             (
                             UnityEngine.Random.Range(m_maskBaseState.m_minScaleNum, m_maskBaseState.m_maxScaleNum),
                             UnityEngine.Random.Range(m_maskBaseState.m_minScaleNum, m_maskBaseState.m_maxScaleNum),
                             1.0f
                             );

                    m_maskBaseState.m_waitAnimeScale = m_maskBaseState.m_scale;
                    m_maskBaseState.m_waitAnimeScale.x *= rundomNum.x;
                    m_maskBaseState.m_waitAnimeScale.y *= rundomNum.y;

                    m_changeFinish = false;
                }
                );

                m_changeFinish = true;

            }


        }

    }

    public override void Exit()
    {
        base.Update();
    }

}

public class MaskOpenState : MaskBaseState
{
    public override void Enter(MaskStateMachine maskState)
    {
        base.Enter(maskState);

        m_maskBaseState.m_transform.DOScale(m_maskBaseState.m_scale, m_maskBaseState.m_changeScaleTime)
        .OnComplete
        (
        () =>
        {
            m_maskBaseState.m_maskStateInfo = MaskStateInfo.wait;
            m_sceneChangeFlg = true;

        }
         );

    }

}

public class MaskCloseState : MaskBaseState
{
    public override void Enter(MaskStateMachine maskState)
    {
        base.Enter(maskState);

        m_maskBaseState.m_transform.DOScale(0.0f, m_maskBaseState.m_changeScaleTime)
        .OnComplete
        (
        () =>
        {
            m_maskBaseState.m_maskStateInfo = MaskStateInfo.None;
            m_sceneChangeFlg = true;
            m_maskBaseState.m_bFinish = true;
        }
         ).SetEase(Ease.InQuad);

    }



}



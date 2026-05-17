using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class EffectController : MonoBehaviour
{
    //召喚エフェクトのコントローラー(山本)

    [Header("Timeline")]
    [SerializeField]
    PlayableAsset m_playableAsset;

    [Header("VisualEffect")]
    [SerializeField]
    private VisualEffect m_visualEffect;

    [Header("ディゾルブ処理完了までの時間")]
    [SerializeField]
    private float m_dissolveTime = 1.0f;

    [Header("エフェクト開始の名前")]
    [SerializeField]
    private string m_startEventName = "none";

    [Header("エフェクト終了の名前")]
    [SerializeField]
    private string m_stopEventName = "none";

    //エフェクトの終了時刻
    private float m_endEffectTime = 5.0f;
    public float EndEffectTime { set { m_endEffectTime = value; } }

    //ディゾルブの進捗度
    //1.0fなら完全にディゾルブされる
    private float m_dissolveNum = 0.0f;

    //ディゾルブ完了のフラグ
    private bool m_startDissolveFlg = false;

    private void Awake()
    {
        m_dissolveNum = 1.0f;
        m_startDissolveFlg = false;

        if (m_visualEffect)
        {
            m_visualEffect.SetFloat("DissolveAmount", m_dissolveNum);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        DOVirtual.Float
          (m_dissolveNum,
          0.0f,
          m_dissolveTime,
          (tweenValue) =>
          {
              if (m_visualEffect)
              {
                  m_visualEffect.SetFloat("DissolveAmount", tweenValue);

                  if (m_visualEffect.GetFloat("DissolveAmount") == 0.0f)
                  {
                      m_dissolveNum = 0.0f;
                      m_visualEffect?.SendEvent("PaperMove");
                  }

              }
          }
          );

    }

    // Update is called once per frame
    void Update()
    {
        if (m_endEffectTime >= 0.0f)
        {
            m_endEffectTime -= Time.deltaTime;
        }
        else
        {
            if (m_startDissolveFlg == false)
            {
                //ディゾルブ開始
                DOVirtual.Float
              (m_dissolveNum,
              1.0f,
              m_dissolveTime,
              (tweenValue) =>
              {
                  if (m_visualEffect)
                  {
                      m_startDissolveFlg = true;
                      m_visualEffect.SendEvent("PaperStop");
                      m_visualEffect.SetFloat("DissolveAmount", tweenValue);

                      if (m_visualEffect.GetFloat("DissolveAmount") == 1.0f)
                      {
                          Destroy(gameObject);
                      }

                  }
              }
              );
            }
        }

    }
}

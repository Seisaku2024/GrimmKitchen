using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントUIのアニメーションをする　制作者　吉田
/// 
/// 今のところ、拡大縮小のみ
/// 
/// </summary>
public class EventUIAnimation : MonoBehaviour
{
    [SerializeField]
    private float m_scale = 1.0f;

    [SerializeField]
    private Ease m_ease = Ease.InOutQuad;

    [SerializeField]
    private float m_time = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartAnimetion();
    }

    private void OnEnable()
    {
        StartAnimetion();
    }

    public void StartAnimetion()
    {
        gameObject.transform.DOKill();

        gameObject.transform.DOScale(m_scale, m_time).
            SetEase(m_ease).
            SetLink(gameObject).
            SetLoops(-1, LoopType.Yoyo).
            SetUpdate(false)
            ;
    }
}

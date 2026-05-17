using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyTargetOverAnimeController : MonoBehaviour
{
    [SerializeField]
    private FixedChallengeEventController m_targetController = null;

    [Header("拡大するオブジェクトTransform")]
    [SerializeField]
    private Transform m_scaleTransform = null;

    [Header("以下　DOTweenの細かい設定")]
    [SerializeField]
    private float m_duration = 2.0f;
    [SerializeField]
    private int m_rotateCount = 5;
    [SerializeField]
    private float m_jumpPower = 10.0f;
    [SerializeField]
    private float m_scalePower = 1.3f;


    // Update is called once per frame
    void Update()
    {
        if (m_targetController == null) return;

        if (m_targetController.IsOver)
        {
            StartAnime();

            // 一度アニメーションが再生されたら、
            // 更新処理を停止する
            enabled = false;
        }
    }

    public void StartAnime()
    {
        // 回転イージング
        transform.DORotate(new Vector3(0, 360 * m_rotateCount, 0), m_duration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutQuart)
            .SetLink(gameObject)
            .SetUpdate(false);

        // 拡大イージング
        if (m_scaleTransform != null)
        {
            m_scaleTransform.DOScale(new Vector3(m_scalePower, m_scalePower, m_scalePower), m_duration / 2)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutBack)
                .SetLink(gameObject)
                .SetUpdate(false);

            // 少しだけ上に移動
            m_scaleTransform.DOJump(m_scaleTransform.position, m_jumpPower, 1, m_duration)
                .SetEase(Ease.OutBack)
                .SetLink(gameObject)
                .SetUpdate(false);
        }
    }

    private void Start()
    {
        if (m_targetController == null)
        {
            m_targetController = transform.parent.GetComponent<FixedChallengeEventController>();
        }
    }
}

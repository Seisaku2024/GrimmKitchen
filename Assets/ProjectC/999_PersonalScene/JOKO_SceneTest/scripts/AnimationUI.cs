using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatioUI : MonoBehaviour
{
    private const float DURATION = 1.0f;

    void Start()
    {
        Image[]circles = GetComponentsInChildren<Image>();

        for (var i = 0; i < circles.Length; i++)
        {
            Sequence sequence = DOTween.Sequence()
                .SetLoops(-1, LoopType.Restart)
                .SetDelay((DURATION / 2) * ((float)i / circles.Length))
                .Append(circles[i].rectTransform.DOAnchorPosY(10f, DURATION / 4))
                .Append(circles[i].rectTransform.DOAnchorPosY(0f, DURATION / 4))
                .AppendInterval((DURATION / 2) * ((float)(1 - i) / circles.Length));
            sequence.Play();
        }

    }
}

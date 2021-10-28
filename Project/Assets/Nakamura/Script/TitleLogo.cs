using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class TitleLogo : MonoBehaviour
{
    [SerializeField] private Image[] _titleCharImages;
    [SerializeField] private Image _pressSpaceImage;

    private const float DURATION = 1f;

    void Start()
    {
        for (var i = 0; i < _titleCharImages.Length; i++)
        {
            var endPosition = _titleCharImages[i].transform.position;
            endPosition.y = _titleCharImages[i].transform.position.y - 20.0f;

            _titleCharImages[i].transform.DOMove(endPosition, 3f)
                .SetDelay((DURATION / 4) * ((float)i))
                .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuart);
            //Sequence seq1 = DOTween.Sequence()
            //     .SetLoops(-1, LoopType.Yoyo)
            //     .SetDelay((DURATION / _titleCharImages.Length) * ((float)i / _titleCharImages.Length))
            //     .Append(_titleCharImages[i].rectTransform.DOAnchorPosY(endY, DURATION / 2));
            //    seq1.Play();     
        }

        //PressSpaceのアニメーション
        _pressSpaceImage.DOFade(0.0f, 3.0f).SetEase(Ease.InBack).SetLoops(-1, LoopType.Yoyo);
    }
}
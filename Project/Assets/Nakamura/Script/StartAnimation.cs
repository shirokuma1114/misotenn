using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartAnimation : MonoBehaviour
{
    [SerializeField] private Image _bg;
    [SerializeField] private Image[] _circles;
    [SerializeField] private Image wave;
    [SerializeField] private Image line;

    private bool isOn;
    private Sequence sequence;
    private int _nowCircleNum;
    
    private const float duration = 300f;

    void Start()
    {
        ColorCircleSpread(true);

        var step = new Vector2(1, 1);
        //Wave
        DOTween.Sequence()
        .Append(wave.rectTransform.DOAnchorPosY(-1320f, 0))
        .AppendCallback(() =>
        {
            wave.material.DOOffset(step, duration).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        });

        DOTween.Sequence()
       .Append(line.rectTransform.DOScaleY(10, 1.0f))
       .AppendCallback(() =>
        {
              
        })
        .Append(line.rectTransform.DOScaleY(0, 0.2f));
    }

    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space))
      {
           ColorCircleSpread(false);
      }
      if (Input.GetKeyDown(KeyCode.B))
      {
           ColorCircleSpread(true);
      }
    }



    //d‚È‚Á‚½‰~‚ð•Â‚¶‚½‚èŠJ‚¢‚½‚è
    void ColorCircleSpread(bool isSpread)
    {
        //L‚ª‚é“®‚«
        if (isSpread)
        {
            var circleSize = 2500f;

            sequence = DOTween.Sequence()
                .Append(_circles[_nowCircleNum].rectTransform.DOSizeDelta(Vector2.one * circleSize, 0.5f))
                .OnComplete(() =>
                {
                    if (_nowCircleNum < _circles.Length)
                    {
                        ColorCircleSpread(true);
                        _nowCircleNum += 1;
                    }
                });
        }
        //‹·‚Ü‚é“®‚«
        else
        {
            var circleSize = 100.0f;

            sequence = DOTween.Sequence()
                .Append(_circles[_nowCircleNum - 1].rectTransform.DOSizeDelta(Vector2.one * circleSize, 0.5f))
                .OnComplete(() =>
                {
                    if (_nowCircleNum > 0)
                    {
                        ColorCircleSpread(false);
                        _nowCircleNum -= 1;
                    }
                });
        }
    }
}
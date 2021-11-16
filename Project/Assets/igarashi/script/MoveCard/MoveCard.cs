using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCard : MonoBehaviour
{
    private int _index;
    private Sequence _startAnimSequence;

    MoveCardManager _manager;
    private Sequence _finAnimSequence = null;

    private bool _startAnimComplete = false;
    public bool IsStartAnimComplete => _startAnimComplete;

    public void OnClick()
    {
        //transform.parent.gameObject.GetComponent<MoveCardManager>().IndexSelect(_index);
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void SetMoveTargetPos(Vector3 targetPos, bool last)
    {
        var rt = GetComponent<RectTransform>();
        _startAnimSequence = DOTween.Sequence();

        if (!last)
        {
            _startAnimSequence.Join(rt.DOMove(targetPos, 1.0f));
            _startAnimSequence.Join(rt.DORotate(new Vector3(0, 0, 0), 1.0f));          
        }
        else
        {
            _startAnimSequence.AppendInterval(1.0f);
            _startAnimSequence.Append(rt.DOMove(targetPos, 1.0f));
            _startAnimSequence.Join(rt.DORotate(new Vector3(0, 0, -450), 1.0f, RotateMode.WorldAxisAdd));
        }
        _startAnimSequence.Play();
        _startAnimSequence.SetAutoKill(false);
    }

    public void PlayFinishAnimation()
    {
        var rt = GetComponent<RectTransform>();

        _finAnimSequence = DOTween.Sequence();

        _finAnimSequence.Append(rt.DOMove(new Vector3(Screen.width / 2, Screen.height / 2, 0), 1.0f));
        _finAnimSequence.Append(rt.DORotate(new Vector3(0, 0, -360 * 2), 2.0f, RotateMode.WorldAxisAdd));
        _finAnimSequence.Join(rt.DOScale(new Vector3(0, 0, 0), 2.0f));

        _finAnimSequence.Play();
        _finAnimSequence.SetAutoKill(false);
    }

    //================================================

    void Start()
    {
        _manager = FindObjectOfType<MoveCardManager>();
    }

    void Update()
    {
        if(_finAnimSequence != null)
        {
            if (_finAnimSequence.IsComplete())
            {
                _manager.FinAnimEnd();

                _finAnimSequence.Kill();
                _finAnimSequence = null;
            }
        }


        if (_startAnimSequence != null)
        {
            if(_startAnimSequence.IsComplete())
            {
                _startAnimComplete = true;

                _startAnimSequence.Kill();
                _startAnimSequence = null;
            }
        }
    }

    private void OnDisable()
    {
        if (_startAnimSequence != null)
        {
            _startAnimSequence.Kill();
        }
        if (_finAnimSequence != null)
        {
            _finAnimSequence.Kill();
        }
    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCard : MonoBehaviour
{
    private int _index;
    private List<Tween> _tweens = new List<Tween>();

    MoveCardManager _manager;
    private Sequence _finAnimSequence = null;

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


        if (!last)
        {
            _tweens.Add(rt.DOMove(targetPos, 1.0f));
            _tweens.Add(rt.DORotate(new Vector3(0, 0, 0), 1.0f));
        }
        else
        {
            var seq = DOTween.Sequence();
            _tweens.Add(seq.AppendInterval(1.0f));
            _tweens.Add(seq.Append(rt.DOMove(targetPos, 1.0f)));
            _tweens.Add(seq.Join(rt.DORotate(new Vector3(0, 0, -450), 1.0f, RotateMode.WorldAxisAdd)));

            seq.Play();
        }
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
            if (!_finAnimSequence.IsPlaying())
            {
                _manager.FinAnimEnd();
                _finAnimSequence.Kill();
                _finAnimSequence = null;
            }
        }
    }

    private void OnDisable()
    {
        if (_tweens.Count != 0)
        {
            for (int i = 0; i < _tweens.Count; i++)
                _tweens[i].Kill();
        }
        if (_finAnimSequence != null)
        {
            _finAnimSequence.Kill();
        }
    }   
}

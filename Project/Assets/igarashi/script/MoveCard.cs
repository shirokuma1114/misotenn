using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCard : MonoBehaviour
{
    private int _index;
    private Tween _tween;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (DOTween.instance != null)
        {
            _tween.Kill();
        }
    }

    //=================================
    //public
    //=================================
    public void OnClick()
    {
        transform.parent.gameObject.GetComponent<MoveCardManager>().IndexSelect(_index);
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public void SetMoveTargetPos(Vector3 targetPos,bool last)
    {
        var rt = GetComponent<RectTransform>();


        if (!last)
        {
            _tween = rt.DOMove(targetPos, 1.0f);
            rt.DORotate(new Vector3(0, 0, 0), 1.0f);
        }
        else
        {
            var seq = DOTween.Sequence();
            seq.AppendInterval(1.0f);
            seq.Append(_tween = rt.DOMove(targetPos, 1.0f));
            seq.Join(rt.DORotate(new Vector3(0, 0, -450), 1.0f, RotateMode.WorldAxisAdd));

            seq.Play();
        }
    }

    //=================================
    
}

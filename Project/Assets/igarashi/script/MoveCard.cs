using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCard : MonoBehaviour
{
    private int _index;

    private Vector3 _moveTargetPos;

    private Tween _tween;

    // Start is called before the first frame update
    void Start()
    {
        var rt = GetComponent<RectTransform>();
        _tween = rt.DOMove(_moveTargetPos, 1.0f);
        rt.DORotate(new Vector3(0, 0, 0), 1.0f);
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

    public void SetMoveTargetPos(Vector3 targetPos)
    {
        _moveTargetPos = targetPos;
    }

    //=================================
    
}

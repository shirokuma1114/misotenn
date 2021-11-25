using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StealEffect : MonoBehaviour
{
    private Sequence _sequence;

    [SerializeField]
    private Vector3 _maxScale;

    private bool _animEnd;
    public bool IsAnimEnd => _animEnd;


    public void Init(Vector3 startPos,Vector3 startUp,Vector3 endPos,Vector3 endUp,Sprite souvenirSprite)
    {
        transform.localScale = new Vector3(0, 0, 0);
        transform.forward = new Vector3(0,0,1);


        Vector3[] path = new Vector3[4];

        path[0] = startPos;
        path[1] = startPos + startUp * 0.5f;
        path[2] = endPos + endUp * 0.5f;
        path[3] = endPos;


        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOScale(_maxScale, 2.5f));
        _sequence.Join(transform.DOPath(path, 5.0f,PathType.CatmullRom));
        _sequence.Join(transform.DORotate(new Vector3(0, 720.0f, 0), 5.0f, RotateMode.LocalAxisAdd)).SetEase(Ease.Linear) ;
        _sequence.Join(transform.DOScale(new Vector3(0, 0, 0), 2.5f).SetDelay(2.5f));

        _sequence.Play();

        _sequence.OnComplete
            ( () => { _animEnd = true;} );


        GetComponent<MeshRenderer>().materials[0].SetTexture("_FrontTex", souvenirSprite.texture);
        GetComponent<MeshRenderer>().materials[0].SetTexture("_BuckTex", souvenirSprite.texture);
    }

    //===========================


    void Start()
    {
        _animEnd = false;
    }

    void Update()
    {
    }

    private void OnDisable()
    {
        if (_sequence != null)
            _sequence.Kill();
    }
}

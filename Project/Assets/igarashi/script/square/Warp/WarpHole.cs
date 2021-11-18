using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using DG.Tweening;

public class WarpHole : MonoBehaviour
{
    public enum WarpHoleState
    {
        IDLE,

        BLACK_OPEN,
        BLACK_CLOSE,
        WHITE_OPEN,
        WHITE_CLOSE,
    }
    private WarpHoleState _state;
    public WarpHoleState State => _state;

    private GameObject _black;
    private GameObject _white;

    [SerializeField]
    private float _maxScale = 5.0f;
    [SerializeField]
    private float _scaleChangeSpeed = 0.5f;


    public void Out()
    {
        _state = WarpHoleState.WHITE_OPEN;
    }

    public void Close()
    {
        _state = WarpHoleState.BLACK_CLOSE;
    }

   //===========================

    private void Awake()
    {
        _black = transform.Find("Black").gameObject;
        _white = transform.Find("White").gameObject;
    }

    void Start()
    {
        _state = WarpHoleState.BLACK_OPEN;
    }

    void Update()
    {
        switch(_state)
        {
            case WarpHoleState.IDLE:
                break;


            case WarpHoleState.BLACK_OPEN:
                BlackOpenStateProcess();
                break;            
            case WarpHoleState.BLACK_CLOSE:
                BlackCloseStateProcess();
                break;

            case WarpHoleState.WHITE_OPEN:
                WhiteOpenStateProcess();
                break;
            case WarpHoleState.WHITE_CLOSE:
                WhiteCloseStateProcess();
                break;
        }
    }

    private void BlackOpenStateProcess()
    {
        _black.transform.localScale += new Vector3(1, 1, 1) * _scaleChangeSpeed * Time.deltaTime;

        if(_black.transform.localScale.x >= _maxScale)
        {
            _black.transform.localScale = new Vector3(_maxScale,_maxScale,_maxScale);
            _state = WarpHoleState.BLACK_CLOSE;
        }
    }

    private void BlackCloseStateProcess()
    {
        _black.transform.localScale -= new Vector3(1, 1, 1) * _scaleChangeSpeed * Time.deltaTime;

        if(_black.transform.localScale.x <= 0)
        {
            _black.SetActive(false);
            _white.SetActive(true);

            _state = WarpHoleState.WHITE_OPEN;
        }
    }

    private void WhiteOpenStateProcess()
    {
        _white.transform.localScale += new Vector3(1, 1, 1) * _scaleChangeSpeed * Time.deltaTime;

        if (_white.transform.localScale.x >= _maxScale)
        {
            _white.transform.localScale = new Vector3(_maxScale, _maxScale, _maxScale);
            _state = WarpHoleState.WHITE_CLOSE;
        }
    }

    private void WhiteCloseStateProcess()
    {
        _white.transform.localScale -= new Vector3(1, 1, 1) * _scaleChangeSpeed * Time.deltaTime;

        if (_white.transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }
    }

}

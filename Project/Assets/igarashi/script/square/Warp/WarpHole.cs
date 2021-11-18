using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using DG.Tweening;

public class WarpHole : MonoBehaviour
{
    private enum WarpHoleState
    {
        OPEN,
        IDEL,
        CLOSE,
    }
    private WarpHoleState _state;

    [SerializeField]
    private float _maxScale = 5.0f;
    [SerializeField]
    private float _scaleChangeSpeed = 0.5f;


    public void Close()
    {
        _state = WarpHoleState.CLOSE;
    }

   //===========================

    private void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        switch(_state)
        {
            case WarpHoleState.OPEN:
                OpenStateProcess();
                break;
            case WarpHoleState.IDEL:
                break;
            case WarpHoleState.CLOSE:
                CloseStateProcess();
                break;
        }
    }

    private void OpenStateProcess()
    {
        transform.localScale += new Vector3(1, 1, 1) * _scaleChangeSpeed * Time.deltaTime;

        if(transform.localScale.x >= _maxScale)
        {
            transform.localScale = new Vector3(_maxScale,_maxScale,_maxScale);
            _state = WarpHoleState.IDEL;
        }
    }

    private void CloseStateProcess()
    {
        transform.localScale -= new Vector3(1, 1, 1) * _scaleChangeSpeed * Time.deltaTime;

        if(transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }
    }
}

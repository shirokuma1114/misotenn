using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProtectEffect : MonoBehaviour
{
    private bool _end;
    public bool IsEnd => _end;

    private BarrierDissolve _effect;

    [SerializeField]
    private float _playTime = 1.0f;
    private float _playTimeCounter = 0.0f;

    private Tween _tween;

    
    public void EndEffect()
    {
        
    }

    //================================

    void Start()
    {
        _end = false;

        _effect = GetComponent<BarrierDissolve>();
        _effect.StartBarrier();
    }

    private void Update()
    {
        if (_playTimeCounter >= _playTime)
        {
            _tween = transform.DOScale(0.0f, 0.5f);
            _tween.OnComplete(() => { _end = true; });
            _tween.SetLink(gameObject);
        }

        _playTimeCounter += Time.deltaTime;
    }

    private void OnDestroy()
    {
        
    }
}

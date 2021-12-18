using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProtectEffect : MonoBehaviour
{
    private bool _end;
    public bool IsEnd => _end;

    private BarrierDissolve _effect;

    private GameObject _subEffect;

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

        _subEffect = transform.Find("SubParticle").gameObject;
    }

    private void FixedUpdate()
    {
        if (!transform.parent)
            return;

        Vector3 localScale = transform.localScale;
        Vector3 lossScale = transform.lossyScale;
        float characterMinScale = Mathf.Min(transform.parent.lossyScale.x, Mathf.Min(transform.parent.lossyScale.y, transform.parent.lossyScale.z));

        transform.localScale = new Vector3(
                localScale.x / lossScale.x * characterMinScale / 10.0f,
                localScale.y / lossScale.y * characterMinScale / 10.0f,
                localScale.z / lossScale.z * characterMinScale / 10.0f
        );
    }

    private void Update()
    {
        if (_end)
            return;

        if (_playTimeCounter >= _playTime)
        {
            _subEffect.SetActive(false);
            _end = true;
        }

        _playTimeCounter += Time.deltaTime;
    }

    private void OnDestroy()
    {
        
    }
}

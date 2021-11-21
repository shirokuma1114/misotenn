using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ProtectEffect : MonoBehaviour
{
    private bool _end;
    public bool IsEnd => _end;

    private MeshRenderer _mesh;

    private Sequence _sequence;

    private void Awake()
    {
        _end = false;

        _mesh = GetComponent<MeshRenderer>();        

        transform.localScale = new Vector3(0, 0, 0);
    }

    void Start()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOScale(50.0f, 1.0f));
        _sequence.AppendInterval(1.0f);
        _sequence.Append(transform.DOScale(100.0f, 1.0f));
        _sequence.Join(_mesh.material.DOColor(new Color(0,0,0,0), "_FadeAlpha", 1.0f));

        _sequence.Play();

        _sequence.SetAutoKill(false);
    }


    void FixedUpdate()
    {
        if (_sequence == null)
            return;

        if(_sequence.IsComplete())
        {
            _end = true;

            _sequence.Kill();
            _sequence = null;
        }
    }
}

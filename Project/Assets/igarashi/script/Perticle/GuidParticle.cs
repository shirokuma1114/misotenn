using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class GuidParticle : MonoBehaviour
{
    private EarthMove _earth;

    private SquareBase _startSquare;
    private Vector3 _startSquarePos;
    private SquareBase _nextSquare;
    private Vector3 _nextSquarePos;
    private float _lerpCounter;
    private float _angle;
    private int _moveCounter;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private int _moveNum;
    [SerializeField]
    private float _floatingVolume = 0.3f;


    public void InitStartSquare(SquareBase square)
    {
        _startSquare = square;
        _nextSquare = _startSquare.OutConnects[0];

        _startSquarePos = _startSquare.transform.localPosition;
        _nextSquarePos = _nextSquare.transform.localPosition;

        _startSquarePos += _startSquarePos.normalized * _floatingVolume;
        _nextSquarePos += _nextSquarePos.normalized * _floatingVolume;
        _angle = Vector3.Angle(_startSquarePos, _nextSquarePos);
    }

    //============================-

    private void Awake()
    {
        _lerpCounter = 0.0f;
        _moveCounter = 0;
    }

    void Start()
    {
        _earth = FindObjectOfType<EarthMove>();

        var psMain = GetComponent<ParticleSystem>().main;
        psMain.simulationSpace = ParticleSystemSimulationSpace.Custom;
        psMain.customSimulationSpace = _earth.transform;
    }

    void Update()
    {
        if(_moveCounter > _moveNum)
        {
            Destroy(gameObject);
        }
        else if (_lerpCounter >= 1.0f)
        {
            _lerpCounter = 0.0f;

            _startSquare = _nextSquare;
            _nextSquare = _startSquare.OutConnects[0];

            _startSquarePos = _startSquare.transform.localPosition;
            _nextSquarePos = _nextSquare.transform.localPosition;

            _startSquarePos += _startSquarePos.normalized * _floatingVolume;
            _nextSquarePos += _nextSquarePos.normalized * _floatingVolume;
            _angle = Vector3.Angle(_startSquarePos, _nextSquarePos);

            _moveCounter++;
        }
        else
        {
            transform.position = _earth.transform.TransformPoint(Vector3.Slerp(_startSquarePos, _nextSquarePos, _lerpCounter));
            _lerpCounter += 1 / _angle * _speed * Time.deltaTime;
        }
    }
}

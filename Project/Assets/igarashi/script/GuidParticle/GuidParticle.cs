using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class GuidParticle : MonoBehaviour
{
    private EarthMove _earth;
    private SquareConnectionLine _line;

    private int _startIndex;
    private int _moveIndex;

    private Vector3 _start;
    private Vector3 _end;
    private float _distance;

    private float _lerpCounter;
    private int _moveCounter;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private int _moveNum;


    public void InitStartSquare(SquareBase square,EarthMove earth)
    {
        _earth = earth;
        _line = _earth.GetComponent<SquareConnectionLine>();

        _moveIndex = _startIndex = _line.GetSquareStartIndex[square.gameObject];
    }

    //============================-

    private void Awake()
    {
        _lerpCounter = 0.0f;
        _moveCounter = 0;
    }

    void Start()
    {
        var psMain = GetComponent<ParticleSystem>().main;
        psMain.simulationSpace = ParticleSystemSimulationSpace.Custom;
        psMain.customSimulationSpace = _earth.transform;
    }

    void Update()
    {
        if(_moveCounter >= _moveNum * _line.CurvePointNum)
        {
            var ps = GetComponent<ParticleSystem>();
            ps.Stop();
        }
        else if (_lerpCounter >= 1.0f)
        {
            _lerpCounter = 0.0f;

            _moveIndex++;
            _moveIndex %= _line.GetPositions.Count;

            _start = _line.GetPositions[_moveIndex];
            _end = _line.GetPositions[_moveIndex % _line.GetPositions.Count];
            _distance = Vector3.Distance(_start, _end);

            _moveCounter++;
        }
        else
        {
            transform.position = _earth.transform.TransformPoint(Vector3.Lerp(_start, _end, _lerpCounter));
            _lerpCounter += 1 / _distance * _speed * Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPositions : MonoBehaviour
{
    private List<Vector3> _positions = new List<Vector3>();
    private GameObject _earth;
    private int _index;

    private Vector3 _start;
    private Vector3 _end;
    private float _distance;

    private float _lerpCount;

    private void Awake()
    {
        _index = 0;
        _lerpCount = 0.0f;
    }

    void Start()
    {
        _earth = FindObjectOfType<SquareConnectionLine>().gameObject;
        _positions = _earth.GetComponent<SquareConnectionLine>().GetPositions();

        _start = _positions[_index];
        _end = _positions[_index + 1];
        _distance = Vector3.Distance(_start, _end);
    }

    private void Update()
    {
        if (_lerpCount >= 1.0f)
        {
            _lerpCount = 0.0f;

            _index++;
            _index %= _positions.Count;

            _start = _positions[_index];
            _end = _positions[_index % _positions.Count];
            _distance = Vector3.Distance(_start, _end);
        }
        else
        {
            transform.position = _earth.transform.TransformPoint(Vector3.Lerp(_start, _end, _lerpCount));
            _lerpCount += 50.0f * Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthMove : MonoBehaviour
{
    public enum EarthMoveState
    {
        IDLE,
        MOVE_INIT,
        MOVE,
        END,
    }
    private EarthMoveState _state = EarthMoveState.IDLE;
    public EarthMoveState State => _state;


    //合わせる視点
    [SerializeField]
    private GameObject _camera = null;

    //回転
    private Vector3 _targetPosition;
    private Quaternion _startRot;
    private Quaternion _endRot;
    private float _lerpTime = 0;
    private float _angle;
    private float _rotationSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        _targetPosition = Vector3.zero;
        _startRot = transform.rotation;
        _endRot = Quaternion.identity;
        _lerpTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case EarthMoveState.IDLE:
                IdleStateProcess();
                break;
            case EarthMoveState.MOVE_INIT:
                MoveInitStateProcess();
                break;
            case EarthMoveState.MOVE:
                MoveStateProcess();
                break;
            case EarthMoveState.END:
                EndStateProcess();
                break;
        }
    }


    //=================================
    //public
    //=================================
    public void MoveToPosition(Vector3 position,float rotSpeed = 100.0f)    //ワールド座標
    {
        _state = EarthMoveState.MOVE_INIT;

        _targetPosition = position;
        _rotationSpeed = rotSpeed;
    }

    //=================================


    //private
    private void IdleStateProcess()
    {

    }

    private void MoveInitStateProcess()
    {
        Vector3 xzCamera = new Vector3(_camera.transform.position.x,0.0f, _camera.transform.position.z).normalized;
        Vector3 xzTarget = new Vector3(_targetPosition.x,0.0f,_targetPosition.z).normalized;
        float yAngle = Vector3.Angle(xzCamera, xzTarget);
        //Vector3.a
        _endRot = Quaternion.AngleAxis(yAngle, Vector3.up);

        //Vector3 refVec = (_camera.transform.position - transform.position).normalized;
        //Vector3 vec = (_targetPosition - transform.position).normalized;
        //_endRot = Quaternion.FromToRotation(vec, refVec);

        _lerpTime = 0.0f;

        _startRot = transform.rotation;

        _angle = Quaternion.Angle(_endRot,_startRot);

        _state = EarthMoveState.MOVE;
    }

    private void MoveStateProcess()
    {
        transform.rotation = Quaternion.Lerp(_startRot, _endRot, _lerpTime);


        if (_lerpTime >= 1.0f)
            _state = EarthMoveState.END;


        _lerpTime += 1 / _angle * _rotationSpeed * Time.deltaTime;
    }

    private void EndStateProcess()
    {
        _state = EarthMoveState.IDLE;
    }
}

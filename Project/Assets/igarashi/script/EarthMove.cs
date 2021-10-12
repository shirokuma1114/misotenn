using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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


    //��]
    private Vector3 _targetPosition;
    private Vector3 _prevTargetPosition;
    private Quaternion _startRot;
    private Quaternion _endRot;
    private float _lerpTime = 0;
    private float _angle;
    private float _rotationSpeed = 100.0f;
    private float _yAngle;

    // Start is called before the first frame update
    void Start()
    {
        _targetPosition = Vector3.zero;
        _startRot = transform.rotation;
        _endRot = Quaternion.identity;
        _lerpTime = 0;
        _prevTargetPosition = -transform.forward * 10.0f;
        _yAngle = 0.0f;
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
    public void MoveToPosition(Vector3 target,float rotSpeed = 100.0f)    //���[���h���W
    {
        _state = EarthMoveState.MOVE_INIT;

        _targetPosition = target;
        _rotationSpeed = rotSpeed;
    }

    //=================================


    //private
    private void IdleStateProcess()
    {

    }

    private void MoveInitStateProcess()
    {
        //�c��]
        Vector3 xzTargetPos = new Vector3(_targetPosition.x, 0.0f, _targetPosition.z);
        float angle = Vector3.SignedAngle(xzTargetPos, _targetPosition, Vector3.Cross(_targetPosition,-Vector3.up));
        _endRot = Quaternion.AngleAxis(angle, Vector3.Cross(_targetPosition, Vector3.up));

        //����]
        Vector3 xzPrevTarget = new Vector3(_prevTargetPosition.x, 0.0f, _prevTargetPosition.z).normalized;
        Vector3 xzTarget = new Vector3(_targetPosition.x, 0.0f, _targetPosition.z).normalized;
        float xzAngle = Vector3.SignedAngle(xzPrevTarget, xzTarget, -transform.up);
        _yAngle += xzAngle;
        _endRot = Quaternion.Euler(0.0f,_yAngle,0.0f) * _endRot;

        _lerpTime = 0.0f;
        _startRot = transform.rotation;
        _angle = Quaternion.Angle(_endRot,_startRot);
        _prevTargetPosition = _targetPosition;


        _state = EarthMoveState.MOVE;
    }

    private void MoveStateProcess()
    {
        transform.rotation = Quaternion.Lerp(_startRot, _endRot, _lerpTime);


        if (_lerpTime >= 1.0f)
        {
            _state = EarthMoveState.END;
            return;
        }


        _lerpTime += 1 / _angle * _rotationSpeed * Time.deltaTime;
    }

    private void EndStateProcess()
    {
        _state = EarthMoveState.IDLE;
    }
}

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


    //回転
    private Vector3 _targetPosition;
    private Vector3 _prevTargetPosition;
    private Quaternion _startRot;
    private Quaternion _endRot;
    private float _lerpTime = 0;
    private float _angle;
    private float _rotationSpeed = 100.0f;
    private float _yAngle;


    /// <summary>
    /// TargetをVector3(0,0,-1)方向へ合わせるように地球を回転させる
    /// </summary>
    /// <param name="target">合わせる方向</param>
    /// <param name="rotSpeed">回転速度</param>
    public void MoveToPosition(Vector3 target,float rotSpeed = 100.0f)    //ワールド座標
    {
        _targetPosition = target;
        _rotationSpeed = rotSpeed;

        _state = EarthMoveState.MOVE_INIT;
    }

    /// <summary>
    /// MoveToPositionを一瞬でLerpなしで実行する
    /// </summary>
    /// <param name="target"></param>
    public void MoveToPositionInstant(Vector3 target)
    {
        _targetPosition = target;


        //縦回転
        Vector3 xzTargetPos = new Vector3(_targetPosition.x, 0.0f, _targetPosition.z);
        float angle = Vector3.SignedAngle(xzTargetPos, _targetPosition, Vector3.Cross(_targetPosition, -Vector3.up));
        _endRot = Quaternion.AngleAxis(angle, Vector3.Cross(_targetPosition, Vector3.up));

        //横回転
        Vector3 xzPrevTarget = new Vector3(_prevTargetPosition.x, 0.0f, _prevTargetPosition.z).normalized;
        Vector3 xzTarget = new Vector3(_targetPosition.x, 0.0f, _targetPosition.z).normalized;
        float xzAngle = Vector3.SignedAngle(xzPrevTarget, xzTarget, -transform.up);
        _yAngle += xzAngle;
        _endRot = Quaternion.Euler(0.0f, _yAngle, 0.0f) * _endRot;

        transform.rotation = _endRot;
        _prevTargetPosition = _targetPosition;


        _state = EarthMoveState.END;
    }


    //=================================================================

    void Awake()
    {
        _targetPosition = Vector3.zero;
        _startRot = transform.rotation;
        _endRot = Quaternion.identity;
        _lerpTime = 0;
        _yAngle = 0.0f;
    }

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


    private void IdleStateProcess()
    {

    }

    private void MoveInitStateProcess()
    {
        //縦回転
        Vector3 xzTargetPos = new Vector3(_targetPosition.x, 0.0f, _targetPosition.z);
        float angle = Vector3.SignedAngle(xzTargetPos, _targetPosition, Vector3.Cross(_targetPosition,-Vector3.up));
        _endRot = Quaternion.AngleAxis(angle, Vector3.Cross(_targetPosition, Vector3.up));
        
        //横回転
        Vector3 xzPrevTarget = new Vector3(_prevTargetPosition.x, 0.0f, _prevTargetPosition.z);
        Vector3 xzTarget = new Vector3(_targetPosition.x, 0.0f, _targetPosition.z);
        float xzAngle = Vector3.SignedAngle(xzPrevTarget, xzTargetPos, -Vector3.up);
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

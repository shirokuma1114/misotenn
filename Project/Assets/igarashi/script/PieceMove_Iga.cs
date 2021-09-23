using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMove_Iga : MonoBehaviour
{
    public enum PieceMoveState
    {
        Wait,
        Move,
    }
    private PieceMoveState _state = PieceMoveState.Wait;

    public Vector3 _targetPos = new Vector3(0, 0, 0);
    private Vector3 _startPos = new Vector3(0, 0, 0);
    private float _lerpTime;

    // Start is called before the first frame update
    void Start()
    {
        _state = PieceMoveState.Wait;
    }

    // Update is called once per frame
    void Update()
    {
        switch(_state)
        {
            case PieceMoveState.Wait:
                if (Input.GetKeyDown(KeyCode.Space)) MoveStart();
                break;

            case PieceMoveState.Move:
                MoveStateProcess();
                break;
        }
    }

    private void MoveStateProcess()
    {
        float lerpX = Mathf.Lerp(_startPos.x, _targetPos.x, _lerpTime);
        float lerpY = Mathf.Lerp(_startPos.y, _targetPos.y, _lerpTime);
        float lerpZ = Mathf.Lerp(_startPos.z, _targetPos.z, _lerpTime);

        transform.position = new Vector3(lerpX, lerpY, lerpZ);


        if (_lerpTime > 1.0f)
            _state = PieceMoveState.Wait;


        _lerpTime += Time.deltaTime;
    }


    public void SetTargetPosition(Vector3 pos)
    {
        _targetPos = pos;
    }

    public void MoveStart()
    {
        _state = PieceMoveState.Move;

        _startPos = transform.position;
        _lerpTime = 0;
    }
}

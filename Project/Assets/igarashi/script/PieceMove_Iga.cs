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

    private Vector3 _targetPos = new Vector3(0, 0, 100);

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
                
                break;

            case PieceMoveState.Move:
                MoveStateProcess();
                break;
        }
    }

    private void MoveStateProcess()
    {
        Vector3 dir = _targetPos - transform.position;
        dir.Normalize();

        transform.position += dir * 0.5f;

        if (transform.position == _targetPos)
            _state = PieceMoveState.Wait;
    }


    public void SetTargetPosition(Vector3 pos)
    {
        _targetPos = pos;
    }

    public void MoveStart()
    {
        _state = PieceMoveState.Move;
    }
}

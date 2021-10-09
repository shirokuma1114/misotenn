using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMoveForward : SquareBase
{
    public enum SquareMoveForwardState
    {
        IDEL,
        MOVE,
        END,
    }
    private SquareMoveForwardState _state;
    public SquareMoveForwardState State => _state;


    CharacterBase _character;
    MessageWindow _messageWindow;
    StatusWindow _statusWindow;
    PayUI _payUI;

    [SerializeField]
    private int _cost;

    [SerializeField]
    private SquareBase _targetsSquare; 

    // Start is called before the first frame update
    void Start()
    {
        _statusWindow = FindObjectOfType<StatusWindow>();
        //_payUI = FindObjectOfType<PayUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(_state)
        {
            case SquareMoveForwardState.IDEL:
                break;
            case SquareMoveForwardState.MOVE:
                MoveStateProcess();
                break;
            case SquareMoveForwardState.END:
                break;
        }
    }


    public override void Stop(CharacterBase character)
    {
        _character = character;

        // インスタンス生成
        var message = _targetsSquare.gameObject.name + "へ移動";

        //_messageWindow.SetMessage(message);
        //_statusWindow.SetEnable(true);
        //_payUI.SetEnable(true);

        _state = SquareMoveForwardState.MOVE;
    }




    private void MoveStateProcess()
    {
        if(_character.State != CharacterState.MOVE)
        {
            Debug.Log(_character.CurrentSquare.gameObject.name);
            _character.StartMove(_character.CurrentSquare.OutConnects[0]._square);
        }


        if (_character.CurrentSquare == _targetsSquare)
            _state = SquareMoveForwardState.END;
    }
}

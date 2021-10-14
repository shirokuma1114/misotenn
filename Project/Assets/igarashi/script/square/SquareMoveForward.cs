using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareMoveForward : SquareBase
{
    public enum SquareMoveForwardState
    {
        IDLE,
        PAY,
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
    private int _moveNum; 
    private int _moveCount; 

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(_state)
        {
            case SquareMoveForwardState.IDLE:
                break;
            case SquareMoveForwardState.PAY:
                PayStateProcess();
                break;
            case SquareMoveForwardState.MOVE:
                MoveStateProcess();
                break;
            case SquareMoveForwardState.END:
                EndStateProcess();
                break;
        }
    }


    public override void Stop(CharacterBase character)
    {
        _character = character;

        _moveCount = 0;

        var message = _cost.ToString() + "â~Çéxï•Ç¡Çƒ" + _moveNum + "É}ÉXêiÇ›Ç‹Ç∑Ç©ÅH";
        _messageWindow.SetMessage(message,character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.SetEnable(true);

        _state = SquareMoveForwardState.PAY;
    }


    private void PayStateProcess()
    {
        if (_payUI.IsChoiseComplete() && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes())
            {
                _character.SubMoney(_cost);

                _state = SquareMoveForwardState.MOVE;
            }
            else
            {
                _state = SquareMoveForwardState.END;
            }

            _payUI.SetEnable(false);
        }
    }

    private void MoveStateProcess()
    {
        if (_moveNum == _moveCount && _character.State != CharacterState.MOVE)
        {
            _state = SquareMoveForwardState.END;
            return;
        }


        if (_character.State != CharacterState.MOVE)
        {
            _character.StartMove(_character.CurrentSquare.OutConnects[0]._square);
            _moveCount++;
        }
    }

    private void EndStateProcess()
    {
        // é~Ç‹ÇÈèàóùèIóπ
        _character.CompleteStopExec();
        _statusWindow.SetEnable(false);

        _state = SquareMoveForwardState.IDLE;
    }
}

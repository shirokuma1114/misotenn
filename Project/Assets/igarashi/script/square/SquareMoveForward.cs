using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    private CharacterBase _character;
    private MessageWindow _messageWindow;
    private StatusWindow _statusWindow;
    private PayUI _payUI;

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


        _squareInfo =
            "前進マス\n" +
            "コスト：" + _cost.ToString() + "\n" +
            "進むマス数";
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

        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("お金が足りません", character.IsAutomatic);
            _state = SquareMoveForwardState.END;
            return;
        }

        var message = _cost.ToString() + "円を支払って" + _moveNum + "マス進みますか？";
        _messageWindow.SetMessage(message,character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareMoveForwardState.PAY;
    }


    private void PayStateProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
                _character.SubMoney(_cost);

                _state = SquareMoveForwardState.MOVE;
            }
            else
            {
                _state = SquareMoveForwardState.END;
            }
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
            _character.StartMove(_character.CurrentSquare.OutConnects[0]);
            _moveCount++;
        }
    }

    private void EndStateProcess()
    {
        if(!_messageWindow.IsDisplayed)
        {
            // 止まる処理終了
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareMoveForwardState.IDLE;
        }        
    }

    public override int GetScore(CharacterBase character)
    {
        // 支払えるならこのマス＋移動先マスのスコア
        if (_cost > character.Money) return base.GetScore(character);

        // 移動先のマスの評価
        SquareBase square = character.CurrentSquare;
        for(int i = 0; i < _moveNum; i++) square = square.OutConnects.Last();

        // このマス分のお金を引く
        character.SubMoney(_cost);
        var score = square.GetScore(character);
        character.AddMoney(_cost);

        return score + base.GetScore(character);
    }
}

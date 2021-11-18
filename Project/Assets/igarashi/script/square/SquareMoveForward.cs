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
    private MovingCountWindow _countWindow;
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
        _countWindow = FindObjectOfType<MovingCountWindow>();
        _payUI = FindObjectOfType<PayUI>();


        _squareInfo =
            "�O�i�}�X\n" +
            "�R�X�g�F" + _cost.ToString() + "\n" +
            "�i�ރ}�X���F" + _moveNum.ToString();
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
            _messageWindow.SetMessage("����������܂���", character.IsAutomatic);
            _state = SquareMoveForwardState.END;
            return;
        }

        var message = _cost.ToString() + "�~���x������" + _moveNum + "�}�X�i�݂܂����H";
        _messageWindow.SetMessage(message,character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareMoveForwardState.PAY;

        if (character.IsAutomatic)
        {
            Invoke("SelectAutomatic", 1.5f);
        }
    }


    private void PayStateProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
                _character.SubMoney(_cost);

                _state = SquareMoveForwardState.MOVE;
                _countWindow.SetEnable(true);
                _countWindow.SetMovingCount(_moveNum - _moveCount);
            }
            else
            {
                _state = SquareMoveForwardState.IDLE;
                _character.CompleteStopExec();
            }
            _statusWindow.SetEnable(false);

        }
    }

    private void MoveStateProcess()
    {
        if (_moveNum == _moveCount && _character.State != CharacterState.MOVE)
        {
            _state = SquareMoveForwardState.IDLE;
            _countWindow.SetEnable(false);
            _character.Stop();
            return;
        }


        if (_character.State != CharacterState.MOVE)
        {
            _character.StartMove(_character.CurrentSquare.OutConnects[0]);
            _countWindow.SetMovingCount(_moveNum - _moveCount);
            _moveCount++;
        }
    }

    void SelectAutomatic()
    {
        if(_character.Money < _cost)
        {
            _payUI.AISelectNo();
            return;
        }
        _payUI.AISelectYes();
    }

    private void EndStateProcess()
    {
        if(!_messageWindow.IsDisplayed)
        {
            // �~�܂鏈���I��
            _character.CompleteStopExec();

            _state = SquareMoveForwardState.IDLE;
        }        
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // �x������Ȃ炱�̃}�X�{�ړ���}�X�̃X�R�A
        if (_cost > character.Money) return base.GetScore(character, characterType);

        // �ړ���̃}�X�̕]��
        SquareBase square = character.CurrentSquare;
        for(int i = 0; i < _moveNum; i++) square = square.OutConnects.Last();

        // ���̃}�X���̂���������
        character.SubMoney(_cost);
        var score = square.GetScore(character, characterType);
        character.AddMoney(_cost);

        return score + base.GetScore(character, characterType);
    }
}

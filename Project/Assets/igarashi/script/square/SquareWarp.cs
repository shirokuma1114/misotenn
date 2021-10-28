using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareWarp : SquareBase
{
    public enum SquareWarpState
    {
        IDLE,
        PAY,
        WARP,
        END,
    }
    private SquareWarpState _state;
    public SquareWarpState State => _state;

    private CharacterBase _character;
    private MessageWindow _messageWindow;
    private StatusWindow _statusWindow;
    private PayUI _payUI;
    private SimpleFade _fade;

    private List<SquareBase> _squares;

    private List<CharacterBase> _characters;
    private int _moveIndex;

    [SerializeField]
    private int _cost;

    // Start is called before the first frame update
    void Awake()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
        _fade = FindObjectOfType<SimpleFade>();

        _squares = new List<SquareBase>();
        _squares.AddRange(FindObjectsOfType<SquareBase>());
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareWarpState.IDLE:
                break;
            case SquareWarpState.PAY:
                PayStateProcess();
                break;
            case SquareWarpState.WARP:
                WarpStateProcess();
                break;
            case SquareWarpState.END:
                EndStateProcess();
                break;
        }
    }


    public override void Stop(CharacterBase character)
    {
        _character = character;


        //お金チェック
        if(!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("お金が足りません", character.IsAutomatic);
            _state = SquareWarpState.END;
            return;
        }

        var message = _cost.ToString() + "円を支払って全員をランダムにワープさせますか？";
        _messageWindow.SetMessage(message,character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.SetEnable(true);

        _characters = new List<CharacterBase>();
        _characters.AddRange(FindObjectsOfType<CharacterBase>());
        _moveIndex = 0;


        _state = SquareWarpState.PAY;
    }



    private void PayStateProcess()
    {
        if (_payUI.IsChoiseComplete() && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes())
            {
                _character.SubMoney(_cost);
                _characters[_moveIndex].StartMove(_squares[Random.Range(0, _squares.Count)]);

                _state = SquareWarpState.WARP;
            }
            else
            {
                _state = SquareWarpState.END;
            }

            _payUI.SetEnable(false);
        }
    }

    private void WarpStateProcess()
    {
        if(_characters[_moveIndex].State == CharacterState.WAIT)
        {
            _characters[_moveIndex].SetWaitEnable(true);

            _moveIndex++;
            if(_moveIndex == _characters.Count)
            {
                _state = SquareWarpState.END;
                return;
            }

            FindObjectOfType<EarthMove>().MoveToPositionInstant(_characters[_moveIndex].CurrentSquare.GetPosition());
            _characters[_moveIndex].SetWaitEnable(false);
            _characters[_moveIndex].StartMove(_squares[Random.Range(0, _squares.Count)]);
        }
    }

    private void EndStateProcess()
    {

        if (!_messageWindow.IsDisplayed)
        {
            _character.CompleteStopExec();
            _character.SetWaitEnable(true);
            _statusWindow.SetEnable(false);

            _state = SquareWarpState.IDLE;
        }            
    }
    public override int GetScore(CharacterBase character)
    {
        // お金が足りる
        return _cost <= character.Money ? 200 : 0;
    }
}

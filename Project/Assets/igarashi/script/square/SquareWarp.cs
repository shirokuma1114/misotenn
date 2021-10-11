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

    CharacterBase _character;
    MessageWindow _messageWindow;
    StatusWindow _statusWindow;
    PayUI _payUI;

    private List<SquareBase> _squares;

    private List<CharacterBase> _characters;
    private int _moveIndex;

    [SerializeField]
    private int _cost;

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();

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
                break;
        }
    }


    public override void Stop(CharacterBase character)
    {
        _character = character;

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
        if (_characters[_moveIndex].State == CharacterState.MOVE)
            return;

        if (_moveIndex == _characters.Count - 1)
        {         
            _state = SquareWarpState.END;
            return;
        }        

        if (_characters[_moveIndex].State == CharacterState.WAIT)
        {
            _moveIndex++;
            _characters[_moveIndex].StartMove(_squares[Random.Range(0, _squares.Count)]);
        }
    }

    private void EndStateProcess()
    {
        _statusWindow.SetEnable(false);
        _character.CompleteStopExec();

        _state = SquareWarpState.IDLE;
    }
    
}

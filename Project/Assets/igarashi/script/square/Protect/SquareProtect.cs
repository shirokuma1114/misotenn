using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareProtect : SquareBase
{
    public enum SquareProtectState
    {
        IDLE,
        PAY,
        PROTECT,
        END,
    }
    private SquareProtectState _state;
    public SquareProtectState State => _state;


    CharacterBase _character;
    MessageWindow _messageWindow;
    StatusWindow _statusWindow;
    PayUI _payUI;

    [SerializeField]
    private int _cost;

    [SerializeField]
    private int _protectTurn;


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
        switch (_state)
        {
            case SquareProtectState.IDLE:
                break;
            case SquareProtectState.PAY:
                PayStateProcess();
                break;
            case SquareProtectState.PROTECT:
                ProtectStateProcess();
                break;
            case SquareProtectState.END:
                EndStateProcess();
                break;
        }
    }



    public override void Stop(CharacterBase character)
    {
        _character = character;

        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("お金が足りません", character.IsAutomatic);
            _state = SquareProtectState.END;
            return;
        }

        var message = _cost.ToString() + "円を支払って" + _protectTurn.ToString() + "ターンの間身を守りますか？";
        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareProtectState.PAY;
    }


    private void PayStateProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
                _character.SubMoney(_cost);

                _state = SquareProtectState.PROTECT;
            }
            else
            {
                _state = SquareProtectState.END;
            }
        }
    }

    private void ProtectStateProcess()
    {
        _character.GetComponent<Protector>().ProtectStart(_protectTurn);
        _state = SquareProtectState.END;
    }

    private void EndStateProcess()
    {
        if (!_messageWindow.IsDisplayed)
        {
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareProtectState.IDLE;
        }
    }
}

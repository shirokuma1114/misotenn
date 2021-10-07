using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSouvenir : SquareBase
{
    public enum SquareState
    {
        IDLE,
        PAY_WAIT,
        EVENT,
        END,
    }
    protected SquareState _state = SquareState.IDLE;

    CharacterBase _character;
    MessageWindow _messageWindow;
    StatusWindow _statusWindow;
    PayUI _payUI;

    [SerializeField]
    private int _cost;

    [SerializeField]
    private GameObject _souvenir;


    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
    }

    public override void Stop(CharacterBase character)
    {
        _character = character;

        // インスタンス生成
        var message = _cost.ToString() + "円を支払ってお土産を買いますか？";

        _messageWindow.SetMessage(message);
        _statusWindow.SetEnable(true);
        _payUI.SetEnable(true);

        _state = SquareState.PAY_WAIT;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareState.PAY_WAIT:
                PayWaitProcess();
                break;

            case SquareState.EVENT:
                EventProcess();
                break;

            case SquareState.END:
                EndProcess();
                break;
        }
    }




    private void PayWaitProcess()
    {
        if (_payUI.IsChoiseComplete() && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes())
            {
                _state = SquareState.EVENT;
            }
            else
            {
                _state = SquareState.END;
            }

            _payUI.SetEnable(false);
        }            
    }

    private void EventProcess()
    {
        if (_character.Money < _cost)
        {
            _messageWindow.SetMessage("お金が足らなかった");
        }
        else
        {    
            _character.SubMoney(_cost);
            _character.AddSouvenir(_souvenir.GetComponent<Souvenir>());

            _messageWindow.SetMessage("こまち社長は\nお土産を手に入れた"); //_character.name + "は" + _souvenir.name + "を手に入れた"
        }
        
        _state = SquareState.END;
    }

    private void EndProcess()
    {
        // 止まる処理終了
        _character.CompleteStopExec();
        _statusWindow.SetEnable(false);

        _state = SquareState.IDLE;
    }
}

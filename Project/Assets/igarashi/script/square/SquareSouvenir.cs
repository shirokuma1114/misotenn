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
        var message = "こまち社長は\nお土産マスに　止まった！";

        _messageWindow.SetMessage(message);
        _statusWindow.SetEnable(true);
        _payUI.SetDescription(_cost.ToString() + "を支払ってお土産を買いますか？");
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

    }

    private void EventProcess()
    {

    }

    private void EndProcess()
    {

    }
}

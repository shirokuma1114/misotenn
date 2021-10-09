using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSouvenir : SquareBase
{
    public enum SquareSouvenirState
    {
        IDLE,
        PAY_WAIT,
        EVENT,
        END,
    }
    protected SquareSouvenirState _state = SquareSouvenirState.IDLE;

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

        // �C���X�^���X����
        var message = _cost.ToString() + "�~���x�����Ă��y�Y�𔃂��܂����H";

        _messageWindow.SetMessage(message);
        _statusWindow.SetEnable(true);
        _payUI.SetEnable(true);

        _state = SquareSouvenirState.PAY_WAIT;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareSouvenirState.PAY_WAIT:
                PayWaitProcess();
                break;

            case SquareSouvenirState.EVENT:
                EventProcess();
                break;

            case SquareSouvenirState.END:
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
                _state = SquareSouvenirState.EVENT;
            }
            else
            {
                _state = SquareSouvenirState.END;
            }

            _payUI.SetEnable(false);
        }            
    }

    private void EventProcess()
    {
        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("����������Ȃ�����");
        }
        else
        {    
            _character.SubMoney(_cost);
            _character.AddSouvenir(_souvenir.GetComponent<Souvenir>());

            _messageWindow.SetMessage("���܂��В���\n���y�Y����ɓ��ꂽ"); //_character.name + "��" + _souvenir.name + "����ɓ��ꂽ"
        }
        
        _state = SquareSouvenirState.END;
    }

    private void EndProcess()
    {
        // �~�܂鏈���I��
        _character.CompleteStopExec();
        _statusWindow.SetEnable(false);

        _state = SquareSouvenirState.IDLE;
    }
}

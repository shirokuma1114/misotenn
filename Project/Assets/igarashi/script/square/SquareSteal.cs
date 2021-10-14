using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSteal : SquareBase
{
    public enum SquareStealState
    {
        IDLE,

        PAY,
        SLECT_TARGET,

        END,
    }
    private SquareStealState _state;
    public SquareStealState State => _state;


    CharacterBase _character;
    MessageWindow _messageWindow;
    StatusWindow _statusWindow;
    PayUI _payUI;
    SelectUI _selectUI;

    private List<CharacterBase> _otherCharacters;

    [SerializeField]
    private int _cost;

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
        _selectUI = FindObjectOfType<SelectUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareStealState.IDLE:
                break;
            case SquareStealState.PAY:
                PayStateProcess();
                break;
            case SquareStealState.SLECT_TARGET:
                SelectTargetStateProcess();
                break;
            case SquareStealState.END:
                EndStateProcess();
                break;
        }
    }

    public override void Stop(CharacterBase character)
    {
        _character = character;


        //�����`�F�b�N
        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("����������܂���", character.IsAutomatic);
            _state = SquareStealState.END;
            return;
        }


        var message = _cost.ToString() + "�~���x�����Ă��y�Y��D���܂����H";
        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.SetEnable(true);

        _otherCharacters = new List<CharacterBase>();
        _otherCharacters.AddRange(FindObjectsOfType<CharacterBase>());
        _otherCharacters.Remove(_character.GetComponent<CharacterBase>());

        _state = SquareStealState.PAY;
    }

    private void PayStateProcess()
    {
        if (_payUI.IsChoiseComplete() && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes())
            {
                _character.SubMoney(_cost);

                List<string> names = new List<string>();
                for (int i = 0; i < _otherCharacters.Count; i++)
                    names.Add(_otherCharacters[i].Name);

                _selectUI.Open(names);
                _messageWindow.SetMessage("�N���炨�y�Y��D���܂����H", _character.IsAutomatic);

                _state = SquareStealState.SLECT_TARGET;
            }
            else
            {
                _state = SquareStealState.END;
            }

            _payUI.SetEnable(false);
        }
    }

    private void SelectTargetStateProcess()
    {
        if(_selectUI.IsComplete && !_messageWindow.IsDisplayed)
        {
            if(_otherCharacters[_selectUI.SelectIndex].Souvenirs.Count > 0)
            {
                var target = _otherCharacters[_selectUI.SelectIndex].Souvenirs[0];
                _character.AddSouvenir(target);
                _otherCharacters[_selectUI.SelectIndex].RemoveSouvenir(0);

                var message = _character.Name + "��" + _otherCharacters[_selectUI.SelectIndex].Name + "��" + target.ToString() + "�������";
                _messageWindow.SetMessage(message,_character.IsAutomatic);
            }
            else
            {
                _messageWindow.SetMessage(_otherCharacters[_selectUI.SelectIndex].Name + "�͂��y�Y�������Ă��Ȃ�����", _character.IsAutomatic);
            }

            _state = SquareStealState.END;
        }
    }

    private void EndStateProcess()
    {
        if (!_messageWindow.IsDisplayed)
        {
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareStealState.IDLE;
        }
    }
}

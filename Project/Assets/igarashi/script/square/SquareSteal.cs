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

    private List<CharacterBase> _charactersOtherThis;

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


        //Ç®ã‡É`ÉFÉbÉN
        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("Ç®ã‡Ç™ë´ÇËÇ‹ÇπÇÒ", character.IsAutomatic);
            _state = SquareStealState.END;
            return;
        }


        var message = _cost.ToString() + "â~Çéxï•Ç¡ÇƒÇ®ìyéYÇíDÇ¢Ç‹Ç∑Ç©ÅH";
        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.SetEnable(true);

        _charactersOtherThis = new List<CharacterBase>();
        _charactersOtherThis.AddRange(FindObjectsOfType<CharacterBase>());
        _charactersOtherThis.Remove(_character.GetComponent<CharacterBase>());

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
                for (int i = 0; i < _charactersOtherThis.Count; i++)
                    names.Add(_charactersOtherThis[i].Name);

                _selectUI.Open(names);
                _messageWindow.SetMessage("íNÇ©ÇÁÇ®ìyéYÇíDÇ¢Ç‹Ç∑Ç©ÅH", _character.IsAutomatic);

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
            if(_charactersOtherThis[_selectUI.SelectIndex].Souvenirs.Count > 0)
            {
                var target = _charactersOtherThis[_selectUI.SelectIndex].Souvenirs[0];
                _character.AddSouvenir(target);
                _charactersOtherThis[_selectUI.SelectIndex].RemoveSouvenir(0);

                var message = _character.Name + "ÇÕ" + _charactersOtherThis[_selectUI.SelectIndex].Name + "ÇÃ" + target.ToString() + "ÇéÊÇ¡ÇΩ";
                _messageWindow.SetMessage(message,_character.IsAutomatic);
            }
            else
            {
                _messageWindow.SetMessage(_charactersOtherThis[_selectUI.SelectIndex].Name + "ÇÕÇ®ìyéYÇéùÇ¡ÇƒÇ¢Ç»Ç©Ç¡ÇΩ", _character.IsAutomatic);
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

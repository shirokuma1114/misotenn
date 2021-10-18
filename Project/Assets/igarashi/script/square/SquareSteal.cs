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
    List<string> _selectElements;

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
        _selectElements = new List<string>();
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

        _otherCharacters = new List<CharacterBase>();
        _otherCharacters.AddRange(FindObjectsOfType<CharacterBase>());
        _otherCharacters.Remove(_character.GetComponent<CharacterBase>());

        _selectElements.Clear();

        _state = SquareStealState.PAY;
    }

    private void PayStateProcess()
    {
        if (_payUI.IsChoiseComplete() && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes())
            {
                for (int i = 0; i < _otherCharacters.Count; i++)
                    _selectElements.Add(_otherCharacters[i].Name);
                _selectElements.Add("Ç‚ÇﬂÇÈ");

                _selectUI.Open(_selectElements);
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
            if(_selectUI.SelectIndex == _otherCharacters.Count)
            {
                _state = SquareStealState.END;
                return;
            }

            if (_otherCharacters[_selectUI.SelectIndex].gameObject.GetComponent<Protector>().IsProtected)
            {
                _messageWindow.SetMessage(_otherCharacters[_selectUI.SelectIndex].Name + "ÇÕêgÇéÁÇÁÇÍÇƒÇ¢ÇÈ", _character.IsAutomatic);
                _selectUI.Open(_selectElements);

                return;
            }
            else if(_otherCharacters[_selectUI.SelectIndex].Souvenirs.Count == 0)
            {
                _messageWindow.SetMessage(_otherCharacters[_selectUI.SelectIndex].Name + "Ç®ìyéYÇéùÇ¡ÇƒÇ¢Ç»Ç¢", _character.IsAutomatic);
                _selectUI.Open(_selectElements);

                return;
            }
            else
            {
                var target = _otherCharacters[_selectUI.SelectIndex].Souvenirs[0];
                _character.AddSouvenir(target);
                _otherCharacters[_selectUI.SelectIndex].RemoveSouvenir(0);

                var message = _character.Name + "ÇÕ" + _otherCharacters[_selectUI.SelectIndex].Name + "ÇÃ" + target.ToString() + "ÇéÊÇ¡ÇΩ";
                _messageWindow.SetMessage(message, _character.IsAutomatic);

                _character.SubMoney(_cost);
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

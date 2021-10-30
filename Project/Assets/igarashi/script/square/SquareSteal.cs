using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    MyGameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
        _selectUI = FindObjectOfType<SelectUI>();
        _selectElements = new List<string>();

        _gameManager = FindObjectOfType<MyGameManager>();
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
        _payUI.Open(character);

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
                _selectElements.Add("��߂�");

                _selectUI.Open(_selectElements, _character);
                _messageWindow.SetMessage("�N���炨�y�Y��D���܂����H", _character.IsAutomatic);

                _state = SquareStealState.SLECT_TARGET;
            }
            else
            {
                _state = SquareStealState.END;
            }
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
                _messageWindow.SetMessage(_otherCharacters[_selectUI.SelectIndex].Name + "�͐g������Ă���", _character.IsAutomatic);
                _selectUI.Open(_selectElements, _character);

                return;
            }
            else if(_otherCharacters[_selectUI.SelectIndex].Souvenirs.Count == 0)
            {
                _messageWindow.SetMessage(_otherCharacters[_selectUI.SelectIndex].Name + "���y�Y�������Ă��Ȃ�", _character.IsAutomatic);
                _selectUI.Open(_selectElements, _character);

                return;
            }
            else
            {
                var target = _otherCharacters[_selectUI.SelectIndex].Souvenirs[0];
                _character.AddSouvenir(target);
                _otherCharacters[_selectUI.SelectIndex].RemoveSouvenir(0);

                var message = _character.Name + "��" + _otherCharacters[_selectUI.SelectIndex].Name + "��" + target.ToString() + "�������";
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

    public override int GetScore(CharacterBase character)
    {
        // �R�X�g������Ȃ�
        if (_cost < character.Money) return base.GetScore(character);

        // �D�����̂�����
        if (_gameManager.GetCharacters(character).Where(x => x.Souvenirs.Count > 0).Count() == 0) return (int)SquareScore.NONE_STEAL + base.GetScore(character);

        // �����ĂȂ����y�Y�������Ă���v���C���[������
        var characters = _gameManager.GetCharacters(character);

        // �����ĂȂ��J�[�h���X�g
        var dontHaveTypes = new HashSet<SouvenirType>();
        for (int i = 0; i < (int)SouvenirType.MAX_TYPE; i++)
        {
            // �J�[�h������
            if (character.Souvenirs.Where(x => x.Type == (SouvenirType)i).Count() >= 1)
            {
                dontHaveTypes.Add((SouvenirType)i);
            }
        }

        foreach(var x in characters)
        {
            foreach (var y in dontHaveTypes)
            {
                // �����Ă��Ȃ����y�Y�������Ă���
                if (x.Souvenirs.Where(z => z.Type == y).Count() > 0)
                {
                    // �����Ώ���
                    if(character.GetSouvenirTypeNum() == 5)
                    {
                        return (int)SquareScore.DONT_HAVE_SOUVENIR_TO_WIN + base.GetScore(character);
                    }
                    return (int)SquareScore.DONT_HAVE_STEAL + base.GetScore(character);
                }
            }
        }
        return (int)SquareScore.STEAL + base.GetScore(character);
    }
}

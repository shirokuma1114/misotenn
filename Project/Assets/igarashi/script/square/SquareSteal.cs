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
    private SouvenirWindow _souvenirWindow;

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
        _souvenirWindow = FindObjectOfType<SouvenirWindow>();

        _selectUI = FindObjectOfType<SelectUI>();
        _selectElements = new List<string>();

        _gameManager = FindObjectOfType<MyGameManager>();


        _squareInfo =
            "����}�X\n" +
            "�R�X�g�F" + _cost.ToString();
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

        // �N�����y�Y�������Ă��Ȃ�
        if(!_gameManager.HasSouvenirByCharacters(character))
        {
            //_messageWindow.SetMessage("�N�����y�Y�������Ă��Ȃ������I", character.IsAutomatic);
            //_state = SquareStealState.END;
            //return;
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

        // �D�������f
        if (character.IsAutomatic)
        {
            Invoke("AISelectAutomatic", 1.5f);
        }
    }

    void AISelectAutomatic()
    {
        _payUI.AISelectYes();
    }

    private void PayStateProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
                _character.Log.AddUseEventNum(SquareEventType.STEAL);

                for (int i = 0; i < _otherCharacters.Count; i++)
                    _selectElements.Add(_otherCharacters[i].Name);
                _selectElements.Add("��߂�");

                _selectUI.Open(_selectElements, _character);
                _messageWindow.SetMessage("�N���炨�y�Y��D���܂����H", _character.IsAutomatic);

                _state = SquareStealState.SLECT_TARGET;

                //AI�̑I��
                if (_character.IsAutomatic)
                {
                    Invoke("AISelectStealCharacter", 1.5f);
                }
            }
            else
            {
                _state = SquareStealState.END;
            }
        }
    }

    private void AISelectStealCharacter()
    {
        // �^�C�v�ɂ���Ď�鏈����ς���
        //_character.CharacterType
        //var hasSouvenirCharacters = _otherCharacters.Where(x => x.Souvenirs.Count > 0).ToList();
        //if (hasSouvenirCharacters.Count == 0) return;

        // ���肪���[�`�̏ꍇ�j�~�������I�� �Y�����Ȃ��ꍇ�����_��
        for(int i = 0; i < _otherCharacters.Count; i++)
        {
            if (_otherCharacters[i].GetSouvenirTypeNum() + 1 == _gameManager.GetNeedSouvenirType())
            {
                _selectUI.IndexSelect(i);
                return;
            }
        }


        // �����̍ŏ��̈ʒu
        var item = Random.Range(0, _otherCharacters.Count);

        for(int i = 0; i < _otherCharacters.Count; i++)
        {
            if (_otherCharacters[item].Souvenirs.Count > 0)
            {
                _selectUI.IndexSelect(item);
                return;
            }
            item = MathUtils.Wrap(++item, 0, _otherCharacters.Count);
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
                var targetSouvenirIndex = Random.Range(0, _otherCharacters[_selectUI.SelectIndex].Souvenirs.Count);
                var target = _otherCharacters[_selectUI.SelectIndex].Souvenirs[targetSouvenirIndex];
                _character.AddSouvenir(target);
                _otherCharacters[_selectUI.SelectIndex].RemoveSouvenir(targetSouvenirIndex);

                var message = _character.Name + "��" + _otherCharacters[_selectUI.SelectIndex].Name + "��" + target.Name + "��D�����I";
                _messageWindow.SetMessage(message, _character.IsAutomatic);

                _souvenirWindow.SetSouvenirs(_character.Souvenirs);
                _souvenirWindow.SetEnable(true);

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

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // �R�X�g������Ȃ�
        if (_cost > character.Money) return base.GetScore(character, characterType);

        // �D�����̂�����
        if (_gameManager.GetCharacters(character).Where(x => x.Souvenirs.Count > 0).Count() == 0) return (int)SquareScore.NONE_STEAL + base.GetScore(character, characterType);

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
                        return (int)SquareScore.DONT_HAVE_SOUVENIR_TO_WIN + base.GetScore(character, characterType);
                    }
                    return (int)SquareScore.DONT_HAVE_STEAL + base.GetScore(character, characterType);
                }
            }
        }
        return (int)SquareScore.STEAL + base.GetScore(character, characterType);
    }
}

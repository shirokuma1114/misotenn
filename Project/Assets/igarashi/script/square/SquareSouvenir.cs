using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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


    [Header("���y�Y")]
    [Space(20)]
    [SerializeField]
    private string _souvenirName;
    [SerializeField]
    private int _cost;

    [SerializeField]
    private SouvenirType _type;

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();

        _squareInfo =
            "���y�Y�}�X\n" +
            "�R�X�g�F" + _cost.ToString() + "\n" +
            "���y�Y���F" + _souvenirName + "\n" +
            "���y�Y�^�C�v�F" + _type.ToString(); 
    }

    public override void Stop(CharacterBase character)
    {
        _character = character;

        
        //�����`�F�b�N
        if (!_character.CanPay(_cost))
        {
            _messageWindow.SetMessage("����������܂���", character.IsAutomatic);
            _state = SquareSouvenirState.END;
            return;
        }


        var message = _cost.ToString() + "�~���x������\n���y�Y�@" + _souvenirName + "���@�����܂����H";

        _messageWindow.SetMessage(message, character.IsAutomatic);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _state = SquareSouvenirState.PAY_WAIT;

        if (character.IsAutomatic)
        {
            Invoke("SelectAutomatic", 1.5f);
        }
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

    void SelectAutomatic()
    {
        _payUI.AISelectYes();
    }


    private void PayWaitProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
                _state = SquareSouvenirState.EVENT;
            }
            else
            {
                _state = SquareSouvenirState.END;
            }
        }            
    }

    private void EventProcess()
    {
        _character.SubMoney(_cost);
        _character.AddSouvenir(new Souvenir(_cost, _souvenirName, _type));

        _messageWindow.SetMessage(_character.Name + "��\n���y�Y�@" + _souvenirName + "���@��ɓ��ꂽ�I", _character.IsAutomatic); //_character.name + "��" + _souvenir.name + "����ɓ��ꂽ"

        _state = SquareSouvenirState.END;
    }

    private void EndProcess()
    {

        if(!_messageWindow.IsDisplayed)
        {
            // �~�܂鏈���I��
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);

            _state = SquareSouvenirState.IDLE;
        }
       
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // ����������Ȃ�
        if (_cost > character.Money) return base.GetScore(character, characterType);

        // �����Ă��Ȃ����y�Y�������Ă���
        if(character.Souvenirs.Where(x => x.Type == _type).Count() == 0)return (int)SquareScore.DONT_HAVE_SOUVENIR + base.GetScore(character, characterType);

        return (int)SquareScore.SOUVENIR + base.GetScore(character, characterType);
    }
}

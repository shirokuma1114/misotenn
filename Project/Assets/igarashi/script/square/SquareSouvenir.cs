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

    private CharacterBase _character;
    private MessageWindow _messageWindow;
    private StatusWindow _statusWindow;
    private PayUI _payUI;
    private SouvenirWindow _souvenirWindow;

    private OmiyageEnshutsu _effect;

    private int _nowStock;


    [Header("���y�Y")]
    [Space(20)]
    [SerializeField]
    private string _souvenirName;
    [SerializeField]
    private int _cost;

    [SerializeField]
    private SouvenirType _type;

    [SerializeField]
    private int _startStock = 3;

    bool _isEffectUsed = false;    


    private void Awake()
    {
        _nowStock = _startStock;
    }

    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
        _souvenirWindow = FindObjectOfType<SouvenirWindow>();

        _effect = FindObjectOfType<OmiyageEnshutsu>();

        _squareInfo =
            "���y�Y�}�X\n" +
            "�R�X�g�F" + _cost.ToString() + "\n" +
            "���y�Y���F" + _souvenirName + "\n" +
            "���y�Y�^�C�v�F" + _type.ToString() + "\n" +
            "�݌ɐ��F" + _nowStock.ToString();
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

        //�݌Ƀ`�F�b�N
        if (_nowStock <= 0)
        {
            _messageWindow.SetMessage("�݌ɂ�����܂���", character.IsAutomatic);
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
            Invoke("SelectAutomatic", 2.0f);
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
        _statusWindow.SetMoney(_character.Money);
        _character.AddSouvenir(SouvenirCreater.Instance.CreateSouvenir(_type));

        //�݌ɍX�V
        _nowStock--;

        var buyMessage = _character.Name + "��\n" + "���y�Y  " + _souvenirName + "���@��ɓ��ꂽ�I\n";
        if (_nowStock > 0)
        {
            buyMessage += "�c��̍݌ɂ�  " + _nowStock.ToString() + "��";
        }
        else
        {
            buyMessage += "�݌ɂ��@�Ȃ��Ȃ����I";
        }

        _messageWindow.SetMessage(buyMessage, _character.IsAutomatic);

        _souvenirWindow.SetSouvenirs(_character.Souvenirs);
        _souvenirWindow.SetEnable(true);

        //���o
        _effect.Use_OmiyageEnshutsu(gameObject.name);
        _isEffectUsed = true;

        _squareInfo =
            "���y�Y�}�X\n" +
            "�R�X�g�F" + _cost.ToString() + "\n" +
            "���y�Y���F" + _souvenirName + "\n" +
            "���y�Y�^�C�v�F" + _type.ToString() + "\n" +
            "�݌ɐ��F" + _nowStock.ToString();

        _state = SquareSouvenirState.END;
    }

    private void EndProcess()
    {
        if (_isEffectUsed)
        {
            if (!_messageWindow.IsDisplayed && _effect.IsAnimComplete)
            {
                // �~�܂鏈���I��
                _character.CompleteStopExec();
                _statusWindow.SetEnable(false);
                _souvenirWindow.SetEnable(false);

                _state = SquareSouvenirState.IDLE;
                _isEffectUsed = false;
            }
        }
        else
        {
            if (!_messageWindow.IsDisplayed)
            {
                // �~�܂鏈���I��
                _character.CompleteStopExec();
                _statusWindow.SetEnable(false);

                _state = SquareSouvenirState.IDLE;
            }
        }
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // ����������Ȃ�
        if (_cost > character.Money) return base.GetScore(character, characterType);

        // �݌ɂ��Ȃ�
        if (_nowStock <= 0) return base.GetScore(character, characterType);

        // �����Ă��Ȃ����y�Y�������Ă���
        if (character.Souvenirs.Where(x => x.Type == _type).Count() == 0)return (int)SquareScore.DONT_HAVE_SOUVENIR + base.GetScore(character, characterType);

        return (int)SquareScore.SOUVENIR + base.GetScore(character, characterType);
    }
}
